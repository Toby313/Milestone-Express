using Godot;
using System;

public partial class Cameramovement : Camera2D
{
	[Export] public int speed = 40;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var velocity = new Vector2();
		velocity.X += 1;
		velocity = velocity * speed;

		Position += velocity * (float)delta;
	}
}
