using Godot;
using System;

public partial class Slime : CharacterBody2D
{
	[Export]
	public float Speed = 20.0f;
	[Export]
	public Vector2 Direction = new Vector2(1.0f, 0.0f);
	private AnimatedSprite2D _animatedSprite;

    public override void _Ready()
    {	
		// Initialise AnimatedSprite2D Node
        _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		// Start ChangeDirection Timer
		GetNode<Timer>("ChangeDirection").Start();
    }

    public override void _Process(double delta)
    {
		Vector2 velocity = Velocity;

		// Animation logic
        if (velocity.Length() > 0)
		{	
			_animatedSprite.Play("walk");
		}
		else
		{
			_animatedSprite.Play("idle");
		}
    }

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Determine the Slime's movement
		velocity.X = Direction.X * Speed;
		Velocity = velocity;
		
		MoveAndSlide();
	}

	private void OnChangeDirectionTimeout()
	{
		Direction *= -1;
	}

}
