using Godot;
using System;

public partial class Cameramovement : Camera2D
{
	[Export] public int speed = 40;

	[Export] public float MoveDuration = 20f;
	private double elapsedTime = 0.0; // Time tracker
    private bool isMoving = true;
    private bool isWaiting = false;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		if (isWaiting) 
            return; // Prevent updates during the delay.

        if (!isMoving)
            return; // Stop processing movement if already stopped.

		var velocity = new Vector2();
		velocity.X += 1;
		velocity = velocity * speed;

		Position += velocity * (float)delta;

		  elapsedTime += delta;

        if (elapsedTime >= MoveDuration)
        {
            isMoving = false;
            GD.Print("Background movement complete.");
		}
	}
}
