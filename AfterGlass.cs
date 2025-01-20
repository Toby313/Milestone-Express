using Godot;
using System;

public partial class AfterGlass : Node2D
{

	private AnimationPlayer Scene_Transition;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        Scene_Transition = GetNodeOrNull<AnimationPlayer>("Scene_Transition/AnimationPlayer");

		if (Scene_Transition == null)
        {
            GD.PrintErr("Scene_Transition/AnimationPlayer node not found!");
        }

		GD.Print("Playing fade_Out animation.");
        Scene_Transition.Play("Fade_Out");		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
