using UnityEngine;

namespace UKButt.ULTRAKILL
{
    public static class PrefsManagerHelper
    {
        // Dynamic set method. Sometimes using separate methods for different types is annoying.
        // They're just prefs
        public static void SetPref(string key, object value, bool noSteamCloud)
        {
            if (value is float floatValue)
            {
                if (noSteamCloud)
                {
                    PrefsManager.Instance.SetFloatLocal(key, floatValue);
                }
                else
                {
                    PrefsManager.Instance.SetFloat(key, floatValue);
                }
            }
            else if (value is int intValue)
            {
                if (noSteamCloud)
                {
                    PrefsManager.Instance.SetIntLocal(key, intValue);
                }
                else
                {
                    PrefsManager.Instance.SetInt(key, intValue);
                }
            }
            else if (value is string stringValue)
            {
                if (noSteamCloud)
                {
                    PrefsManager.Instance.SetStringLocal(key, stringValue);
                }
                else
                {
                    PrefsManager.Instance.SetString(key, stringValue);
                }
            }
            else if (value is bool boolValue)
            {
                if (noSteamCloud)
                {
                    PrefsManager.Instance.SetBoolLocal(key, boolValue);
                }
                else
                {
                    PrefsManager.Instance.SetBool(key, boolValue);
                }
            }
            else
            {
                Debug.LogError("Unsupported type for PrefsManagerHelper.SetPref");
            }
        }
    }
}