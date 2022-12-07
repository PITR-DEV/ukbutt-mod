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
            ButtplugManager.Vibrate(shakeAmount);
        }
    }
    
    // patch private OnPointerClick
    [HarmonyPatch(typeof(Button), "Press")]
    public class ButtonPatch
    {
        static void Postfix()
        {
            ButtplugManager.Tap(true);
        }
    }
    
    // patch CheatsManager.Start
    [HarmonyPatch(typeof(CheatsManager), "Start")]
    public class CheatsPatch
    {
        static void Postfix()
        {
            // Register the ULTRAKILL cheat
            CheatsManager.Instance.RegisterExternalCheat(new UKButtStopCheat());
            CheatBinds.Instance.RestoreBinds(new Dictionary<string, List<ICheat>>
            {
                {
                    "EXTERNAL",
                    new List<ICheat>
                    {
                        new UKButtStopCheat()
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
            ButtplugManager.Tap();
        }
    }
}