using System.Collections.Generic;
using System.Globalization;
using GameConsole;
using GameConsole.CommandTree;

namespace UKButt.Commands
{
    public abstract class CommandRootBackport : CommandRoot
    {
        protected CommandRootBackport(Console con) : base(con) { }
        
        public abstract override string Name { get; }
        public abstract override string Description { get; }

        protected abstract override Branch BuildTree(Console con);
        
        private const string KeyColor = "#db872c";
        private const string TypeColor = "#879fff";
        private const string ValueColor = "#4ac246";

        public new Branch BuildPrefsEditor(List<PrefReference> pref) =>
            Leaf("prefs", () =>
            {
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

                        Console.Instance.PrintLine(
                            $"- <color={KeyColor}>{p.Key}</color>: <color={ValueColor}>{valueString}</color>   [<color={TypeColor}>float</color>] {isLocalString}");
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

                        Console.Instance.PrintLine(
                            $"- <color={KeyColor}>{p.Key}</color>: <color={ValueColor}>{valueString}</color>   [<color={TypeColor}>float</color>] {isLocalString}");
                    }
                    else if (p.Type == typeof(string))
                    {
                        var currentValue = p.Local
                            ? PrefsManager.Instance.GetStringLocal(p.Key)
                            : PrefsManager.Instance.GetString(p.Key);
                        Console.Instance.PrintLine(
                            $"- <color={KeyColor}>{p.Key}</color>: <color={ValueColor}>\"{(string.IsNullOrEmpty(currentValue) ? p.Default : currentValue)}\"</color>   [<color={TypeColor}>float</color>] {isLocalString}");
                    }
                    else
                    {
                        Console.Instance.PrintLine($"Pref {p.Key} is type {p.Type.Name} (Unrecognized)");
                    }
                }

                Console.Instance.PrintLine(
                    $"You can use `<color=#7df59d>prefs set <type> <value></color>` to change a pref");
                Console.Instance.PrintLine(
                    $"or `<color=#7df59d>prefs set_local <type> <value></color>` to change a <color={KeyColor}>local</color> pref. (it matters)");
            });
    }
}