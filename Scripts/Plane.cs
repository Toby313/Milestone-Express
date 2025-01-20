using Godot;
using System;

public partial class Plane : Node2D
{
	[Export] private float Speed = 80.0f;
	[Export] private PackedScene CloudScene;
	[Export] private float CloudSpawnInterval = 0.7f;
	[Export] private float MinXPosition = -145f; 
	[Export] private float MaxXPosition = 145.0f; 

	private float timeSinceLastCloud = 0.0f;
	private float targetX;

	public void Initialize(float targetXPosition)
	{
		targetX = targetXPosition;
	}

	public override void _Process(double delta)
	{
		// Move the plane towards the target X position
		Position = new Vector2(Position.X + Speed * (float)delta, Position.Y);

		// Despawn the plane if it reaches the target X position
		if ((Speed > 0 && Position.X >= targetX) || (Speed < 0 && Position.X <= targetX))
		{
			QueueFree();
		}

		// Handle cloud spawning
		timeSinceLastCloud += (float)delta;
		if (timeSinceLastCloud >= CloudSpawnInterval)
		{
			// Check if the plane is within the X spawn range
			if (Position.X >= MinXPosition && Position.X <= MaxXPosition)
			{
				SpawnCloud();
			}
			timeSinceLastCloud = 0.0f;
		}
	}

	private void SpawnCloud()
	{
		// Instantiate the cloud
		var cloud = (Node2D)CloudScene.Instantiate();
		cloud.Position = Position; // Spawn at the plane's position
		GetParent().AddChild(cloud); // Add the cloud to the scene
	}
}
