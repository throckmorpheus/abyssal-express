using Godot;
using System;
using YarnSpinnerGodot;

public partial class DialogueController : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		portraitRect = GetNode<TextureRect>("CharacterPortrait/TextureRect");
		portraitRect.Texture = CharacterPortrait;
		portraitRect.Visible = true;

		dialogueRunner = GetNode<DialogueRunner>("DialogueRunner");
		dialogueRunner.Connect("onNodeStart", new Callable(this, MethodName.OnDialogueStart));
		dialogueRunner.Connect("onDialogueComplete", new Callable(this, MethodName.OnDialogueComplete));

		dialogueRunner.StartDialogue(StartNode);
	}

	public void OnDialogueStart() {
		portraitRect.Visible = true;
	}

	public void OnDialogueComplete() {
		portraitRect.Visible = false;
	}

	[Export] public String StartNode;

	private Texture2D _characterPortrait;
	[Export] public Texture2D CharacterPortrait
	{
		get => _characterPortrait;
		set {
			_characterPortrait = value;
			portraitRect.Texture = _characterPortrait;
		}
	}

	private TextureRect portraitRect;
	private DialogueRunner dialogueRunner;
}
