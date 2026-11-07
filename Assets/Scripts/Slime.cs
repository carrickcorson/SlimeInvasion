using Godot;
using System;
using System.IO;

public partial class Slime : CharacterBody2D
{
	private float _speed = 1.0f;
	private int _direction = 1;
	private float _gravityDivisor = 3.0f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Ensure that the slime has a Polygon2D movement region child
		if ((GetChildren() == null) || (GetChild(0) is not Polygon2D))
		{
			NodePath npath = GetPath();
			GetTree().Quit();
			throw new Exception("Expected Polygon2D as child of " + npath);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta / _gravityDivisor;
		}
		velocity.X = _speed * _direction;

		Velocity = velocity;

		MoveAndSlide();
	}

	public void ChangeDirection()
	{
		_direction *= -1;
	}
}
