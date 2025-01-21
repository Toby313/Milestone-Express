using Godot;
using System;

public partial class PlaneSpawner : Node2D
{
	[Export] private PackedScene PlaneScene;
	[Export] private float SpawnInterval = 5.0f;
	[Export] private Vector2 SpawnPosition = new Vector2(-165, 0);
	private float MinY = -55f;
	private float MaxY = -40f;
	[Export] private float TargetX = 165.0f;
	[Export] private Label InfoLabel; 
	[Export] private float StartDelay = 5.5f; 
	[Export] private Label EndLabel;
	[Export] private float gameDuration = 30.0f;
	[Export] private Label gameOverLabel;

	 [Export] public string NextScenePath = "res://Act3.tscn";


	private Timer spawnTimer;
	private Timer startTimer;
	private Timer gameTimer;

	public override void _Ready()
	{
		// Initialize the label and make it visible
		InfoLabel.Visible = true;
		EndLabel.Visible = false;
		gameOverLabel.Visible = false;

		// Create and set up the start timer
		startTimer = new Timer();
		startTimer.WaitTime = StartDelay;
		startTimer.OneShot = true;
		startTimer.Connect("timeout", Callable.From(OnStartTimerTimeout));
		AddChild(startTimer);
		startTimer.Start();

		gameTimer = new Timer();
		gameTimer.WaitTime = gameDuration;
		gameTimer.OneShot = true;
		gameTimer.Connect("timeout", Callable.From(OnGameTimerTimeout));
		AddChild(gameTimer);
		gameTimer.Start();

		AudioManager audioManager = (AudioManager)GetNode("/root/AudioManager");
		audioManager.OnAudioFinished();
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
	
	private void OnGameTimerTimeout()
	{
		StopSpawning();
		EndLabel.Visible = true;
		RestartGameWithDelay(5.0f); 
	}
	
	private async void RestartGameWithDelay(float delay)
	{
		GD.Print("Restarting game with delay...");
		await ToSignal(GetTree().CreateTimer(delay), "timeout");
   	 	GetTree().ChangeSceneToFile(NextScenePath); // Change to the next scene
	}
	

	private void SpawnPlane()
	{
		var plane = (Node2D)PlaneScene.Instantiate();
		
		float randomY = (float)GD.RandRange(MinY, MaxY);
		plane.Position = new Vector2(SpawnPosition.X, randomY);
		
		AddChild(plane);

		var planeScript = plane as Plane;
		planeScript.Initialize(TargetX);
	}
	
	private void StopSpawning()
	{
		if (spawnTimer != null)
		{
			spawnTimer.Stop();
		}
	}
}
