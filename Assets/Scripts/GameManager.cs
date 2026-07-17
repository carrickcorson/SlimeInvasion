using Godot;
using System;
using System.Threading.Tasks;

public partial class GameManager : Node2D
{
	private Node2D _levelContainer;
	private Player _player;
	private Hud _hud;

	private int _level = 1;

	// Called when the node enters the scene tree for the first time.
	public async override void _Ready()
	{
		_levelContainer = GetNode<Node2D>("Level");
		_player = GetNode<Player>("Player");
		_hud = GetNode<Hud>("CanvasLayer/HUD");

		try
		{
			await LoadLevel(_level);
		}
		catch (Exception err)
		{
			GD.Print("[ERROR] " + err.Message);
		}
	}

	// Called when the level is completed
	public async void NextLevel()
	{
		_level += 1;
		try
		{
			await LoadLevel(_level);
		}
		catch (Exception err)
		{
			GD.Print("[ERROR] " + err.Message);
		}
	}

	// Load a specified level number
	private async Task LoadLevel(int level)
	{	
		// Load the specified level as a packed scene
		string levelDirPath = "res://Assets/Scenes/Levels/";
		string level_path = levelDirPath + "level_" + level + ".tscn";
		PackedScene scene = ResourceLoader.Load<PackedScene>(level_path);

		// Catch if the scene does not exist at level_path
		if (scene == null)
		{
			GD.Print("[ERROR] Failed to load Level " + level.ToString() + ", Exiting...");
			GetTree().Quit();
			return;
		}

		// Clear all levels from _levelContainer
		foreach (Node2D child in _levelContainer.GetChildren())
		{
			child.QueueFree();
			await ToSignal(child, Node2D.SignalName.TreeExited);
		}

		// Add the next level to the level container as an instance
		Node2D level_instance = scene.Instantiate() as Node2D;
		_levelContainer.AddChild(level_instance);
		GD.Print("[DEBUG] Player has entered Level " + level.ToString());

		// Get the Exit Portal for the loaded level
		ExitPortal exitPortal = GetNodeOrNull<ExitPortal>("Level/Level" + level.ToString() + "/ExitPortal");
		if (exitPortal == null)
		{
			GD.Print("[ERROR] Failed to load Exit Portal for Level " + level.ToString());
			return;
		}

		// Get the Poop Manager for the loaded level
		PoopManager poopManager = GetNodeOrNull<PoopManager>("Level/Level" + level.ToString() + "/PoopManager");
		if (poopManager == null)
		{
			GD.Print("[ERROR] Failed to load Poop Manager for Level " + level.ToString());
			return;
		}
		
		// Initialise Exit Portal controls for the loaded level
		exitPortal.LevelCompleted += NextLevel;
		poopManager.PoopThresholdReached += exitPortal.PoopThresholdReached;

		// Initialise HUD controls for the loaded level
		poopManager.PoopCollected += _hud.UpdatePoopCount;
		poopManager.PoopThresholdReached += _hud.PortalOpened;
		_hud.ResetHud(poopManager.poopRequired);


		// Teleport player to start position
		Marker2D playerStartPosition = GetTree().GetFirstNodeInGroup("player_start_position") as Marker2D;
		_player.TeleportToLocation(playerStartPosition.Position);

	}
}
