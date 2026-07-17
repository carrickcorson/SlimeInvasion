using Godot;
using System;

public partial class Hud : Control
{
	private Label _poopCountLabel;
	private Label _portalStatusLabel;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{	
		// Initialise labels
		_poopCountLabel = GetNode<Label>("PoopTexture/PoopLabel");
		_portalStatusLabel = GetNode<Label>("PortalLabel");
	}

	public void UpdatePoopCount(int amount, int required)
	{
		// Update the text in the Poop Counter label
		_poopCountLabel.Text = "x " + amount.ToString() + "/" + required.ToString();
	}

	public void ResetHud(int required)
	{
		// Reset the poop count, the required number of poop, and the portal label
		UpdatePoopCount(0, required);
		PortalClosed();
	}

	public void PortalOpened()
	{
		// Portal label text for an open portal
		_portalStatusLabel.Text = "Portal is open!";
	}

	private void PortalClosed()
	{
		// Portal label text for a closed portal
		_portalStatusLabel.Text = "Portal is closed...";
	}
}
