namespace PukiTools.GodotSharp;

public partial class UserSettingsData
{
    // Put your settings here—there is an example down below.
    public VideoSection Video;

    /// <summary>
    /// Gets a setting by key. More useful in GDScript than it is in C#.
    /// </summary>
    /// <param name="key">The key (case-sensitive)</param>
    /// <returns>The variant value if found, null if not.</returns>
    public partial Variant GetSetting(string key);

    /// <summary>
    /// Sets a setting by key. More useful in GDScript than it is in C#.
    /// </summary>
    /// <param name="key">The key (case-sensitive)</param>
    /// <param name="val">The variant value to set this setting to</param>
    public partial void SetSetting(string key, Variant val);

    /// <summary>
    /// Gets all the main sections present.
    /// </summary>
    /// <returns>All main sections</returns>
    public partial string[] GetSections();

    /// <summary>
    /// Gets all subsections for a section.
    /// </summary>
    /// <param name="section">The section provided (can be a subsection)</param>
    /// <returns>An array of all subsections for a section (can be empty)</returns>
    public partial string[] GetSubSectionsForSection(string section);

    /// <summary>
    /// Gets all the sections present, including subsections.
    /// </summary>
    /// <returns>An array containing every section and subsection.</returns>
    public partial string[] GetAllSections();
    
    /// <summary>
    /// Gets all present keys in the section provided.
    /// </summary>
    /// <param name="section">The section</param>
    /// <returns>An array of keys if section is found, empty array if not.</returns>
    public partial string[] GetSectionKeys(string section);

    /// <summary>
    /// Get every section key present.
    /// </summary>
    /// <returns>An array of all keys.</returns>
    public partial string[] GetAllSectionKeys();
    
    /// <summary>
    /// Gets all attributes for a setting.
    /// </summary>
    /// <param name="key">The setting</param>
    /// <returns>An array of one or multiple attribute data.</returns>
    public partial UserSettingAttributeData[] GetAttributesForSetting(string key);
    
    /// <summary>
    /// Gets all attributes for a section.
    /// </summary>
    /// <param name="section">The section</param>
    /// <returns>An array of one or multiple attribute data</returns>
    public partial UserSettingAttributeData[] GetAttributesForSection(string section);
}

public class VideoSection
{
    [ProjectSetting("display/window/size/mode")] 
    public Window.ModeEnum Fullscreen;
    
    [ProjectSetting("rubicon/general/starting_window_size")] 
    public Vector2I Resolution;
    
    [ProjectSetting("display/window/vsync/vsync_mode")] 
    public DisplayServer.VSyncMode VSync;
    
    [ProjectSetting("application/run/max_fps")] 
    public int MaxFps;

    public Settings3DSection Settings3D;
    public class Settings3DSection
    {
        [ProjectSetting("rendering/scaling_3d/scale")] 
        public Viewport.Scaling3DModeEnum Scaling3DMode;
        
        [ProjectSetting("rendering/scaling_3d/fsr_sharpness")] 
        public float FsrSharpness;
    }
}