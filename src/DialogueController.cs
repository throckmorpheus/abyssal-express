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

		DialogueRunner = GetNode<DialogueRunner>("DialogueRunner");
		DialogueRunner.Connect("onNodeStart", new Callable(this, MethodName.OnDialogueStart));
		DialogueRunner.Connect("onDialogueComplete", new Callable(this, MethodName.OnDialogueComplete));

		//dialogueRunner.StartDialogue(StartNode);
	}

	public void OnDialogueStart() {
		portraitRect.Visible = true;
	}

	public void OnDialogueComplete() {
		portraitRect.Visible = false;
	}

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
	public DialogueRunner DialogueRunner { get; private set; }
}
