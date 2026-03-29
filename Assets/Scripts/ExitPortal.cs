using Godot;

public partial class ExitPortal : Area2D
{
	[Signal]
	public delegate void LevelCompletedEventHandler();

	private AnimatedSprite2D _animatedSprite2D;
	private CollisionShape2D _collisionShape;

	private bool _exitOpen = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");

		// Ensure portal is closed upon initialisation
		Close();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		if (_exitOpen)
		{
			_animatedSprite2D.Play("active");
		}
		else
		{
			_animatedSprite2D.Play("dormant");
		}
	}

	public void Open()
	{
		_exitOpen = true;
	}

	public void Close()
	{
		_exitOpen = false;
	}

	public void PoopThresholdReached()
	{
		Open();
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is Player && _exitOpen)
		{
			CompleteLevel();
		}
	}


	private void CompleteLevel()
	{
		EmitSignal(SignalName.LevelCompleted);
	}
}
