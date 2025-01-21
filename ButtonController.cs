using Godot;
using System;

public partial class ButtonController : Node2D
{
    public override void _Ready()
    {
        // Connect button signals
        Button quitButton = GetNode<Button>("Stop");
        Button restartButton = GetNode<Button>("Restart");

        quitButton.Pressed += OnQuitButtonPressed;
        restartButton.Pressed += OnRestartButtonPressed;
    }

    private void OnQuitButtonPressed()
    {
        GD.Print("Quitting the game...");
        GetTree().Quit(); // Ends the project
    }

    private void OnRestartButtonPressed()
    {
        GD.Print("Restarting the game...");
        var nextScenePath = "res://mainscene.tscn"; // Path to the next scene
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
