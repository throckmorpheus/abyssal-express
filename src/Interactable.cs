using Godot;
using System;

public partial class Interactable : Node2D
{
	public override void _Ready()
	{
		outlineMaterial = GD.Load<Material>("res://assets/materials/outline.material");

		clickArea = GetNode<Area2D>("ClickArea");
		interactArea = GetNode<Area2D>("InteractArea");

		model = GetNode<CanvasGroup>("Model");

		clickArea.MouseEntered += OnMouseEntered;
		clickArea.MouseExited += OnMouseExited;
	}

	void OnMouseEntered() {
		model.Material = outlineMaterial;
	}

	void OnMouseExited() {
		model.Material = null;
	}
	
	private Area2D clickArea;
	private Area2D interactArea;

	private CanvasGroup model;
	private Material outlineMaterial;
}
