using Godot;
using System;

public partial class Cloud : Node2D
{
	[Export] private float Speed = 60.0f;
	private Label gameOverLabel;
	private Area2D cloudArea;

	public override void _Ready()
	{
		cloudArea = GetNode<Area2D>("Area2D");
		cloudArea.BodyEntered += OnBodyEntered;

		cloudArea.CollisionLayer = 2;
		cloudArea.CollisionMask = 3;

		// Find the Label node by group
		var uiNodes = GetTree().GetNodesInGroup("UI");
		foreach (var node in uiNodes)
		{
			if (node is Label label)
			{
				gameOverLabel = label;
				break; // Assuming you only need one label
			}
		}

		// Ensure the label is initially hidden
		if (gameOverLabel != null)
		{
			gameOverLabel.Visible = false;
		}
	}

	public override void _Process(double delta)
	{
		Position += new Vector2(0, Speed * (float)delta);
	}

	private void OnBodyEntered(Node body)
	{
		if (body.IsInGroup("Ground"))
		{
			GD.Print("Cloud collided with ground. Restarting game...");
			RestartGameWithDelay(3.0f); // 3-second delay before restarting
		}
		else
		{
			GD.Print("Cloud collided with non-ground object. Destroying...");
			QueueFree();
		}
	}

	private async void RestartGameWithDelay(float delay)
	{
		if (gameOverLabel != null)
		{
			gameOverLabel.Visible = true; // Show the label
		}

		// Wait for the delay
		await ToSignal(GetTree().CreateTimer(delay), "timeout");

		if (gameOverLabel != null)
		{
			gameOverLabel.Visible = false; // Hide the label after the delay
		}

		// Reload the current scene
		GetTree().ReloadCurrentScene();
	}
}
