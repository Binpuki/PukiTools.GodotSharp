using System.Collections.Generic;
using Godot.Collections;
using Array = Godot.Collections.Array;
using ThreadLoadStatus = Godot.ResourceLoader.ThreadLoadStatus;

namespace PukiTools.GodotSharp;

[GlobalClass, Autoload("ResourceQueueLoader")] public partial class ResourceQueueLoaderInstance : Node
{
	[Export] public string CurrentlyLoading;
	
	[Export] public string[] PathQueue = [];
	
	[Signal] public delegate void StartedPreloadingEventHandler(string path);
	
	[Signal] public delegate void PreloadProgressedEventHandler(string path, Array progress);
	
	[Signal] public delegate void ResourceLoadedEventHandler(string pathLoaded);

	private Array _progressArray = [0];
	
	public override void _Ready()
	{
		base._Ready();

		ProcessMode = ProcessModeEnum.Always;
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

		if (PathQueue.Length == 0 && CurrentlyLoading == null)
			return;

		if (CurrentlyLoading == null)
		{
			CurrentlyLoading = PopNext();
			GD.Print($"Loading {CurrentlyLoading}");
			while (!string.IsNullOrEmpty(CurrentlyLoading) && ResourceLoader.HasCached(CurrentlyLoading))
			{
				_progressArray[0] = 1.0;
				EmitSignalStartedPreloading(CurrentlyLoading);
				EmitSignalPreloadProgressed(CurrentlyLoading, _progressArray);
				EmitSignalResourceLoaded(CurrentlyLoading);
				
				CurrentlyLoading = PopNext();
				GD.Print($"Loading {CurrentlyLoading}");
			}
			
			ResourceLoader.LoadThreadedRequest(CurrentlyLoading);
			EmitSignalStartedPreloading(CurrentlyLoading);
			return;
		}

		if (ResourceLoader.HasCached(CurrentlyLoading)) // Failsafe
		{
			EmitSignalResourceLoaded(CurrentlyLoading);
			CurrentlyLoading = null;
            return;
		}

		ThreadLoadStatus status = ResourceLoader.LoadThreadedGetStatus(CurrentlyLoading, _progressArray);
		switch (status)
		{
			case ThreadLoadStatus.Failed:
			case ThreadLoadStatus.InvalidResource:
			{
				GD.PrintErr($"[ResourceQueueLoader] Failed to load resource {CurrentlyLoading}");
				EmitSignalResourceLoaded(CurrentlyLoading);
				CurrentlyLoading = null;
				break;
			}
			case ThreadLoadStatus.InProgress:
			{
				EmitSignalPreloadProgressed(CurrentlyLoading, _progressArray);
				break;
			}
			case ThreadLoadStatus.Loaded:
			{
				EmitSignalResourceLoaded(CurrentlyLoading);
				CurrentlyLoading = null;
				break;
			}
		}
	}

	public void Queue(string resourcePath)
	{
		List<string> queueList = [..PathQueue, resourcePath];
		PathQueue = queueList.ToArray();
	}

	public void QueueRange(string[] resourcePaths)
	{
		List<string> queueList = [..PathQueue];
		queueList.AddRange(resourcePaths);
		PathQueue = queueList.ToArray();
	}

	public void Dequeue(string resourcePath)
	{
		List<string> queueList = [..PathQueue];
		queueList.Remove(resourcePath);
		PathQueue = queueList.ToArray();
	}

	private string PopNext()
	{
		if (PathQueue.Length == 0)
			return "";
		
		List<string> queueList = [..PathQueue];
		string next = queueList[0];
		queueList.RemoveAt(0);
		PathQueue = queueList.ToArray();
		
		return next;
	}
}
