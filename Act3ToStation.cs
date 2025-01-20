using Godot;
using System;

public partial class Act3ToStation : Camera2D
{
    int initialSpeed = 180; // The starting speed of the train
    int currentSpeed = 180; // The speed that will gradually decrease
    float MoveDuration = 3.0f; // Duration before slowing down starts
    float DecelerationDuration = 2.0f; // Time over which the train slows down
    double elapsedTime = 0.0;
    double decelerationStartTime = 0.0; // Time when deceleration starts
    bool isMoving = true;
    bool isSlowingDown = false; // Whether the train is in the deceleration phase
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

		if (Scene_Transition == null)
        {
            GD.PrintErr("Scene_Transition/AnimationPlayer node not found!");
        }

		GD.Print("Playing fade_Out animation.");
        Scene_Transition.Play("Fade_Out");

    }

    public override void _Process(double delta)
    {
        if (isWaiting)
            return; // Stop processing when in waiting state.

        if (isMoving)
        {
            MoveBackground(delta);
        }
        else if (isSlowingDown)
        {
            DecelerateBackground(delta);
        }
    }

    private void MoveBackground(double delta)
    {
        // Calculate velocity and move the background
        var velocity = new Vector2(currentSpeed, 0);
        Position += velocity * (float)delta;

        // Increment elapsed time
        elapsedTime += delta;

        if (elapsedTime >= MoveDuration)
        {
            StartDeceleration(); // Trigger the deceleration phase
        }
    }

    private void StartDeceleration()
    {
        GD.Print("Background movement slowing down.");
        isMoving = false;
        isSlowingDown = true;
        decelerationStartTime = elapsedTime; // Mark the start of the deceleration
    }

    private void DecelerateBackground(double delta)
    {
        if (elapsedTime - decelerationStartTime >= DecelerationDuration)
        {
            StopBackgroundMovement(); // Fully stop after deceleration
            return;
        }

        // Gradually reduce the speed over DecelerationDuration
        float decelerationProgress = (float)((elapsedTime - decelerationStartTime) / DecelerationDuration);
        currentSpeed = (int)(initialSpeed * (1 - decelerationProgress)); // Linearly reduce speed

        var velocity = new Vector2(currentSpeed, 0);
        Position += velocity * (float)delta;
        elapsedTime += delta;

        GD.Print($"Decelerating: currentSpeed = {currentSpeed}");
    }

    private void StopBackgroundMovement()
    {
        GD.Print("Background movement stopped.");
        isSlowingDown = false;
        isWaiting = true;

        StartSceneTransition(); // Trigger the scene transition
    }

    public void StartSceneTransition()
    {
        if (!transitionTimer.IsStopped())
        {
            GD.Print("Scene transition already in progress.");
            return;
        }

        GD.Print("Starting scene transition with 1-second delay...");
        transitionTimer.Start(); // Start the transition timer

        if (Scene_Transition != null)
        {
            Scene_Transition.Play("Fade_In1");
        }
    }

    private void OnTransitionTimerTimeout()
    {
        TransitionToNextScene(); // Proceed to the next scene after the timer ends
    }

    private void TransitionToNextScene()
    {
        var nextScenePath = "res://ExploringMiniGame.tscn"; // Path to the next scene
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
}
