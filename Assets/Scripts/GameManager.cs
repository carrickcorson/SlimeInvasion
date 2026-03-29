using Godot;
using System;
using System.Threading.Tasks;

public partial class GameManager : Node2D
{
	private Node2D _levelContainer;
	private Player _player;

	private int _level = 1;

	// Called when the node enters the scene tree for the first time.
	public async override void _Ready()
	{
		_levelContainer = GetNode<Node2D>("Level");
		_player = GetNode<Player>("Player");

		try
		{
			await LoadLevel(_level);
		}
		catch (Exception err)
		{
			GD.Print("[ERROR] " + err.Message);
		}
	}

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

	private async Task LoadLevel(int level)
	{
		string levelDirPath = "res://Assets/Scenes/Levels/";
		string level_path = levelDirPath + "level_" + level + ".tscn";
		PackedScene scene = ResourceLoader.Load<PackedScene>(level_path);

		// Catch if the scene does not exist at level_path
		if (scene == null)
		{
			GD.Print("[WARNING] Failed to load Level " + level.ToString());
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

		// Get the level manager for the loaded level
		LevelManager levelManager = GetNodeOrNull<LevelManager>("Level/Level" + level.ToString());
		if (levelManager == null)
		{
			GD.Print("[ERROR] Failed to load LevelManager for Level " + level.ToString());
			return;
		}
		levelManager.LevelCompleted += NextLevel;

		// Teleport player to start position
		Marker2D playerStartPosition = GetTree().GetFirstNodeInGroup("player_start_position") as Marker2D;
		_player.TeleportToLocation(playerStartPosition.Position);

	}
}
