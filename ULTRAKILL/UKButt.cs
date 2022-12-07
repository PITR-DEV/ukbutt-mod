using System.Collections.Generic;
using System.Threading.Tasks;
using GameConsole;

namespace UKButt.Commands
{
    public class UKButt : BetterCommandRoot
    {
        public override string Name => "UKButt";
        public override string Description => "Buttplug.io support";
        
        protected override void BuildTree(Console con)
        {
            BuildPrefsEditor(new List<PrefReference>
            {
                new PrefReference
                {
                    Key = UKButtProperties.SocketUri,
                    Local = true,
                    Type = typeof(string),
                    Default = "ws://localhost:12345"
                },
                new PrefReference
                {
                    Key = UKButtProperties.Strength,
                    Local = true,
                    Type = typeof(float),
                    Default = "0.8"
                },
                new PrefReference
                {
                    Key = UKButtProperties.EnableMenuHaptics,
                    Local = true,
                    Type = typeof(bool),
                    Default = "True"
                },
                new PrefReference
                {
                    Key = UKButtProperties.UseUnscaledTime,
                    Local = true,
                    Type = typeof(bool),
                    Default = "True"
                },
                new PrefReference
                {
                    Key = UKButtProperties.StickForSeconds,
                    Local = true,
                    Type = typeof(float),
                    Default = "2.0"
                },
                new PrefReference
                {
                    Key = UKButtProperties.TapStickForSeconds,
                    Local = true,
                    Type = typeof(float),
                    Default = "0.2"
                },
            });
            
            Leaf("restart_client", () =>
            {
                Task.Run(ButtplugManager.Instance.RestartClient);
            });
        }
    }
}