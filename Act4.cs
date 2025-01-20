using Godot;
using System;

public partial class Act4 : Camera2D
{
	float maxSpeed = 180; // The target speed the background reaches
	float accelerationDuration = 1.5f; // Time it takes to reach max speed
	float currentSpeed = 0.0f; // The speed that starts at 0 and increases
	float MoveDuration = 5.0f; // Duration of the movement
	double elapsedTime = 0.0;
	bool isMoving = true;
	bool isWaiting = false;
	Timer transitionTimer;
	private AnimationPlayer Scene_Transition;


	public override void _Ready()
	{
		// Initialize the transition timer
		transitionTimer = new Timer();
		AddChild(transitionTimer);
		transitionTimer.WaitTime = 1.0f; // Timer lasts 1 second
		transitionTimer.OneShot = true;
		transitionTimer.Timeout += OnTransitionTimerTimeout;

		// Locate Scene_Transition and its AnimationPlayer
		Scene_Transition = GetNodeOrNull<AnimationPlayer>("Scene_Transition/AnimationPlayer");

		AudioManager audioManager = (AudioManager)GetNode("/root/AudioManager");
	  
		if (Scene_Transition == null)
		{
			GD.PrintErr("Scene_Transition/AnimationPlayer node not found!");
		}

		audioManager.StartAudioAct4();
		audioManager.PlayStartTrain();
	}

	public void StartSceneTransition()
	{
		if (!transitionTimer.IsStopped())
		{
			GD.Print("Scene transition already in progress.");
			return;
		}

		if (Scene_Transition != null)
		{
			GD.Print("Playing fade-in animation.");
			Scene_Transition.Play("Fade_In1");
		}
		else
		{
			GD.PrintErr("Cannot play animation: Scene_Transition is null.");
		}

		GD.Print("Starting scene transition with 1-second delay...");
		transitionTimer.Start(); // Start the transition timer

		AudioManager audioManager = (AudioManager)GetNode("/root/AudioManager");
		audioManager.StopStartTrain();
	}

	private void OnTransitionTimerTimeout()
	{
		GD.Print("Timer finished. Changing to the next scene...");
		TransitionToNextScene(); // Proceed to the next scene after the timer ends
	}

	private void TransitionToNextScene()
	{
		var nextScenePath = "res://Act4Inside.tscn"; // Path to the next scene
		var nextScene = (PackedScene)ResourceLoader.Load(nextScenePath);
		if (nextScene != null)
		{
			GetTree().ChangeSceneToPacked(nextScene); // Change to the next scene
			GD.Print("Transitioned to the next scene.");
		}
		else
		{
			GD.PrintErr("Failed to load the next scene.");
		}
	}

	public override void _Process(double delta)
	{
		if (isWaiting)
			return; // Stop processing when in waiting state.

		if (isMoving)
		{
			UpdateSpeed(delta);
			MoveBackground(delta);
		}
	}

	private void UpdateSpeed(double delta)
	{
		// Gradually increase speed over the accelerationDuration
		if (elapsedTime < accelerationDuration)
		{
			float accelerationProgress = (float)(elapsedTime / accelerationDuration);
			currentSpeed = maxSpeed * accelerationProgress;
		}
		else
		{
			currentSpeed = maxSpeed; // Cap speed to maxSpeed after accelerationDuration
		}
	}

	private void MoveBackground(double delta)
	{
		// Calculate velocity based on current speed and move the background
		var velocity = new Vector2(currentSpeed, 0);
		Position += velocity * (float)delta;

		// Increment elapsed time
		elapsedTime += delta;

		if (elapsedTime >= MoveDuration)
		{
			StopBackgroundMovement(); // Stop the background and start the transition
		}
	}

	private void StopBackgroundMovement()
	{
		GD.Print("Background movement stopped.");
		StartSceneTransition(); // Trigger the scene transition
	}
}
