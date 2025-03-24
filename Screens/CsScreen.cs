namespace PukiTools.GodotSharp.Screens;

/// <summary>
/// A main screen class for C# classes.
/// </summary>
[GlobalClass] public abstract partial class CsScreen : Node
{
    /// <summary>
    /// Resources that will be loaded upon entering a loading screen.
    /// </summary>
    [Export] public ResourceLoadList ResourcesToLoad = new();

    /// <summary>
    /// Check this to make SURE things preload. Useful in the case of hitting "Play Current Scene" on a screen.  
    /// </summary>
    [Export] public bool NeedsPreloading = false;
    
    /// <summary>
    /// Triggers right after the scene is loaded to add resources to load.
    /// </summary>
    public virtual void ReadyPreload() => _preloaded = true;

    /// <summary>
    /// Triggers upon loading a resource specified in <see cref="ResourcesToLoad"/>.
    /// </summary>
    /// <param name="path">The resource loaded.</param>
    public abstract void OnPreload(string path);
    
    private bool _preloaded = false;

    /// <inheritdoc />
    public override void _Ready()
    {
        if (!NeedsPreloading || (NeedsPreloading && _preloaded))
            return;
        
        ScreenManager.SwitchScreen(GetSceneFilePath(), "default");
    }
    
    /// <summary>
    /// Whether this screen was preloaded first.
    /// </summary>
    /// <returns>True or false</returns>
    protected bool IsPreloaded() => _preloaded;
}