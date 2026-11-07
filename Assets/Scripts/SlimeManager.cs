using Godot;
using System;

public partial class SlimeManager : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		foreach (Node child in GetChildren())
		{
			if (child is not Slime)
			{
				GetTree().Quit();
				throw new Exception("[ERROR] SlimeManager children must be Slime class");
			} 
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		foreach (Slime slime in GetChildren()){
			
			if (IsInBounds(slime))
			{
				
			}
		}
	}

	private bool IsInBounds(Slime slime, Polygon2D movementRegion)
	{
		bool isInBounds = true;

		Vector2 position = slime.Position;

		float minX = movementRegion.Polygon[0].X;
		float maxX = movementRegion.Polygon[1].X;

		return isInBounds;
	}
}
