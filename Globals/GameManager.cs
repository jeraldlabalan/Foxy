using Godot;
using System;
using System.Collections.Generic;

public partial class GameManager : Node
{
    const int TotalLevels = 3;
    
    private Dictionary<int, PackedScene> _levels = new Dictionary<int, PackedScene>();

    private PackedScene _mainScene = GD.Load<PackedScene>("res://Scenes/Main/Main.tscn");

    int _currentLevel = 0;

    public static GameManager Instance { get; private set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        Instance = this;
	}

    public static void LoadNextLevelScene()
    {
        if (Instance._currentLevel < TotalLevels)
        {
            Instance._currentLevel++;
            Instance.GetTree().ChangeSceneToPacked(Instance._levels[Instance._currentLevel]);
        }
    }

    public static void LoadMainScene()
    {
        Instance.GetTree().ChangeSceneToPacked(Instance._mainScene);
    }
}
