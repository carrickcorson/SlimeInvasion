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

		if ((GetChildren() == null) || (GetChild(0) is not Polygon2D))
		{
			NodePath npath = GetPath();
			GetTree().Quit();
			throw new Exception("Expected Polygon2D as child of " + npath);
		}
	}

	public override void _Process(double delta)
	{
		if (!_IsInBounds())
		{
			_direction *= -1;
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

	private bool _IsInBounds()
	{
		bool isInBounds = true;

		Polygon2D movementRegion = GetNode<Polygon2D>("Polygon2D");

		Vector2 position = Position;

		float minX = movementRegion.Polygon[0].X;
		float maxX = movementRegion.Polygon[1].X;

		return isInBounds;
	}
}
