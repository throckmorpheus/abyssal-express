using Godot;
using System;
using System.Collections.Generic;


public partial class PlayerController : CharacterBody2D
{
	public override void _Ready() {
		base._Ready();
		currentRooms = new List<Room>();

		shouldWalkToTarget = false;

        walkTarget = GetNode<Node2D>("WalkTarget");
		walkTarget.Visible = false;

		footMarker = GetNode<Node2D>("FootMarker");
		model = GetNode<HumanoidAnimator>("PlayerModel");

		var hitbox = GetNode<Area2D>("Hitbox");
		hitbox.AreaEntered += OnAreaEntered;
		hitbox.AreaExited += OnAreaExited;

		var areas = hitbox.GetOverlappingAreas();
		foreach (var area in areas) {
			OnAreaEntered(area);
		}
	}

    public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);
		
		if (Input.IsActionJustPressed("click")) {
			var globalMousePos = GetGlobalMousePosition();
			if (activeRoom != null) {
				var floorSpaceMousePos = activeRoom.Floor.ToLocal(globalMousePos);
				var floorSpaceTarget = activeRoom.Floor.Curve.GetClosestPoint(floorSpaceMousePos);
				walkTarget.Position = activeRoom.Floor.ToGlobal(floorSpaceTarget);
				shouldWalkToTarget = true;
				walkTarget.Visible = true;
				
				model.Walking = true;
				var toTargetVec = walkTarget.Position - Position;
				if (toTargetVec.X < 0f) {
					model.Facing = HumanoidAnimator.FacingDirection.LEFT;
				}
				else {
					model.Facing = HumanoidAnimator.FacingDirection.RIGHT;
				}
			}
		}

		bool walkLeftPressed = Input.IsActionPressed("walk_left");
		bool walkRightPressed = Input.IsActionPressed("walk_right");
		if ((walkLeftPressed && walkRightPressed) || (!shouldWalkToTarget && !walkLeftPressed && !walkRightPressed)) {
			shouldWalkToTarget = false;
			walkTarget.Visible = false;
			model.Walking = false;
		}
		else if (walkLeftPressed) {
			shouldWalkToTarget = false;
			walkTarget.Visible = false;
			model.Walking = true;
			model.Facing = HumanoidAnimator.FacingDirection.LEFT;
			var moveVecLocal = new Vector2(-WalkSpeed, 0);
			MoveAndCollide(moveVecLocal.Rotated(activeRoom.Transform.Rotation));
		}
		else if (walkRightPressed) {
			shouldWalkToTarget = false;
			walkTarget.Visible = false;
			model.Walking = true;
			model.Facing = HumanoidAnimator.FacingDirection.RIGHT;
			var moveVecLocal = new Vector2(WalkSpeed, 0);
			MoveAndCollide(moveVecLocal.Rotated(activeRoom.Transform.Rotation));
		}

		if (shouldWalkToTarget) {
			var toTargetVec = walkTarget.Position - footMarker.Position - Position;
			var distanceToTarget = toTargetVec.Length();
			MoveAndCollide(toTargetVec.Normalized() * (WalkSpeed > distanceToTarget ? distanceToTarget : WalkSpeed));
			if (distanceToTarget < 1f) {
				model.Walking = false;
			}
		}
	}

	void OnAreaEntered(Area2D area) {
		const int RoomZoneLayer = 3;
		//const int CameraZoneLayer = 4;

		if (area.GetCollisionLayerValue(RoomZoneLayer)) {
			currentRooms.Add((Room)area);
			RecalculateActiveRoom();
		}
	}

	void OnAreaExited(Area2D area) {
		const int RoomZoneLayer = 3;
		//const int CameraZoneLayer = 4;

		if (area.GetCollisionLayerValue(RoomZoneLayer)) {
			currentRooms.Remove((Room)area);
			RecalculateActiveRoom();
		}
	}

	void RecalculateActiveRoom() {
		Room current = null;
		currentRooms.ForEach(room => {
			int currentPriority = current == null ? -1 : activeRoom.RoomPriority;
			if (room.RoomPriority > currentPriority) {
				current = room;
			}
		});
		activeRoom = current;
	}

	[Export] public float WalkSpeed = 5f;
	
	private Node2D footMarker;
	private HumanoidAnimator model;

	private bool shouldWalkToTarget;
	private Node2D walkTarget;

	private List<Room> currentRooms;
	private Room activeRoom;
}
