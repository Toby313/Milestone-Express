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
        GetTree().ReloadCurrentScene(); // Restarts the project by reloading the current scene
    }
}
