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

	public override void _Process(double delta) {
		if (mousedOver && Input.IsActionJustPressed("click")) {
			OnInteract();
		}
	}

	void OnMouseEntered() {
		GD.Print("Mouse over");
		mousedOver = true;
		model.Material = outlineMaterial;
	}

	void OnMouseExited() {
		GD.Print("Mouse exit");
		mousedOver = false;
		model.Material = null;
	}

	public virtual void OnInteract() {}
	
	private Area2D clickArea;
	private Area2D interactArea;

	private CanvasGroup model;
	private Material outlineMaterial;

	private bool mousedOver = false;
}
