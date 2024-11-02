using Godot;
using System;

public partial class HumanoidAnimator : CharacterBody2D
{
    private AnimationPlayer animationPlayer;

    public override void _Ready() {
        base._Ready();
        animationPlayer = (AnimationPlayer)GetNode("AnimationPlayer");
    }

    public override void _Process(double delta) {
        base._Process(delta);
        if (Input.IsActionJustPressed("ui_right")) {
            animationPlayer.Play("walking", 0.3);
        }
        if (Input.IsActionJustReleased("ui_right")) {
            animationPlayer.Play("idle", 0.3);
        }
    }
}
