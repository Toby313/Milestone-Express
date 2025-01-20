using Godot;
using System;

public partial class PlaneSpawner : Node2D
{
	[Export] private PackedScene PlaneScene;
	[Export] private float SpawnInterval = 5.0f;
	[Export] private Vector2 SpawnPosition = new Vector2(-165, 0);
	[Export] private float MinY = -60;
	[Export] private float MaxY = -40f;
	[Export] private float TargetX = 165.0f;
	[Export] private Label InfoLabel;
	[Export] private float StartDelay = 3.0f; 

	private Timer spawnTimer;
	private Timer startTimer;

	public override void _Ready()
	{
		// Initialize the label and make it visible
		InfoLabel.Visible = true;

		// Create and set up the start timer
		startTimer = new Timer();
		startTimer.WaitTime = StartDelay;
		startTimer.OneShot = true;
		startTimer.Connect("timeout", Callable.From(OnStartTimerTimeout));
		AddChild(startTimer);
		startTimer.Start();
	}

	private void OnStartTimerTimeout()
	{
		// Hide the label
		InfoLabel.Visible = false;

		// Create and set up the spawn timer
		spawnTimer = new Timer();
		spawnTimer.WaitTime = SpawnInterval;
		spawnTimer.Connect("timeout", Callable.From(OnSpawnTimerTimeout));
		AddChild(spawnTimer);
		spawnTimer.Start();
	}

	private void OnSpawnTimerTimeout()
	{
		SpawnPlane();
	}

	private void SpawnPlane()
	{
		// Create a new instance of the plane
		var plane = (Node2D)PlaneScene.Instantiate();
		
		// Set a random Y position for the spawn
		float randomY = (float)GD.RandRange(MinY, MaxY);
		plane.Position = new Vector2(SpawnPosition.X, randomY);
		
		// Add the plane to the scene
		AddChild(plane);

		// Move the plane towards the target X position
		var planeScript = plane as Plane;
		planeScript.Initialize(TargetX);
	}
}
