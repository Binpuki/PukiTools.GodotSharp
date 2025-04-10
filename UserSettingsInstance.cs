namespace PukiTools.GodotSharp;

[GlobalClass, Autoload("UserSettings")]
public partial class UserSettingsInstance : Node
{
    [Signal] public delegate void SettingsChangedEventHandler();
    
    private UserSettingsData _data;

    public override void _Ready()
    {
        if (Load() != Error.Ok)
            Reset();
        
        Save();
        UpdateSettings();
    }

    public void UpdateSettings()
    {
        EmitSignalSettingsChanged();
    }

    public Error Load(string path = null)
    {
        path ??= ProjectSettings.GetSetting("puki_tools/general/settings_path").AsString();
        if (!FileAccess.FileExists(path))
            return Error.FileNotFound;

        ConfigFile config = new();
        Error loadError = config.Load(path);
        if (loadError != Error.Ok)
            return loadError;

        Reset();
        _data.Load(config);
        return Error.Ok;
    }

    public Error Save(string path = null)
    {
        path ??= ProjectSettings.GetSetting("puki_tools/general/settings_path").AsString();
        ConfigFile configFile = _data.CreateConfigFileInstance();

        return configFile.Save(path);
    }

    public void Reset()
    {
        _data = new UserSettingsData();
    }

    /// <inheritdoc cref="UserSettingsData.GetSetting"/>
    public Variant GetSetting(string key) => _data.GetSetting(key);

    /// <inheritdoc cref="UserSettingsData.SetSetting"/>
    public void SetSetting(string key, Variant value)
    {
        _data.SetSetting(key, value);
    }

    /// <inheritdoc cref="UserSettingsData.GetSections"/>
    public string[] GetSections() => _data.GetSections();
    
    /// <inheritdoc cref="UserSettingsData.GetSubSectionsForSection"/>
    public string[] GetSubSectionsForSection(string section) => _data.GetSubSectionsForSection(section);
    
    /// <inheritdoc cref="UserSettingsData.GetAllSections"/>
    public string[] GetAllSections() => _data.GetAllSections();

    /// <inheritdoc cref="UserSettingsData.GetSectionKeys"/>
    public string[] GetSectionKeys(string section) => _data.GetSectionKeys(section);
    
    /// <inheritdoc cref="UserSettingsData.GetAllSectionKeys"/>
    public string[] GetAllSectionKeys() => _data.GetAllSectionKeys();

    /// <inheritdoc cref="UserSettingsData.GetAttributesForSetting"/>
    public UserSettingAttributeData[] GetAttributesForSetting(string key) => _data.GetAttributesForSetting(key);
    
    /// <inheritdoc cref="UserSettingsData.GetAttributesForSection"/>
    public UserSettingAttributeData[] GetAttributesForSection(string section) => _data.GetAttributesForSection(section);
}
