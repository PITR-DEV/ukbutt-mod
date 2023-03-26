using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BepInEx;
using ButtplugManaged;
using UnityEngine;

namespace UKButt
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.NAME, PluginInfo.VERSION)]
    public class ButtplugManager : BaseUnityPlugin
    {
        public static ButtplugManager Instance;
        public bool emergencyStop = false;

        private ButtplugClient buttplugClient;
        private readonly List<ButtplugClientDevice> connectedDevices = new List<ButtplugClientDevice>();

        public float currentSpeed = 0;
        public float currentRank = 0;
        public float currentLinearTime = 0;

        private UnscaledTimeSince _unscaledTimeSinceVibes;
        private UnscaledTimeSince _timeSinceVibes;
        private UnscaledTimeSince _timeSinceVibeUpdate;

        private float TimeSinceVibes => PrefsManager.Instance.GetBoolLocal(UKButtProperties.UseUnscaledTime, true) ? (float)_unscaledTimeSinceVibes : (float)_timeSinceVibes;

        // TODO single class to store the defaults for this and the UKButt command prefs editor
        private float StickForNormal => PrefsManager.Instance == null ? 2.0f : PrefsManager.Instance.GetFloatLocal(UKButtProperties.StickForSeconds, 2.0f);
        private float SoftStickFor => PrefsManager.Instance == null ? 0.2f : PrefsManager.Instance.GetFloatLocal(UKButtProperties.TapStickForSeconds, 0.2f);
        private float StrengthMultiplier => PrefsManager.Instance == null ? 0.8f : PrefsManager.Instance.GetFloatLocal(UKButtProperties.Strength, 0.8f);
        public InputMode InputMode => (InputMode)PrefsManager.Instance.GetIntLocal(UKButtProperties.InputMode, (int)InputMode.Varied);
        public static bool ForwardPatchedEvents => Instance != null && Instance.InputMode == InputMode.Varied;
        
        private float LinearPosMin => PrefsManager.Instance == null ? 0.1f : PrefsManager.Instance.GetFloatLocal(UKButtProperties.LinearPosMin, 0.1f);
        private float LinearPosMax => PrefsManager.Instance == null ? 0.9f : PrefsManager.Instance.GetFloatLocal(UKButtProperties.LinearPosMax, 0.9f);
        private float LinearTimeMin => PrefsManager.Instance == null ? 0.3f : PrefsManager.Instance.GetFloatLocal(UKButtProperties.LinearTimeMin, 0.3f);
        private float LinearTimeMax => PrefsManager.Instance == null ? 1.5f : PrefsManager.Instance.GetFloatLocal(UKButtProperties.LinearTimeMax, 1.5f);

        private bool StrokeWhileIdle => PrefsManager.Instance != null && PrefsManager.Instance.GetBoolLocal(UKButtProperties.StrokeWhileIdle, false);

        // Toggle for movement direction state
        private bool _moveMax = true;

        // Current setting for stroke time, recalculated based on rank at the end of each stroke
        private float _moveTime;

        // Timer for calculating stroke command frequency
        private TimeSince _timeSinceLastMove;

        private void Awake()
        {
            Logger.LogInfo("Initializing UKButt");
            Instance = this;
            // Start at the slowest Linear time.
            _moveTime = LinearTimeMax;

            var harmony = HarmonyLib.Harmony.CreateAndPatchAll(typeof(CameraPatch));
            harmony.PatchAll(typeof(CameraPatch)); // Intercepts shake calls
            harmony.PatchAll(typeof(CheatsPatch)); // Adds the bindable emergency stop cheat
            harmony.PatchAll(typeof(RevolverPatch)); // Intercepts the revolver's firing
            harmony.PatchAll(typeof(DashPatch)); // Intercepts the player dash/dodge
            harmony.PatchAll(typeof(ButtonPatch)); // Intercepts the button press

            // Rank based input mode
            harmony.PatchAll(typeof(StyleAscendRank));
            harmony.PatchAll(typeof(StyleDescendRank));

            // Register the command to the ULTRAKILL console
            GameConsole.Console.Instance.RegisterCommand(new Commands.UKButt());

            // Connect the buttplug.io client
            Task.Run(RestartClient);
        }

        public async void RestartClient()
        {
            if (buttplugClient != null)
            {
                Logger.LogInfo("Disconnecting from Buttplug server");
                await buttplugClient.DisconnectAsync();
                buttplugClient = null;
            }

            buttplugClient = new ButtplugClient("ULTRAKILL");
            buttplugClient.DeviceAdded += AddDevice;
            buttplugClient.DeviceRemoved += RemoveDevice;
            buttplugClient.ScanningFinished += ScanningFinished;
            
            Logger.LogInfo("Connecting to Buttplug server");
            await buttplugClient.ConnectAsync(new ButtplugWebsocketConnectorOptions(new Uri($"{PrefsManager.Instance.GetStringLocal(UKButtProperties.SocketUri, "ws://localhost:12345")}/buttplug")));

            var startScanningTask = buttplugClient.StartScanningAsync();
            try
            {
                await startScanningTask;
            }
            catch (ButtplugException ex)
            {
                Logger.LogError($"Scanning failed: {ex.InnerException.Message}");
            }
        }

        public static void Vibrate(float originalAmount)
        {
            if (!Instance || Instance.emergencyStop) return;
            ResetVibeTimes();
            var amount = Mathf.Clamp(originalAmount, 0, 1);
            Instance.currentSpeed = amount;
        }

        // Used for very subtle vibrations (menu button clicks and dashes)
        public static void Tap(bool isMenu = false)
        {
            if (!Instance || Instance.emergencyStop) return;
            if (isMenu && !PrefsManager.Instance.GetBoolLocal(UKButtProperties.EnableMenuHaptics, true)) return;
            ResetVibeTimes();
            if (Instance.currentSpeed < 0.1f)
            {
                Instance.currentSpeed = 0.1f;
            }
        }

        private void Update()
        {
            if (buttplugClient == null) return;
            if (emergencyStop) currentSpeed = 0;


            if (InputMode == InputMode.Varied) UpdateHookArm();
            else if (InputMode == InputMode.ContinuousRank)
            {
                if (StyleHUD.Instance == null) currentRank = 0;
                currentSpeed = currentRank / 8f;
            }
            else if (InputMode == InputMode.Passthrough)
            {

                if (OptionsManager.Instance && OptionsManager.Instance.paused)
                {
                    currentSpeed = 0;
                }
                else
                {
                    currentSpeed = RumbleManager.Instance.currentIntensity;
                }
            }

            // This shouldn't be run at more than 10hz, bluetooth can't keep up. Repeated commands will be
            // ignored in Buttplug, but quick updates can still cause lag.
            if (_timeSinceVibeUpdate > 0.10)
            {
                foreach (var buttplugClientDevice in connectedDevices)
                {
                    if (buttplugClientDevice.AllowedMessages.ContainsKey("VibrateCmd"))
                    {
                        buttplugClientDevice.SendVibrateCmd(Math.Min(currentSpeed * StrengthMultiplier, 1.0));
                    }
                    // Only trigger stroker movement if we're using continuous rank mode. Variable mode doesn't make sense for this.
                    if (InputMode == InputMode.ContinuousRank && (StrokeWhileIdle || currentSpeed > 0.00001)) {
                        if (buttplugClientDevice.AllowedMessages.ContainsKey("LinearCmd"))
                        {
                            if (_timeSinceLastMove > _moveTime)
                            {
                                _moveTime = Math.Max(LinearTimeMax - ((LinearTimeMax - LinearTimeMin) * (currentSpeed * StrengthMultiplier)), LinearTimeMin);
                                buttplugClientDevice.SendLinearCmd((uint)(1000 * _moveTime), _moveMax ? LinearPosMin : LinearPosMax);
                            }
                        }
                    } 
                    else
                    {
                        // This resets our move time so that once MoveWhileIdle or speed goes > 1, we'll be sure to actually trigger a command.
                        _moveTime = 0;
                    }
                }
                // On the extremely rare chance someone has multiple linear devices, don't reset values
                // until after commands have been sent to all of them.
                //
                // Also, don't run this if we're not stroking already.
                if (_moveTime > 0 && _timeSinceLastMove > _moveTime)
                {
                    _timeSinceLastMove = 0;
                    _moveMax = !_moveMax;
                }
                _timeSinceVibeUpdate = 0;
            }

            if (TimeSinceVibes > StickForNormal && InputMode == InputMode.Varied) currentSpeed = 0;
            else if (InputMode == InputMode.None) currentSpeed = 0;
        }

        private static void ResetVibeTimes()
        {
            Instance._timeSinceVibes = Instance.StickForNormal - Instance.SoftStickFor;
            Instance._unscaledTimeSinceVibes = Instance.StickForNormal - Instance.SoftStickFor;
        }

        private void UpdateHookArm()
        {
            if (!HookArm.Instance) return;
            switch (HookArm.Instance.state)
            {
                case HookState.Pulling:
                    Vibrate(0.5f);
                    ResetVibeTimes();
                    break;
                case HookState.Throwing:
                    Vibrate(0.2f);
                    ResetVibeTimes();
                    break;
            }
        }

        private void AddDevice(object sender, DeviceAddedEventArgs args)
        {
            Debug.Log("Device Added: " + args.Device.Name);
            connectedDevices.Add(args.Device);
        }

        private void RemoveDevice(object sender, DeviceRemovedEventArgs args)
        {
            Debug.Log("Device Removed: " + args.Device.Name);
            connectedDevices.Remove(args.Device);
        }

        private void ScanningFinished(object sender, EventArgs args)
        {
            Debug.Log("Scanning Finished");
        }

        private void OnDestroy()
        {
            buttplugClient?.DisconnectAsync().Wait();
        }
    }
}
