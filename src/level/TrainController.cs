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
		animationTime += delta;

		foreach (var trainCar in trainCars) {
			trainCar.Rotation = (float)(angle * Math.Sin(animationTime * speed));
		}
		ArrangeCars();
	}

	void ArrangeCars() {
		for(int i = 0; i < trainCars.Count; i++) {
			TrainCar trainCar = trainCars[i];

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

	public Room MousedOverRoom() {
		Room mousedOverRoom = null;
		foreach (var car in trainCars) {
			if (car.Room.MousedOver && (mousedOverRoom == null || mousedOverRoom.RoomPriority < car.Room.RoomPriority)) {
				mousedOverRoom = car.Room;
			}
		}
		foreach (var gangway in gangways) {
			if (gangway.Room.MousedOver && (mousedOverRoom == null || mousedOverRoom.RoomPriority < gangway.Room.RoomPriority)) {
				mousedOverRoom = gangway.Room;
			}
		}
		return mousedOverRoom;
	}

	public Room NextRoom(Room room) {
		for (int i = 0; i < trainCars.Count; i++) {
			if (trainCars[i].Room == room) {
				return gangways[i].Room;
			}
			if (gangways[i].Room == room) {
				if (i + 1 < trainCars.Count) {
					return trainCars[i + 1].Room;
				}
				//return trainCars[0].Room;
			}
		}
		return null;
	}

	public Room PreviousRoom(Room room) {
		for (int i = 0; i < trainCars.Count; i++) {
			if (trainCars[i].Room == room) {
				if (i - 1 > 0) {
					return gangways[i - 1].Room;
				}
				//return gangways[trainCars.Count - 1].Room;
			}
			if (gangways[i].Room == room) {
				return trainCars[i].Room;
			}
		}
		return null;
	}

	[ExportGroup("Animation")]
	[Export] private double speed = 0.01;
	[Export(PropertyHint.Range, "0, 360, radians_as_degrees")] private double angle = 0.09;

	[ExportGroup("Car Scenes")]
	[Export] public PackedScene[] TrainCarScenes;
	[Export] public PackedScene GangwayScene;

	private List<TrainCar> trainCars;
	private List<Gangway> gangways;

	private double animationTime;
}
