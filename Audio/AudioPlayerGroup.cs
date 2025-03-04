using System.Collections.Generic;
using System.Linq;

namespace PukiTools.GodotSharp.Audio;

[GlobalClass] public partial class AudioPlayerGroup : Node
{
    [Export] public string Bus;
    
    [Export] public AudioStreamPlayer[] Players = [];

    private Dictionary<AudioStreamPlayer, bool> _shouldAutoDestroy = new();

    public AudioPlayerGroup(string bus)
    {
        Bus = bus;
    }

    public AudioStreamPlayer Play(AudioStream stream, bool autoDestroy = true, float time = 0f)
    {
        AudioStreamPlayer player = GetPlayer(stream);
        if (player != null)
        {
            MarkAutoDestroy(player, autoDestroy);
            player.Play(time);
            return player;
        }

        player = new AudioStreamPlayer();
        player.Stream = stream;
        player.Bus = Bus;

        player.Finished += () => HandleAutoDestroy(player);
        MarkAutoDestroy(player, autoDestroy);
        
        AddChild(player);
        player.Play(time);

        return player;
    }

    public void SetPaused(bool paused)
    {
        for (int i = 0; i < Players.Length; i++)
            Players[i].StreamPaused = paused;
    }

    public void Stop()
    {
        for (int i = 0; i < Players.Length; i++)
            Players[i].Stop();
    }

    public AudioStreamPlayer GetPlayer(AudioStream stream)
    {
        return Players.FirstOrDefault(p => p.Stream == stream);
    }

    public void MarkAutoDestroy(AudioStreamPlayer player, bool autoDestroy = true)
    {
        _shouldAutoDestroy[player] = autoDestroy;
    }

    public void ChangeVolumeDb(float volumeDb)
    {
        for (int i = 0; i < Players.Length; i++)
            Players[i].VolumeDb = volumeDb;
    }

    public void ChangeVolumeLinear(float volumeLinear)
    {
        for (int i = 0; i < Players.Length; i++)
            Players[i].VolumeLinear = volumeLinear;
    }

    private void HandleAutoDestroy(AudioStreamPlayer player)
    {
        if (_shouldAutoDestroy.ContainsKey(player) && !_shouldAutoDestroy[player])
            return;
        
        RemoveChild(player);
        player.QueueFree();
    }
}