using System;

namespace UKButt.Commands
{
    public class UKButtCycleMode : ICheat
    {
        public string LongName => "UKButt Input Mode";
        public string Identifier => "ukbutt.cycle-mode";
        public string ButtonEnabledOverride => null;
        public string ButtonDisabledOverride => Enum.GetName(typeof(InputMode), ButtplugManager.Instance.InputMode);
        public string Icon => null;
        public bool IsActive => false;
        public bool DefaultState => false;
        public StatePersistenceMode PersistenceMode => StatePersistenceMode.NotPersistent;
        
        public void Enable(CheatsManager manager)
        {
            var inputMode = (int)ButtplugManager.Instance.InputMode;
            var modesCount = Enum.GetValues(typeof(InputMode)).Length;
            
            inputMode = (inputMode + 1) % modesCount;
            PrefsManager.Instance.SetIntLocal(UKButtProperties.InputMode, inputMode);
        }
        
        public void Disable() { } // Not needed
    }
}