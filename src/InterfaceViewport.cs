using Godot;
using System;

public partial class InterfaceViewport : CanvasLayer
{
	public override void _Ready()
	{
		GUISingleton.Instance.DialogueController = GetNode<DialogueController>("MarginContainer/Dialogue");
	}
}
