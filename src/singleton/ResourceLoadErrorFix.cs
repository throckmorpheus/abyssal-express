using Godot;

// I can't pretend to understand why, but in Godot 4.3 resource loading from C# will sometimes fail unless you have this as an autoload
public partial class ResourceLoadErrorFix : Node
{
    public override void _Ready()
    {
        ResourceLoader.Load("res://src/singleton/ResourceLoadErrorFix.cs");
    }
}