namespace PukiTools.Godot.SourceGenerators
{
    public static class GenerationConstants
    {
        public const string NodeClass = "Godot.Node";

        public const string SignalAttr = "Godot.SignalAttribute";
        
        public const string AutoloadAttr = "PukiTools.Godot.AutoloadAttribute";
        
        public const string ProjectSettingAttr = "PukiTools.Godot.ProjectSettingAttribute";
        
        public const string UserSettingsData = "PukiTools.Godot.UserSettingsData";
        
        public const string UserSettingsInstance = "PukiTools.Godot.UserSettingsInstance";
        
        public const string UserSettingAttributeData = "PukiTools.Godot.UserSettingAttributeData";
        
        // TODO: I'm sure there are other C# keywords, put them here.
        public static string[] Keywords = { "event" };
    }   
}