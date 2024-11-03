using Godot;
using System;


public partial class Player : Area2D
{
[Signal]
    public delegate void talkEventHandler(int entity);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
		var char1 = GetNode<Character1>("Character1");
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		float mouseposx = GetGlobalMousePosition().X;
		
		var velocity = Vector2.Zero;
		//if talking dont move
		if(talking ==false){

		if (Input.IsActionPressed("Move")){
			Moveto = mouseposx;	
			startpos = Position.X;
			if(Moveto > Position.X){
				dir =1;
			}else{
				dir =-1;
			}
		}
		
		talk += talktime;

		
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

		if(Position.X > ScreenSize.X -50){
			//next train
		}

		if(Position.X < 50){
			//previous train
		}

	}
	private void talktime(int entity){
		talking =true;
	}




	[Export]
	public int Speed { get; set; } = 200; // How fast the player will move (pixels/sec).
	private bool talking = false;
	private bool inrange = false;
	private float Moveto;
	private float startpos;
	private int dir=0;
	public Vector2 ScreenSize; // Size of the game window.

}
