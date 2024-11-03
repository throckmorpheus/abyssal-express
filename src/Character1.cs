using Godot;
using System;

public partial class Character1 : Area2D
{
	[Signal]
public delegate void talkEventHandler();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		float mouseposx = GetGlobalMousePosition().X;
		float mouseposy = GetGlobalMousePosition().Y;

		if(Input.IsActionPressed("Move") ){
			if(mouseposx > Position.X -30  && mouseposx < Position.X +30){
				if(mouseposy > Position.Y -30 && mouseposy < Position.Y +30){
					EmitSignal(SignalName.talk, 1);
				}
			}
		}
	}
}
