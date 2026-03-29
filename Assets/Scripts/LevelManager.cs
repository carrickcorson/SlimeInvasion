using Godot;
using System;

public partial class LevelManager : Node2D
{
	[Signal]
	public delegate void LevelCompletedEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		PoopManager poopManager = GetNode<PoopManager>("PoopManager");
		ExitPortal portal = GetNode<ExitPortal>("ExitPortal");


		poopManager.PoopThresholdReached += portal.PoopThresholdReached;
		portal.LevelCompleted += GoToNextLevel;
	}

	private void GoToNextLevel()
	{
		EmitSignal(SignalName.LevelCompleted);
	}
}
