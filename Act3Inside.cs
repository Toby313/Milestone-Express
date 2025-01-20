using Godot;
using System;

public partial class Act3Inside : Node2D
{
    Timer transitionTimer;

	private AnimationPlayer Scene_Transition;

    public override void _Ready()
    {
        transitionTimer = new Timer();
        AddChild(transitionTimer);
        transitionTimer.WaitTime = 21.0f; // 14-second timer
        transitionTimer.OneShot = true;
        transitionTimer.Timeout += OnMainTimerTimeout;
        transitionTimer.Start();

        GD.Print("14-second timer started.");

		// Locate Scene_Transition and its AnimationPlayer
        Scene_Transition = GetNodeOrNull<AnimationPlayer>("Scene_Transition/AnimationPlayer");

		if (Scene_Transition == null)
        {
            GD.PrintErr("Scene_Transition/AnimationPlayer node not found!");
        }

		GD.Print("Playing fade_Out animation.");
        Scene_Transition.Play("Fade_Out");

        AudioManager audioManager = (AudioManager)GetNode("/root/AudioManager");
        audioManager.PlayInsideTrain();

    }

    private void OnMainTimerTimeout()
    {
        GD.Print("14 seconds elapsed. Starting 1-second transition timer.");

		
        if (Scene_Transition != null)
        {
            GD.Print("Playing fade_In animation.");
            Scene_Transition.Play("Fade_In1");
        }
        else
        {
            GD.PrintErr("Cannot play animation: Scene_Transition is null.");
        }

        // Set up a 1-second transition timer
        Timer oneSecondTimer = new Timer();
        AddChild(oneSecondTimer);
        oneSecondTimer.WaitTime = 1.0f;
        oneSecondTimer.OneShot = true;
        oneSecondTimer.Timeout += TransitionToNextScene;
        oneSecondTimer.Start();
    }

    private void TransitionToNextScene()
    {
        var nextScenePath = "res://Act3ToStation.tscn"; // Change this to your next scene's path
        var nextScene = (PackedScene)ResourceLoader.Load(nextScenePath);
        if (nextScene != null)
        {
            GetTree().ChangeSceneToPacked(nextScene);
            GD.Print("Transitioned to the next scene.");
        }
        else
        {
            GD.PrintErr("Failed to load the next scene.");
        }

        AudioManager audioManager = (AudioManager)GetNode("/root/AudioManager");
        audioManager.StopInsideTrain();
    }
}

