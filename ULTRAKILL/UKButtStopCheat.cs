namespace UKButt.Commands
{
    public class UKButtStopCheat : ICheat
    {
        public string LongName => "Stop UKButt";
        public string Identifier => "ukbutt.stop";
        public string ButtonEnabledOverride => "UnPause";
        public string ButtonDisabledOverride => "Pause Everything";
        public string Icon => null;
        public bool IsActive => ButtplugManager.Instance.emergencyStop;
        public bool DefaultState => false;
        public StatePersistenceMode PersistenceMode => StatePersistenceMode.NotPersistent;
        
        public void Enable(CheatsManager manager)
        {
            ButtplugManager.Instance.emergencyStop = true;
        }
        public void Disable()
        {
            ButtplugManager.Instance.emergencyStop = false;
        }

        public void Update() { } // Not needed
    }
}