namespace UKButt
{
    public static class UKButtProperties
    {
        // Keys used for storing preferences
        
        public static readonly string SocketUri = "ukbutt.socketUri";
        
        // Adjustments
        public static readonly string Strength = "ukbutt.strength";
        
        // Default: 2.0
        // Defines for how long a typical vibration lasts
        public static readonly string StickForSeconds = "ukbutt.stickForSeconds";
        
        // Default: 0.2
        // Same as above, but for subtle vibrations. I.e. menu feedback
        public static readonly string TapStickForSeconds = "ukbutt.stickForSeconds";
        
        // Toggles
        public static readonly string UseUnscaledTime = "ukbutt.useUnscaledTime";
        public static readonly string EnableMenuHaptics = "ukbutt.enableMenuHaptics";
    }
}