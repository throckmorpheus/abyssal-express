using Godot;
using System;

public partial class TrainCar : Node2D
{
	public override void _Ready()
	{
        room = GetNode<Room>("Room");
	}

    private Room room;
    public Room Room {
        get => room;
    }
}
