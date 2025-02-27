using Array = Godot.Collections.Array;

namespace PukiTools.Godot;

[GlobalClass] public partial class UserSettingAttributeData : RefCounted
{
    [Export] public string Name;

    [Export] public Array Parameters = [];

    public UserSettingAttributeData()
    {
        
    }
    
    public UserSettingAttributeData(string name, params Variant[] parameters)
    {
        Name = name;
        for (int i = 0; i < parameters.Length; i++)
            Parameters.Add(parameters[i]);
    }
}