using System.Collections.Generic;
using System.Threading.Tasks;
using GameConsole;
using GameConsole.CommandTree;

namespace UKButt.Commands
{
    public class UKButt : CommandRootBackport
    {
        public UKButt(Console con) : base(con) { }
        public override string Name => "UKButt";
        public override string Description => "Buttplug.io support";
        
        protected override Branch BuildTree(Console con)
        {
            return Branch(
                "ukbutt", requireCheats: false, children: new Node[]
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
                        new PrefReference
                        {
                            Key = UKButtProperties.InputMode,
                            Local = true,
                            Type = typeof(int),
                            Default = "1"
                        },
                        new PrefReference
                        {
                            Key = UKButtProperties.LinearPosMin,
                            Local = true,
                            Type = typeof(float),
                            Default = "0.1"
                        },
                        new PrefReference
                        {
                            Key = UKButtProperties.LinearPosMax,
                            Local = true,
                            Type = typeof(float),
                            Default = "0.9"
                        },
                        new PrefReference
                        {
                            Key = UKButtProperties.LinearTimeMin,
                            Local = true,
                            Type = typeof(float),
                            Default = "0.3"
                        },
                        new PrefReference
                        {
                            Key = UKButtProperties.LinearTimeMax,
                            Local = true,
                            Type = typeof(float),
                            Default = "1.5"
                        },
                        new PrefReference
                        {
                            Key = UKButtProperties.StrokeWhileIdle,
                            Local = true,
                            Type = typeof(bool),
                            Default = "False"
                        },
                    }),
                    Leaf("restart_client", () =>
                    {
                        Task.Run(ButtplugManager.Instance.RestartClient);
                    })
                });

        }
    }
}