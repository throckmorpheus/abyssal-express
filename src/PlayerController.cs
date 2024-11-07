using Godot;
using System;
using System.Collections.Generic;


public partial class PlayerController : CharacterBody2D
{
	public override void _Ready() {
		base._Ready();
		currentRooms = new List<Room>();

		shouldWalkToTarget = false;

        walkTargetFinal = GetNode<Node2D>("WalkTarget");
		walkTargetFinal.Visible = false;

		walkTargetThisRoom = new Node2D();
		AddChild(walkTargetThisRoom);

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

		if (!hasDoneInitialLockToGround) {
			activeRoom = FindActiveRoom();
			if (activeRoom != null) {
				LockTo(activeRoom, true);
				hasDoneInitialLockToGround = true;
			}
		}
		
		if (Input.IsActionJustPressed("click")) {
			var targetRoom = trainController.MousedOverRoom();
			if (targetRoom == null) { targetRoom = activeRoom; }

			var globalMousePos = GetGlobalMousePosition();
			if (targetRoom != null) {
				walkTargetFinal.GetParent().RemoveChild(walkTargetFinal);
				targetRoom.AddChild(walkTargetFinal);
				walkTargetFinal.GlobalPosition = LockVecToFloorGlobal(globalMousePos, targetRoom);

				walkTargetThisRoom.GlobalPosition = LockVecToFloorGlobal(walkTargetFinal.GlobalPosition, activeRoom);
				shouldWalkToTarget = true;
				walkTargetFinal.Visible = true;
				
				model.Walking = true;
				var toTargetVec = walkTargetThisRoom.Position - Position;
				model.Facing = toTargetVec.X < 0f ? HumanoidAnimator.FacingDirection.LEFT : HumanoidAnimator.FacingDirection.RIGHT;
			}
		}

		float scaledWalkSpeed = WalkSpeed * (float)delta;

		bool walkLeftPressed = Input.IsActionPressed("walk_left");
		bool walkRightPressed = Input.IsActionPressed("walk_right");
		if ((walkLeftPressed && walkRightPressed) || (!shouldWalkToTarget && !walkLeftPressed && !walkRightPressed)) {
			shouldWalkToTarget = false;
			walkTargetFinal.Visible = false;
			model.Walking = false;
		}
		else if (walkLeftPressed || walkRightPressed) {
			shouldWalkToTarget = false;
			walkTargetFinal.Visible = false;
			model.Walking = true;
			model.Facing = walkLeftPressed ? HumanoidAnimator.FacingDirection.LEFT : HumanoidAnimator.FacingDirection.RIGHT;
			var testPosition = footMarker.GlobalPosition + new Vector2(walkLeftPressed ? -1f : 1f, 0f).Rotated(GlobalRotation) * scaledWalkSpeed;
			var lockedPosition = LockVecToFloorGlobal(testPosition, activeRoom);

			var distanceToTargetThisRoom = (activeRoom.ToLocal(lockedPosition) - footMarker.Position.Rotated(Rotation) - Position).Length();
			if (distanceToTargetThisRoom < 1f) {
				float floorCurveOffset = activeRoom.Floor.Curve.GetClosestOffset(activeRoom.Floor.ToLocal(lockedPosition));
				Room ignoredRoom = floorCurveOffset > 0.5f ? trainController.NextRoom(activeRoom) : trainController.PreviousRoom(activeRoom);
				var nextActive = FindActiveRoom(activeRoom, ignoredRoom);
				if (nextActive != null) {
					activeRoom = nextActive;
					LockTo(activeRoom);
					lockedPosition = LockVecToFloorGlobal(testPosition, activeRoom);
				}
			}
			
			var targetLocal = ToLocal(lockedPosition) - footMarker.Position;

			Vector2 motionVec = targetLocal.Normalized() * scaledWalkSpeed;
			Position += motionVec;
		}

		if (shouldWalkToTarget) {
			var toTargetVec = walkTargetThisRoom.Position - footMarker.Position.Rotated(Rotation) - Position;
			var distanceToTarget = toTargetVec.Length();
			
			Vector2 motionVec = toTargetVec.Normalized() * (scaledWalkSpeed > distanceToTarget ? distanceToTarget : scaledWalkSpeed);
			Position += motionVec;
			if (distanceToTarget < 1f) {
				if ((walkTargetFinal.GlobalPosition - footMarker.GlobalPosition).Length() > 1f) {
					var nextActive = FindActiveRoom(activeRoom);
					if (nextActive != null) {
						activeRoom = nextActive;
						LockTo(activeRoom);
						walkTargetThisRoom.GlobalPosition = LockVecToFloorGlobal(walkTargetFinal.GlobalPosition, activeRoom);
					}
				}
				else {
					model.Walking = false;
					walkTargetFinal.Visible = false;
				}
			}
		}
	}

	void LockTo(Room room, bool reposition = false) {
		var oldGlobalTransform = GlobalTransform;
		GetParent().RemoveChild(this); room.AddChild(this);
		GlobalTransform = oldGlobalTransform;
		GlobalRotation = room.GlobalRotation;
		if (reposition) { GlobalPosition = ToGlobal(ToLocal(LockVecToFloorGlobal(footMarker.GlobalPosition, room)) - footMarker.Position); }

		walkTargetThisRoom.GetParent().RemoveChild(walkTargetThisRoom); room.AddChild(walkTargetThisRoom);
		walkTargetThisRoom.GlobalPosition = footMarker.GlobalPosition;
	}

	Vector2 LockVecToFloorGlobal(Vector2 globalPos, Room room) {
		var floorSpacePos = room.Floor.ToLocal(globalPos);
		var floorSpaceTarget = room.Floor.Curve.GetClosestPoint(floorSpacePos);
		var globalTarget = room.Floor.ToGlobal(floorSpaceTarget);
		return globalTarget;
	}

	void OnAreaEntered(Area2D area) {
		const int RoomZoneLayer = 3;
		//const int CameraZoneLayer = 4;

		if (area.GetCollisionLayerValue(RoomZoneLayer)) { currentRooms.Add((Room)area); }
	}

	void OnAreaExited(Area2D area) {
		const int RoomZoneLayer = 3;
		//const int CameraZoneLayer = 4;

		if (area.GetCollisionLayerValue(RoomZoneLayer)) { currentRooms.Remove((Room)area); }
	}

	Room FindActiveRoom(Room skip1 = null, Room skip2 = null) {
		Room current = null;
		currentRooms.ForEach(room => {
			int currentPriority = current == null ? -1 : activeRoom.RoomPriority;
			if (room.RoomPriority > currentPriority && room != skip1 && room != skip2) {
				current = room;
			}
		});
		return current;
	}

	[Export] public float WalkSpeed = 5f;

	[Export] public TrainController trainController;
	
	private Node2D footMarker;
	private HumanoidAnimator model;

	private bool shouldWalkToTarget;
	private Node2D walkTargetFinal;
	private Node2D walkTargetThisRoom;

	private List<Room> currentRooms;
	private Room activeRoom;

	private bool hasDoneInitialLockToGround = false;
}
