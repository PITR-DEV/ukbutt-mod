using System.Collections.Generic;
using HarmonyLib;
using UKButt.Commands;
using UnityEngine.UI;

namespace UKButt
{
    [HarmonyPatch(typeof(CameraController), nameof(CameraController.CameraShake))]
    public class CameraPatch
    {
        static void Postfix(float shakeAmount)
        {
            if (!ButtplugManager.ForwardPatchedEvents) return;
            ButtplugManager.Vibrate(shakeAmount);
        }
    }
    
    // patch private OnPointerClick
    [HarmonyPatch(typeof(Button), "Press")]
    public class ButtonPatch
    {
        static void Postfix()
        {
            if (!ButtplugManager.ForwardPatchedEvents) return;
            ButtplugManager.Tap(true);
        }
    }
    
    // patch CheatsManager.Start
    [HarmonyPatch(typeof(CheatsManager), "Start")]
    public class CheatsPatch
    {
        static void Postfix()
        {
            // Register ULTRAKILL cheats
            CheatsManager.Instance.RegisterExternalCheat(new UKButtStopCheat());
            CheatsManager.Instance.RegisterExternalCheat(new UKButtCycleMode());
            CheatBinds.Instance.RestoreBinds(new Dictionary<string, List<ICheat>>
            {
                {
                    "EXTERNAL",
                    new List<ICheat>
                    {
                        new UKButtStopCheat(),
                        new UKButtCycleMode()
                    }
                }
            });
            CheatsManager.Instance.RebuildMenu();
        }
    }
    
    [HarmonyPatch(typeof(Revolver), "Shoot")]
    public class RevolverPatch
    {
        static void Postfix(int shotType = 1)
        {
            if (!ButtplugManager.ForwardPatchedEvents) return;
            if (shotType == 1)
            {
                ButtplugManager.Vibrate(0.5f);
            }
            else
            {
                ButtplugManager.Vibrate(1f);
            }
            
        }
    }
    
    [HarmonyPatch(typeof(NewMovement), "Dodge")]
    public class DashPatch
    {
        static void Postfix()
        {
            if (!ButtplugManager.ForwardPatchedEvents) return;
            ButtplugManager.Tap();
        }
    }
    
    [HarmonyPatch(typeof(StyleHUD), "AscendRank")]
    public class StyleAscendRank
    {
        static void Postfix()
        {
            ButtplugManager.Instance.currentRank = StyleHUD.Instance.rankIndex;
        }
    }
    
    [HarmonyPatch(typeof(StyleHUD), "DescendRank")]
    public class StyleDescendRank
    {
        static void Postfix()
        {
            ButtplugManager.Instance.currentRank = StyleHUD.Instance.rankIndex;
        }
    }
}