using Godot;
using System;

public partial class Room : Area2D
{
	public override void _Ready()
	{
		floor = GetNode<Path2D>("Floor");
	}
	
	[Export] public int RoomPriority = 1;

	private Path2D floor;
	public Path2D Floor {
		get => floor;
	}

	[Export] public Node2D TopLeft, TopRight, BottomLeft, BottomRight;
}
