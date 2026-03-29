using Godot;

public partial class Poop : Area2D
{
	[Signal]
	public delegate void PoopCollectedEventHandler();

	private AnimatedSprite2D _animatedSprite2D;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_animatedSprite2D.Play("default");
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is Player)
		{
			QueueFree();
			GD.Print("Player has gathered some poop!");

			CollectPoop();
		}
	}

	private void CollectPoop()
	{
		EmitSignal(SignalName.PoopCollected);
	}
}
