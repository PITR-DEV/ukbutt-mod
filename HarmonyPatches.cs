using System.Collections.Generic;
using HarmonyLib;
using UKButt.Commands;

namespace UKButt
{
    [HarmonyPatch(typeof(CameraController), nameof(CameraController.CameraShake))]
    public class CameraPatch
    {
        static void Postfix(float shakeAmount)
        {
            ButtplugManager.Instance.Vibrate(shakeAmount);
        }
    }
    
    // patch private OnPointerClick
    [HarmonyPatch(typeof(ShopButton), "OnPointerClick")]
    public class ShopPatch
    {
        static void Postfix()
        {
            ButtplugManager.Instance.Tap();
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
                ButtplugManager.Instance.Vibrate(0.5f);
            }
            else
            {
                ButtplugManager.Instance.Vibrate(1f);
            }
            
        }
    }
    
    [HarmonyPatch(typeof(NewMovement), "Dodge")]
    public class DashPatch
    {
        static void Postfix()
        {
            ButtplugManager.Instance.Tap();
        }
    }
}