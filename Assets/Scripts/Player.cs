using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export]
	public float Speed = 20.0f;
	[Export]
	public float JumpVelocity = -50.0f;

	[Export]
	public float GravityDivisor = 3.0f;
	public Vector2 ScreenSize = new Vector2(320, 240);

	private AnimatedSprite2D _animatedSprite;
	private bool DoubleJumpFlag = false;
	private bool IdleFlag = false;


    public override void _Ready()
    {	
		// ScreenSize = GetViewportRect().Size;
        _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }

    public override void _Process(double delta)
    {
		Vector2 velocity = Velocity;
		Timer idleTimer = GetNode<Timer>("IdleTimer");
		
		// Animation Logic
        if (velocity.Length() > 0)
		{	
			if (velocity.Y > 0)
			{
				// Falling
				_animatedSprite.Play("fall");
				_animatedSprite.FlipH = velocity.X < 0;
			}
			else if (velocity.Y < 0)
			{
				// Jumping
				_animatedSprite.Play("jump");
				_animatedSprite.FlipH = velocity.X < 0;
			}
			else
			{
				// Walking
				_animatedSprite.Play("walk");
				_animatedSprite.FlipH = velocity.X < 0;	
			}
			// Stop idle processes if movement occurs
			IdleFlag = false;
			idleTimer.Stop();

		}
		else
		{	
			if (IdleFlag)
			{
				// Idling
				_animatedSprite.Play("idle");	
			}
			else
			{
				// Pre-Idling
				_animatedSprite.Play("stand");
				if (idleTimer.IsStopped())
				{
					idleTimer.Start();
				}
			}
			
		}
    }

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		Vector2 position = Position;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta / GravityDivisor;
		}
		else
		{
			DoubleJumpFlag = false;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("jump"))
		{
			if (!DoubleJumpFlag || IsOnFloor())
			{
				velocity.Y = JumpVelocity;
				if (!IsOnFloor())
				{
					DoubleJumpFlag = true;
				}	
			}
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("move_left", "move_right", "jump", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		Velocity = velocity;

		if (position.X < 0)
		{
			position.X = 0;
		}
		else if (position.X > ScreenSize.X)
		{
			position.X = ScreenSize.X;
		}

		if (position.Y < 0)
		{
			position.Y = 0;
		}
		else if (position.Y > ScreenSize.Y)
		{
			position.Y = ScreenSize.Y;
		}

		Position = position;

		MoveAndSlide();
	}

	private void OnIdleTimerTimeout()
	{
		IdleFlag = true;
	}
}
