using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 150.0f;
	public const float JumpVelocity = -200.0f;

	// Define the min and max X position boundaries.
	public const float MinX = -1000f; // Minimum X position
	public const float MaxX = 1000f;  // Maximum X position

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		Vector2 direction = Input.GetVector("walk_left", "walk_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		// Constrain the X position between the defined min and max boundaries.
		Position = new Vector2(Mathf.Clamp(Position.X, MinX, MaxX), Position.Y);

		Velocity = velocity;
		MoveAndSlide();
	}
}
