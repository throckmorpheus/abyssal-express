using Godot;
using System;

public partial class HumanoidAnimator : CanvasGroup
{
    public override void _Ready() {
        base._Ready();
        player = GetNode<AnimationPlayer>("AnimationPlayer");
        tree = GetNode<AnimationTree>("AnimationTree");
    }

    public enum FacingDirection {
        LEFT, RIGHT
    }
    private FacingDirection facing = FacingDirection.RIGHT;
    public FacingDirection Facing {
        get => facing;
        set => SetFacing(value);
    }
    
    void SetFacing(FacingDirection val) {
        facing = val;
        if (facing == FacingDirection.LEFT) {
            Scale = Scale with { X = -1 };
        }
        if (facing == FacingDirection.RIGHT) {
            Scale = Scale with { X = 1 };
        }
    }

    private AnimationPlayer player;
    private AnimationTree tree;

    private bool _walking;
    public bool Walking {
        get => _walking;
        set {
            _walking = value;
            tree.Set("parameters/conditions/is_walking", _walking);
            tree.Set("parameters/conditions/not_walking", !_walking);
        }
    }
}
