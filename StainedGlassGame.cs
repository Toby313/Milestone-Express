using Godot;
using System;
using System.Collections.Generic;

public partial class StainedGlassGame : Node2D
{
    // Declare member variables for Sprite2D and Button nodes
    private Sprite2D[] _sprites;
    private Button _button1, _button2, _button3, _button4, _button5, _confirmButton;
    private int _currentIndex = 0; // Track which Sprite2D we're currently editing

	private Sprite2D Fix2D;

    // Define the required color combination (Red, Yellow, Blue)
    private Color[] _correctCombination = new Color[]
    {
        new Color(0.75f, 0.75f, 0.75f), // Silver
        new Color(0.76f, 0.6f, 0.42f), // Woody
        new Color(0.6f, 0.3f, 0.1f)  // Brown
    };

    public override void _Ready()
    {
    // Initialize the Sprite2D nodes
    _sprites = new Sprite2D[]
    {
        GetNode<Sprite2D>("Sprite1"),
        GetNode<Sprite2D>("Sprite2"),
        GetNode<Sprite2D>("Sprite3")
    };

    // Set each sprite's Modulate to white (default color)
    foreach (var sprite in _sprites)
    {
        sprite.Modulate = new Color(1, 1, 1); // White as default
    }

	Fix2D = GetNode<Sprite2D>("Fix2D");

    if (Fix2D == null)
    {
        GD.PrintErr("Sprite2D node not found.");
    }

	Fix2D.Visible = true;
    // Initialize the buttons
    _button1 = GetNode<Button>("ButtonRed");
    _button2 = GetNode<Button>("ButtonGreen");
    _button3 = GetNode<Button>("ButtonBlue");
    _button4 = GetNode<Button>("ButtonYellow");
    _button5 = GetNode<Button>("ButtonMagenta");
    _confirmButton = GetNode<Button>("ConfirmButton");

    // Connect color buttons to change colors
    _button1.Pressed += () => ChangeColor(new Color(0.76f, 0.6f, 0.42f)); // 
    _button2.Pressed += () => ChangeColor(new Color(0.75f, 0.75f, 0.75f)); // Sliver
    _button3.Pressed += () => ChangeColor(new Color(0, 0, 1)); // Blue
    _button4.Pressed += () => ChangeColor(new Color(1, 1, 0)); // Yellow
    _button5.Pressed += () => ChangeColor(new Color(0.6f, 0.3f, 0.1f)); // Brown

    // Connect the Confirm button to switch to the next Sprite2D
    _confirmButton.Pressed += OnConfirmButtonPressed;

    // Initialize the first Sprite2D's color to white as well (redundant but ensures clarity)
    _sprites[_currentIndex].Modulate = new Color(1, 1, 1); // White as default
	}
    // Method to change the color of the current Sprite2D
    private void ChangeColor(Color color)
    {
        _sprites[_currentIndex].Modulate = color;
        GD.Print("Color changed to: " + color);
    }

    // Method to handle the confirm button press and switch to the next Sprite2D
    private void OnConfirmButtonPressed()
    {
        GD.Print("Color confirmed for Sprite" + (_currentIndex + 1));

        // Move to the next Sprite2D
        _currentIndex++;

		if (Fix2D != null && _currentIndex == 2)
		{
			Fix2D.Visible = false;  // Hide the sprite
		}
		else
		{
			GD.PrintErr("Sprite2D is null, cannot hide.");
		}

        if (_currentIndex >= _sprites.Length)
        {
            if (CheckColorCombination())
            {
                _confirmButton.Disabled = true;
                GD.Print("You won! All Sprites are correctly colored.");
                var nextScenePath = "res://StainedGlassLevel2.tscn";
                var nextScene = (PackedScene)ResourceLoader.Load(nextScenePath);
                if (nextScene != null)
                {
                    GetTree().ChangeSceneToPacked(nextScene);
                    GD.Print("Transitioned to the next level.");
                }
                else
                {
                    GD.PrintErr("Failed to load the next scene.");
                }
            }
            else
            {
                GD.Print("Incorrect color combination. Resetting colors...");
                foreach (var sprite in _sprites)
                {
                    sprite.Modulate = new Color(1, 1, 1); // Reset to white
					Fix2D.Visible = true;
                }
                _currentIndex = 0;
            }
        }
        else
        {
            _sprites[_currentIndex].Modulate = new Color(1, 1, 1); // Set to white for the next Sprite
        }
    }

    // Method to check if the current color combination matches the correct one
    private bool CheckColorCombination()
    {
        for (int i = 0; i < _sprites.Length; i++)
        {
            if (_sprites[i].Modulate != _correctCombination[i])
            {
                return false; // If any Sprite doesn't match, return false
            }
        }
        return true; // If all Sprites match, return true
    }
}