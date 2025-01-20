using Godot;
using System;

public partial class MinigameController : Node2D
{
	private float beatInterval = 0.67f; // Time between beats in seconds
	private float timeSinceLastBeat = 0f; // Tracks time since the last beat
	private float hitCooldown = 0.5f;   // Cooldown between player inputs
	private float timeSinceLastHit = 0f; // Tracks time since the last hit

	private int score = 0;              // Player's total score
	private Label FeedbackLabel;        // Feedback for "Perfect", "Good", etc.
	private AudioStreamPlayer AnvilSound; // Plays the anvil sound
	private Button RestartButton;       // Restart game button
	private Button ContinueButton;      // Continue button (only for high scores)
	private float gameTimeRemaining = 10.0f; // Game duration in seconds
	private bool isGameOver = false;    // Tracks if the game has ended
	private bool isGameStarted = false; // Tracks if the game has started

	public override void _Ready()
	{
		// Get UI elements
		FeedbackLabel = GetNode<Label>("FeedbackLabel");
		AnvilSound = GetNode<AudioStreamPlayer>("AnvilSound");
		RestartButton = GetNode<Button>("RestartButton");
		ContinueButton = GetNode<Button>("ContinueButton");

		// Hide buttons at the start
		RestartButton.Visible = false;
		ContinueButton.Visible = false;

		// Display a message to start the game
		FeedbackLabel.Text = "Klik op\nde maat!";

		// Connect buttons
		RestartButton.Connect("pressed", new Callable(this, "OnRestartPressed"));
		ContinueButton.Connect("pressed", new Callable(this, "OnContinuePressed"));
	}

	public override void _Process(double delta)
	{
		if (isGameOver) return;

		if (!isGameStarted)
		{
			// Start the game when the player presses the "hammer_strike" button
			if (Input.IsActionJustPressed("hammer_strike"))
			{
				isGameStarted = true;
				FeedbackLabel.Text = ""; // Clear the start message
			}
			return; // Do not run game logic until the game has started
		}

		// Reduce the remaining game time
		gameTimeRemaining -= (float)delta;

		// Check if the time is up
		if (gameTimeRemaining <= 0)
		{
			EndGame();
		}

		// Update the timers
		timeSinceLastBeat += (float)delta;
		timeSinceLastHit += (float)delta;

		// Play beat sound or logic
		if (timeSinceLastBeat >= beatInterval)
		{
			timeSinceLastBeat = 0f;
			FeedbackLabel.Text = ""; // Clear feedback
			AnvilSound.Play(); // Play the cue sound
		}

		// Check player input with cooldown
		if (Input.IsActionJustPressed("hammer_strike") && timeSinceLastHit >= hitCooldown)
		{
			CheckTiming();
			timeSinceLastHit = 0f; // Reset cooldown timer
		}
	}

	private void CheckTiming()
	{
		float timingError = Math.Abs(timeSinceLastBeat - beatInterval / 2); // How far off they are

		if (timingError <= 0.05f) // Perfect hit
		{
			FeedbackLabel.Text = "Perfect!";
			score += 100;
		}
		else if (timingError <= 0.1f) // Good hit
		{
			FeedbackLabel.Text = "Goed!";
			score += 50;
		}
		else // Miss
		{
			FeedbackLabel.Text = "Mis!";
		}
	}

	private void EndGame()
	{
		isGameOver = true; // Stop gameplay logic
		FeedbackLabel.Text = $"Score:\n{score}";

		// Check if the score is 1000 or more
		if (score >= 1000)
		{
			ContinueButton.Visible = true;
		}

		RestartButton.Visible = true;
	}

	private void OnRestartPressed()
	{
		// Reset game state
		isGameOver = false;
		isGameStarted = false;
		score = 0;
		gameTimeRemaining = 10.0f;
		timeSinceLastBeat = 0f;
		timeSinceLastHit = 0f;

		// Hide buttons and reset UI
		RestartButton.Visible = false;
		ContinueButton.Visible = false;
		FeedbackLabel.Text = "Klik op\nde maat!";
	}

	private void OnContinuePressed()
	{
		TransitionToNextScene();
	}
	private void TransitionToNextScene()
    {
        var nextScenePath = "res://Act4.tscn"; // Path to the next scene
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
