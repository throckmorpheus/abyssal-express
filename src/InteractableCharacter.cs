using Godot;
using System;

public partial class InteractableCharacter : Interactable
{
	public override void OnInteract() {
        GUISingleton.Instance.DialogueController.CharacterPortrait = Portrait;
        GUISingleton.Instance.DialogueController.DialogueRunner.StartDialogue(DialogueEntryPoint);
    }

    [Export] public Texture2D Portrait;
    [Export] public String DialogueEntryPoint;
}
