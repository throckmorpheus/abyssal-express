using Godot;
using System;

public partial class Player : Area2D
{

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		float mouseposx = GetGlobalMousePosition().X;
		
		var velocity = Vector2.Zero;

		if (Input.IsActionPressed("Move")){
			Moveto = mouseposx;	
			startpos = Position.X;
			if(Moveto > Position.X){
				dir =1;
			}else{
				dir =-1;
			}
		}
		if (Input.IsActionPressed("Interact")){
			
		}
		if(Input.IsActionPressed("Move_right")){
			Moveto = Position.X +100;
			

		}

		var animatedsprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		if(dir > 0){
		if (Position.X < Moveto){
			//move
			velocity.X =  Speed * dir;
			Position += velocity * (float)delta;	
			animatedsprite2D.Animation = "walk";	
			animatedsprite2D.Play();	
		}
				else{dir =0;}
		}
		else{
		if(dir <0){
		if (Position.X > Moveto){
			//move
			velocity.X =  Speed * dir;
			Position += velocity * (float)delta;
			animatedsprite2D.Animation = "walk";	
			animatedsprite2D.Play();			
		}
				else{dir =0;}
		}
		else{
			animatedsprite2D.Animation = "wait";	
			animatedsprite2D.Play();
		}}
			Position = new Vector2(
    			x: Mathf.Clamp(Position.X, 30, ScreenSize.X -30),
    			y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
			);
	}

	[Export]
    public int Speed { get; set; } = 200; // How fast the player will move (pixels/sec).
	//public float PlayerPos = 0;
	private float Moveto;
	private float startpos;
	private int dir=0;
    public Vector2 ScreenSize; // Size of the game window.

}
