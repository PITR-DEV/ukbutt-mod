﻿using System;
using System.Collections.Generic;
using BepInEx;
using Buttplug;
using ButtplugUnity;
using UnityEngine;

namespace UKButt
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.NAME, PluginInfo.VERSION)]
    public class ButtplugManager : BaseUnityPlugin
    {
        public static ButtplugManager Instance;
        public bool emergencyStop = false;
        
        private ButtplugUnityClient unityButtClient;
        private readonly List<ButtplugClientDevice> connectedDevices = new List<ButtplugClientDevice>();
        public float currentSpeed = 0;
        private UnscaledTimeSince timeSinceVibes;

        // TODO single class to store the defaults for this and the UKButt command prefs editor
        private float StickForNormal => PrefsManager.Instance == null ? 2.0f : PrefsManager.Instance.GetFloatLocal(UKButtProperties.StickForSeconds, 2.0f);
        private float SoftStickFor => PrefsManager.Instance == null ? 0.2f : PrefsManager.Instance.GetFloatLocal(UKButtProperties.TapStickForSeconds, 0.2f);
        private float StrengthMultiplier => PrefsManager.Instance == null ? 0.8f : PrefsManager.Instance.GetFloatLocal(UKButtProperties.Strength, 0.8f);

        private void Awake()
        {
            Logger.LogInfo("Initializing UKButt");
            Instance = this;

            var harmony = HarmonyLib.Harmony.CreateAndPatchAll(typeof(CameraPatch));
            harmony.PatchAll(typeof(CameraPatch)); // Intercepts shake calls
            harmony.PatchAll(typeof(CheatsPatch)); // Adds the bindable emergency stop cheat
            harmony.PatchAll(typeof(RevolverPatch)); // Intercepts the revolver's firing
            harmony.PatchAll(typeof(DashPatch)); // Intercepts the player dash/dodge

            // Register the command to the ULTRAKILL console
            GameConsole.Console.Instance.RegisterCommand(new Commands.UKButt());

            // Connect the buttplug.io client
            RestartClient();
        }

        public async void RestartClient()
        {
            if (unityButtClient != null)
            {
                Debug.Log("Disconnecting from Buttplug server");
                await unityButtClient.DisconnectAsync();
            }
            
            unityButtClient = new ButtplugUnityClient("ULTRAKILL");
            unityButtClient.DeviceAdded += AddDevice;
            unityButtClient.DeviceRemoved += RemoveDevice;
            unityButtClient.ScanningFinished += ScanningFinished;
            
            Debug.Log("Connecting to Buttplug server");
            await unityButtClient.ConnectAsync(new ButtplugWebsocketConnectorOptions(new Uri($"{PrefsManager.Instance.GetStringLocal(UKButtProperties.SocketUri, "ws://localhost:12345")}/buttplug")));
        
            var startScanningTask = unityButtClient.StartScanningAsync();
            try
            {
                await startScanningTask;
            }
            catch (ButtplugException ex)
            {
                Console.WriteLine(
                    $"Scanning failed: {ex.InnerException.Message}");
            }
        }

        public void Vibrate(float originalAmount)
        {
            Debug.Log("Intercepted vibration call " + originalAmount);
            timeSinceVibes = 0;
            var amount = Mathf.Clamp(originalAmount, 0, 1);
            currentSpeed = amount;
        }

        public void Tap()
        {
            timeSinceVibes = StickForNormal - SoftStickFor;
            currentSpeed = 0.1f;
        }
        
        private void Update()
        {
            if (emergencyStop) currentSpeed = 0;
            
            foreach (var buttplugClientDevice in connectedDevices)
            {
                // Debug.Log("Sending vibration to " + buttplugClientDevice.Name + " at " + currentSpeed);
                buttplugClientDevice.SendVibrateCmd(currentSpeed * StrengthMultiplier);
            }

            if (timeSinceVibes > StickForNormal) currentSpeed = 0;
            // TODO maybe add gradual falloff?
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
            unityButtClient?.DisconnectAsync().Wait();
        }
    }
}