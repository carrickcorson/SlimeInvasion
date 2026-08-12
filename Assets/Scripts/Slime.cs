using Godot;
using System;

public partial class Slime : Node2D
{
	private CharacterBody2D _body;
	private Area2D _movementArea;
	private AnimatedSprite2D _animatedSprite2D;

	private float _speed = 10.0f;
	private Vector2 _direction = new Vector2(1.0f, 0.0f); // moving right
	private float _gravityDivisor = 3.0f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_body = GetNode<CharacterBody2D>("Body");
		_animatedSprite2D = GetNode<AnimatedSprite2D>("Body/AnimatedSprite2D");
		_movementArea = GetNode<Area2D>("MovementArea");
		_movementArea.BodyExited += OnBodyExited;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Mathf.Abs(_body.Velocity.X) > 0)
		{
			_animatedSprite2D.Play("move");
		}
		else
		{
			_animatedSprite2D.Play("idle");
		}
	}

    public override void _PhysicsProcess(double delta)
    {
		Vector2 velocity = _body.Velocity;

		velocity += _body.GetGravity() * (float)delta / _gravityDivisor;
		velocity.X = _direction.X * _speed;

		_body.Velocity = velocity;
        _body.MoveAndSlide();
    }

	private void OnBodyExited(Node2D body)
	{
		if (body is CharacterBody2D)
		{
			_direction.X *= -1.0f;
		}
	}
}
