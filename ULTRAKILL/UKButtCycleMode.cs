namespace UKButt.Commands
{
    public class UKButtCycleMode : ICheat
    {
        public string LongName => "UKButt Input Mode:";
        public string Identifier => "ukbutt.cycle-mode";
        public string ButtonEnabledOverride => null;
        public string ButtonDisabledOverride => ButtplugManager.Instance.InputMode == InputMode.Varied ? "Varied" : "Rank Continuous";
        public string Icon => null;
        public bool IsActive => false;
        public bool DefaultState => false;
        public StatePersistenceMode PersistenceMode => StatePersistenceMode.NotPersistent;
        
        public void Enable()
        {
            // The cheat is permanently set to false, meaning that Enable will always be called on click.
            if (ButtplugManager.Instance.InputMode == InputMode.Varied) PrefsManager.Instance.SetIntLocal(UKButtProperties.InputMode, (int)InputMode.ContinuousRank);
            else PrefsManager.Instance.SetIntLocal(UKButtProperties.InputMode, (int)InputMode.Varied);
        }
        
        public void Disable() { } // Not needed
        public void Update() { } // Not needed
    }
}