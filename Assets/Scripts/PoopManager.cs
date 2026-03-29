using Godot;

public partial class PoopManager : Node2D
{
	[Signal]
	public delegate void PoopThresholdReachedEventHandler();

	private int _poopCollected = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		foreach (Poop poop in GetChildren())
		{
			poop.PoopCollected += CollectPoop;
		}
		// Ensure count is reset on initialisation
		ResetPoop();
	}

	public void ResetPoop()
	{
		// Reset poop count to zero and close the portal
		_poopCollected = 0;
	}

	public void CollectPoop()
	{
		// Add one to the poop count
		_poopCollected += 1;

		// Open the portal if the player has collected three or more
		if (_poopCollected >= 3)
		{
			EmitSignal(SignalName.PoopThresholdReached);
		}
	}
}
