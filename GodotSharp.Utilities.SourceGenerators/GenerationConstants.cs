namespace GodotSharp.Utilities.SourceGenerators
{
    public static class GenerationConstants
    {
        public const string NodeClass = "Godot.Node";

        public const string SignalAttr = "Godot.SignalAttribute";
        
        public const string AutoloadAttr = "GodotSharp.Utilities.AutoloadAttribute";
        
        public const string ProjectSettingAttr = "GodotSharp.Utilities.ProjectSettingAttribute";
        
        public const string UserSettingsData = "GodotSharp.Utilities.UserSettingsData";
        
        public const string UserSettingsInstance = "GodotSharp.Utilities.UserSettingsInstance";
        
        public const string UserSettingAttributeData = "GodotSharp.Utilities.UserSettingAttributeData";
        
        // TODO: I'm sure there are other C# keywords, put them here.
        public static string[] Keywords = { "event" };
    }   
}