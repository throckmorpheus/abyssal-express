using Godot;
using System;
using System.Collections.Generic;

public partial class TrainController : Node2D
{
	public override void _Ready()
	{
		trainCars = new List<TrainCar>();
		gangways = new List<Gangway>();

		foreach (var scene in TrainCarScenes) {
			var trainCar = scene.Instantiate<TrainCar>();
			AddChild(trainCar);
			trainCars.Add(trainCar);
		}

		for (int i = 0; i < trainCars.Count; i++) {
			var gangway = GangwayScene.Instantiate<Gangway>();
			AddChild(gangway);
			gangways.Add(gangway);
		}

		ArrangeCars();
	}

	public override void _Process(double delta)
	{
	}

	void ArrangeCars() {
		for(int i = 0; i < trainCars.Count; i++) {
			TrainCar trainCar = trainCars[i];

			trainCar.Rotate(MathF.PI / 180f * 15f);

			Gangway gangwayPrev = i > 0 ? gangways[i - 1] : null;
			Gangway gangwayNext = gangways[i];

			if (gangwayPrev != null) {
				trainCar.GlobalPosition = gangwayPrev.Room.BottomRight.GlobalPosition - trainCar.ToLocal(trainCar.Room.BottomLeft.GlobalPosition).Rotated(trainCar.Rotation);
				gangwayPrev.Room.TopRight.GlobalPosition = trainCar.Room.TopLeft.GlobalPosition;
			}

			gangwayNext.GlobalPosition = trainCar.Room.BottomRight.GlobalPosition - gangwayNext.ToLocal(gangwayNext.Room.BottomLeft.GlobalPosition);
			gangwayNext.Room.TopLeft.GlobalPosition = trainCar.Room.TopRight.GlobalPosition;
		}
	}

	[Export] public PackedScene[] TrainCarScenes;
	[Export] public PackedScene GangwayScene;

	private List<TrainCar> trainCars;
	private List<Gangway> gangways;

}
