using Godot;
using System;

public partial class TestArea : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		    // Test: Manually update the color of a specific area
		var testArea = GetNode<Area2D>("Leaves");
		var colorRect = testArea.GetNode<ColorRect>("ColorRect");
		if (colorRect != null)
		{
			colorRect.Color = new Color(0, 1, 0); // Green
			GD.Print("Manually changed Leaves ColorRect to Green");
		}
	}


}
