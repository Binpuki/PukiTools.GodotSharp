namespace PukiTools.Godot;

/// <summary>
/// Used to mark classes that should be an autoload singleton. Generates a static class so accessing it in C# is the same as accessing it in GDScript.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class AutoloadAttribute(string autoLoadName) : Attribute
{
    public string AutoLoadName = autoLoadName;
}