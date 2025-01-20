using Godot;
using System;

public partial class PlayerMove : CharacterBody2D
{
    [Export] public float Speed = 200f; // Movement speed of the player

    public override void _PhysicsProcess(double delta)
    {
        // Get player input
        Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        if (direction != Vector2.Zero)
        {
            direction = direction.Normalized();
            Velocity = direction * Speed;
        }
        else
        {
            Velocity = Vector2.Zero; // Stop when no input is detected
        }

        // Move the player
        MoveAndSlide();
    }

    private void _on_area_2d_body_entered(Node2D body)
    {
        // Check if the body is a ball
        if (body is RigidBody2D ball)
        {
            // Calculate the push direction
            Vector2 pushDirection = (ball.GlobalPosition - GlobalPosition).Normalized();

            // Apply an impulse to the ball
            ball.ApplyImpulse(Vector2.Zero, pushDirection * 300f); // Adjust the impulse strength as needed
        }
    }
}
