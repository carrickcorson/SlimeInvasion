using Godot;
using System;
using System.Threading.Tasks;

public partial class Player : CharacterBody2D
{
	[Export]
	public float speed = 25.0f;
	[Export]
	public float jumpVelocity = -80.0f;
	[Export]
	public float gravityDivisor = 3.0f;

	public Vector2 _screenSize;

	private AnimatedSprite2D _animatedSprite;
	private bool _doubleJumpFlag = false;
	private bool _idleFlag = false;


	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_screenSize = GetViewportRect().Size;
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
			_idleFlag = false;
			idleTimer.Stop();

		}
		else
		{
			if (_idleFlag)
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

	public override void _Input(InputEvent @event)
	{
		Vector2 velocity = Velocity;
		base._Input(@event);

		// Handle jump
		if (@event.IsActionPressed("jump"))
		{
			if (!_doubleJumpFlag || IsOnFloor())
			{
				velocity.Y = jumpVelocity;
				if (!IsOnFloor())
				{
					_doubleJumpFlag = true;
				}
			}
		}

		// Handle drop down
		if (@event.IsActionPressed("move_down"))
		{
			SetCollisionMaskValue(10, false);
		}
		else
		{
			SetCollisionMaskValue(10, true);
		}

		Velocity = velocity;

	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		Vector2 position = Position;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta / gravityDivisor;
		}
		else
		{
			_doubleJumpFlag = false;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("move_left", "move_right", "jump", "move_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, speed);
		}

		Velocity = velocity;

		// Keep player on the screen
		if (position.X < 0)
		{
			position.X = 0;
		}
		else if (position.X > _screenSize.X)
		{
			position.X = _screenSize.X;
		}

		if (position.Y < 0)
		{
			position.Y = 0;
		}
		else if (position.Y > _screenSize.Y)
		{
			position.Y = _screenSize.Y;
		}

		Position = position;

		MoveAndSlide();
	}

	public async Task TeleportToLocation(Vector2 new_position)
	{
		Camera2D camera = GetNode<Camera2D>("Camera2D");

		Position = new_position;
		camera.ResetSmoothing();
	}

	private void OnIdleTimerTimeout()
	{
		_idleFlag = true;
	}

}
