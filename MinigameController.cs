using Godot;
using System;

public partial class MinigameController : Node2D
{
	private float beatInterval = 0.67f; // Time between beats in seconds (optional for sound/visual sync)
	private float timeSinceLastBeat = 0f; // Tracks time since the last beat
	private float hitCooldown = 0.5f;   // Cooldown between player inputs
	private float timeSinceLastHit = 0f; // Tracks time since the last hit

	private int score = 0;            // Player's total score
	private Label FeedbackLabel;      // Feedback for "Perfect", "Good", etc.
	private AudioStreamPlayer AnvilSound; // Plays the anvil sound
	private float gameTimeRemaining = 10.0f; // Game duration in seconds
	private bool isGameOver = false; // Tracks if the game has ended

	public override void _Ready()
	{
		// Get UI elements
		FeedbackLabel = GetNode<Label>("FeedbackLabel");
		AnvilSound = GetNode<AudioStreamPlayer>("AnvilSound");
	}

	public override void _Process(double delta)
	{
		if (isGameOver) return;
	
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
			FeedbackLabel.Text = "Good!";
			score += 50;
		}
		else // Miss
		{
			FeedbackLabel.Text = "Miss!";
		}
	}

	private void EndGame()
	{
		isGameOver = true; // Stop gameplay logic
		FeedbackLabel.Text = $"Game Over! Final Score: {score}";
	}
}
