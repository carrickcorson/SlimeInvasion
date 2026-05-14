using Godot;
using System;

public partial class Hud : Control
{
	private Label _poopCountLabel;
	private Label _portalStatusLabel;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_poopCountLabel = GetNode<Label>("PoopTexture/PoopLabel");
		_portalStatusLabel = GetNode<Label>("PortalLabel");
	}

	public void UpdatePoopCount(int amount, int required)
	{
		_poopCountLabel.Text = "x " + amount.ToString() + "/" + required.ToString();
		if (amount >= required)
		{
			PortalOpened();
		}
	}

	public void ResetHud(int required)
	{
		UpdatePoopCount(0, required);
		PortalClosed();
	}

	private void PortalClosed()
	{
		_portalStatusLabel.Text = "Portal is closed...";
	}

	private void PortalOpened()
	{
		_portalStatusLabel.Text = "Portal is open!";
	}
}
