using System.ComponentModel;
using Godot;

public partial class PoopManager : Node2D
{
	[Signal]
	public delegate void PoopThresholdReachedEventHandler();

	[Signal]
	public delegate void PoopCollectedEventHandler(int amount, int total);

	private int _poopCollected = 0;
	public int poopRequired = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		foreach (Poop poop in GetChildren())
		{
			poop.PoopCollected += CollectPoop;
			poopRequired += 1;
		}
		// Ensure count is reset on initialisation
		ResetPoop();
	}

	public void ResetPoop()
	{
		// Reset poop count to zero
		_poopCollected = 0;
	}

	public void CollectPoop()
	{
		// Add one to the poop count
		_poopCollected += 1;
		EmitSignal(SignalName.PoopCollected, _poopCollected, poopRequired);

		// Open the portal if the player has collected three or more
		if (_poopCollected >= poopRequired)
		{
			EmitSignal(SignalName.PoopThresholdReached);
		}
	}
}
