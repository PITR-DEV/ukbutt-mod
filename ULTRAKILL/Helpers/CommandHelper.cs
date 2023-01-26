using System;
using System.Collections.Generic;
using System.Globalization;

namespace GameConsole
{
    // TODO Move to the game itself
    // and update this mod to use the new API once it's available
    
    public abstract class BetterCommandRoot : CommandRoot
    {
        private const string KeyColor = "#db872c";
        private const string TypeColor = "#879fff";
        private const string ValueColor = "#4ac246";

        public void BuildPrefsEditor(List<PrefReference> pref)
        {
            Leaf("prefs", () =>
                {
                    if (Console.Instance.CheatBlocker()) return;
                    Console.Instance.PrintLine("Available prefs:");
                    foreach (var p in pref)
                    {
                        var isLocalString = p.Local ? "<color=red>LOCAL</color>" : string.Empty;
                        if (p.Type == typeof(int))
                        {
                            string valueString;
                            if (!PrefsManager.Instance.HasKey(p.Key))
                            {
                                valueString = string.IsNullOrEmpty(p.Default)
                                    ? "<color=red>NOT SET</color>"
                                    : p.Default;
                            }
                            else
                            {
                                var currentValue = p.Local
                                    ? PrefsManager.Instance.GetIntLocal(p.Key)
                                    : PrefsManager.Instance.GetInt(p.Key);
                                valueString = currentValue.ToString();
                            }
                            
                            Console.Instance.PrintLine(
                                $"- <color={KeyColor}>{p.Key}</color>: <color={ValueColor}>{valueString}</color>   [<color={TypeColor}>int</color>] {isLocalString}");
                        }
                        else if (p.Type == typeof(float))
                        {
                            string valueString;
                            if (!PrefsManager.Instance.HasKey(p.Key))
                            {
                                valueString = string.IsNullOrEmpty(p.Default)
                                    ? "<color=red>NOT SET</color>"
                                    : p.Default;
                            }
                            else
                            {
                                var currentValue = p.Local
                                    ? PrefsManager.Instance.GetFloatLocal(p.Key)
                                    : PrefsManager.Instance.GetFloat(p.Key);
                                valueString = currentValue.ToString(CultureInfo.InvariantCulture);
                            }

                            Console.Instance.PrintLine($"- <color={KeyColor}>{p.Key}</color>: <color={ValueColor}>{valueString}</color>   [<color={TypeColor}>float</color>] {isLocalString}");
                        }
                        else if (p.Type == typeof(bool))
                        {
                            string valueString;
                            if (!PrefsManager.Instance.HasKey(p.Key))
                            {
                                valueString = string.IsNullOrEmpty(p.Default)
                                    ? "<color=red>NOT SET</color>"
                                    : p.Default;
                            }
                            else
                            {
                                var currentValue = p.Local
                                    ? PrefsManager.Instance.GetBoolLocal(p.Key)
                                    : PrefsManager.Instance.GetBool(p.Key);
                                valueString = currentValue ? "True" : "False";
                            }

                            Console.Instance.PrintLine($"- <color={KeyColor}>{p.Key}</color>: <color={ValueColor}>{valueString}</color>   [<color={TypeColor}>bool</color>] {isLocalString}");
                        }
                        else if (p.Type == typeof(string))
                        {
                            var currentValue = p.Local
                                ? PrefsManager.Instance.GetStringLocal(p.Key)
                                : PrefsManager.Instance.GetString(p.Key);
                            Console.Instance.PrintLine($"- <color={KeyColor}>{p.Key}</color>: <color={ValueColor}>\"{(string.IsNullOrEmpty(currentValue) ? p.Default : currentValue)}\"</color>   [<color={TypeColor}>string</color>] {isLocalString}");
                        }
                        else
                        {
                            Console.Instance.PrintLine($"Pref {p.Key} is type {p.Type.Name} (Unrecognized)");
                        }
                    }
                    
                    Console.Instance.PrintLine($"You can use `<color=#7df59d>prefs set <type> <pref> <value></color>` to change a pref");
                    Console.Instance.PrintLine($"or `<color=#7df59d>prefs set_local <type> <pref> <value></color>` to change a <color=red>LOCAL</color> pref. (it matters)");
                    // Console.Instance.PrintLine($"`<color=#c1e6f7>{Name.ToLower()} prefs</color>` is <color=orange>different from</color> `<color=#c1e6f7>prefs</color>`");
                    
                });
        }

        public class PrefReference
        {
            public string Key;
            public Type Type;
            public bool Local;
            public string Default;
        }
    }
}