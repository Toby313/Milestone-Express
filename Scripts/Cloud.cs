using Godot;
using System;

public partial class Cloud : Node2D
{
	[Export] private float Speed = 50.0f;
	private Area2D cloudArea;
	private string playerTag = "Player"; // Use a tag to identify the player

	public override void _Ready()
	{
		// Get the Area2D node (make sure you have a CollisionShape2D inside the cloud)
		cloudArea = GetNode<Area2D>("Area2D");

		// Connect the body_entered signal to handle collision
		cloudArea.BodyEntered += OnBodyEntered;
	}

	public override void _Process(double delta)
	{
		// Move the cloud downwards
		Position += new Vector2(0, Speed * (float)delta);
	}

	// Collision detection using body_entered signal
	private void OnBodyEntered(Node body)
	{
		// If the cloud collides with the player, destroy the cloud
		if (body is Node player && player.HasMeta("tag") && player.GetMeta("tag").ToString() == playerTag)
		{
			// Destroy the cloud when it collides with the player
			QueueFree();
		}
	}
}
