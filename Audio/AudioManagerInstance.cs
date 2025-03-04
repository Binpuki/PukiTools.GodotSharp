using Godot.Collections;

namespace PukiTools.GodotSharp.Audio;

[GlobalClass, Autoload("AudioManager")]
public partial class AudioManagerInstance : Node
{
    private Dictionary<string, AudioPlayerGroup> _groups = new();

    public override void _Ready()
    {
        base._Ready();

        int busCount = AudioServer.BusCount;
        for (int i = 0; i < busCount; i++)
        {
            string bus = AudioServer.GetBusName(i);

            AudioPlayerGroup group = new AudioPlayerGroup(bus);
            _groups[bus] = group;
            
            AddChild(group);
        }
    }
    
    public AudioPlayerGroup GetGroup(string bus) => _groups[bus];
}