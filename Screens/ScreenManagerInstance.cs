using System.Collections.Generic;
using System.Linq;
using PukiTools.GodotSharp;
using Array = Godot.Collections.Array;
using ThreadLoadStatus = Godot.ResourceLoader.ThreadLoadStatus;

namespace PukiTools.GodotSharp.Screens;

/// <summary>
/// Handles switching screens as well as preloading the next screen's assets.
/// </summary>
[GlobalClass, Autoload("ScreenManager")]
public partial class ScreenManagerInstance : CanvasLayer
{
    /// <summary>
    /// The current screen loaded.
    /// </summary>
    [Export] public Node CurrentScreen;
    
    /// <summary>
    /// The current loading screen.
    /// </summary>
    [Export] public Node LoadingScreen;

    /// <summary>
    /// How much progress has been done on loading.
    /// </summary>
    [Export] public int Progress = 0;
    
    /// <summary>
    /// Emits when the progress is updated.
    /// </summary>
    [Signal] public delegate void ProgressUpdatedEventHandler(int progress);
    
    /// <summary>
    /// Emits when loading is complete.
    /// </summary>
    [Signal] public delegate void CompletedEventHandler();
    
    private SceneTree _tree;

    private Array _progressArray = [];
    
    private string _screenPath;
    private bool _startLoading = false;
    private bool _screenLoaded = false;

    private List<string> _preloadList;
    private int _preloadCount = 0;

    /// <summary>
    /// Sets up the node for use.
    /// </summary>
    public override void _Ready()
    {
        _tree = GetTree();
        Layer = 128;
        ProcessMode = ProcessModeEnum.Always;

        ResourceQueueLoader.PreloadProgressed += OnProgressUpdated;
        ResourceQueueLoader.ResourceLoaded += OnResourceLoaded;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        
        if (CurrentScreen == null)
            CurrentScreen = _tree.GetCurrentScene();
    }

    public void AddPath(string path)
    {
        _preloadList = [.._preloadList, path];
        ResourceQueueLoader.Queue(path);
    }
    
    /// <summary>
    /// Switches the current main screen to the screen at the path provided.
    /// </summary>
    /// <param name="path">The path provided.</param>
    /// <param name="loadingScreen">The loading screen to play.</param>
    public void SwitchScreen(string path, string loadingScreen = null)
    {
        if (!ResourceLoader.Exists(path))
        {
            GD.PrintErr($"[SceneManager] Couldn't find screen at path {path}. Aborting.");
            return;
        }
        
        Reset();
        _screenPath = path;

        if (loadingScreen == null)
        {
            StartLoading();
            return;
        }
        
        PackedScene loadingScene = GD.Load<PackedScene>(loadingScreen);
        if (loadingScene is null)
        {
            StartLoading();
            return;
        }

        LoadingScreen = loadingScene.InstantiateOrNull<Node>();
        if (LoadingScreen is null)
        {
            StartLoading();
            return;
        }
        
        AddChild(LoadingScreen);
    }

    /// <summary>
    /// Actually starts loading the queued screen.
    /// </summary>
    public void StartLoading()
    {
        ResourceQueueLoader.Queue(_screenPath);
        _startLoading = true;
        
        CurrentScreen?.QueueFree();
    }

    public string GetCurrentScreenDebugInfo()
    {
        if (CurrentScreen is CsScreen csScreen)
            return csScreen.GetDebugInfo();

        if (CurrentScreen.InheritsFrom(ApiConstants.GdScreen))
            return CurrentScreen.Call(ApiConstants.GdScreenGetDebugInfo).ToString();

        return "";
    }

    private void Reset()
    {
        Progress = 0;
        _screenPath = null;
        _screenLoaded = false;
        _preloadCount = 0;
        _startLoading = false;
        _preloadList = [];
    }

    private void CallReadyPreload()
    {
        if (CurrentScreen is CsScreen cSharpScreen)
        {
            cSharpScreen.ReadyPreload();
            return;
        }

        GDScript screenScript = CurrentScreen.GetScript().As<GDScript>();
        if (screenScript.GetGlobalName() != "GDScreen")
            return;

        CurrentScreen.Call("ready_preload");
    }
    
    private void NotifyResourceLoaded(string path)
    {
        if (CurrentScreen is CsScreen cSharpScreen)
        {
            cSharpScreen.OnPreload(path);
            return;
        }

        GDScript screenScript = CurrentScreen.GetScript().As<GDScript>();
        if (screenScript.GetGlobalName() != "GDScreen")
            return;

        CurrentScreen.Call("on_resource_loaded", path);
    }

    private void UpdateResourcePaths()
    {
        if (CurrentScreen is CsScreen cSharpScreen)
        {
            _preloadList = cSharpScreen.ResourcesToLoad.ToList();
            return;
        }

        GDScript screenScript = CurrentScreen.GetScript().As<GDScript>();
        if (screenScript.GetGlobalName() != "GDScreen")
            return;

        _preloadList = CurrentScreen.Get("resources_to_load").AsStringArray().ToList();
    }

    private void OnProgressUpdated(string path, Array progressArray)
    {
        if (path == _screenPath)
        {
            Progress = Mathf.FloorToInt(progressArray[0].AsDouble() * 50);
            return;
        }
        
        float segment = 1f / _preloadList.Count;
        Progress = 50 + (Mathf.FloorToInt(((float)_preloadCount / _preloadList.Count) +
                                     (segment * progressArray[0].AsDouble())) * 50);
    }

    private void OnResourceLoaded(string path)
    {
        if (path == _screenPath)
        {
            Progress = 50;
            CurrentScreen = ResourceLoader.Load<PackedScene>(path).Instantiate();
            UpdateResourcePaths();
            CallReadyPreload();
            return;
        }
        
        if (!_preloadList.Contains(path))
            return;
        
        _preloadCount++;
        Progress = 50 + Mathf.FloorToInt((float)_preloadCount / _preloadList.Count) * 50;
        EmitSignalProgressUpdated(Progress);
        NotifyResourceLoaded(path);
        
        if (_preloadCount < _preloadList.Count)
            return;

        Progress = 100;
        
        _tree.Root.AddChild(CurrentScreen);
        _tree.CurrentScene = CurrentScreen;
        
        EmitSignalProgressUpdated(Progress);
        EmitSignalCompleted();
        
        Reset();
    }
}