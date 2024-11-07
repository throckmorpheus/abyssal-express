using Godot;

public partial class GUISingleton : Node
{
    public static GUISingleton Instance { get; private set; }

    public override void _Ready() {
        Instance = this;
    }

    public DialogueController DialogueController;
}
