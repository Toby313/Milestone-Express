using Godot;
using System;

public partial class SportsTimer : Node2D
{
    [Export] public float TimerDuration = 30f; // Duration of the timer in seconds
    [Export] public string NextScenePath = "res://Act2.tscn"; // Path to the next scene

    private Timer _timer;

    public override void _Ready()
    {
        // Create a Timer node
        _timer = new Timer();
        _timer.WaitTime = TimerDuration;
        _timer.OneShot = true; // Ensures the timer runs only once
        _timer.Timeout += OnTimerTimeout; // Connect the Timeout signal
        AddChild(_timer);

        // Start the timer
        GD.Print("Timer started for " + TimerDuration + " seconds.");
        _timer.Start();
    }

    private void OnTimerTimeout()
    {
        GD.Print("Timer finished! Changing scene...");
        GetTree().ChangeSceneToFile(NextScenePath); // Change to the next scene
    }
}
