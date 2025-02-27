namespace PukiTools.GodotSharp.SourceGenerators
{
    public static class GenerationConstants
    {
        public const string NodeClass = "Godot.Node";

        public const string SignalAttr = "Godot.SignalAttribute";
        
        public const string AutoloadAttr = "PukiTools.GodotSharp.AutoloadAttribute";
        
        public const string ProjectSettingAttr = "PukiTools.GodotSharp.ProjectSettingAttribute";
        
        public const string UserSettingsData = "PukiTools.GodotSharp.UserSettingsData";
        
        public const string UserSettingsInstance = "PukiTools.GodotSharp.UserSettingsInstance";
        
        public const string UserSettingAttributeData = "PukiTools.GodotSharp.UserSettingAttributeData";
        
        // TODO: I'm sure there are other C# keywords, put them here.
        public static string[] Keywords = { "event" };
    }   
}