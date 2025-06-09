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

        for (int i = 1; i <= TotalLevels; i++)
        {
            _levels.Add(
                i,
                GD.Load<PackedScene>($"res://Scenes/LevelBase/Level{i}.tscn")
            );
        }
	}

    private void SetNextLevel()
    {
        _currentLevel++;
        if (_currentLevel > TotalLevels)
        {
            _currentLevel = 1; // Reset to first level if exceeded
        }
    }


    public static void LoadNextLevelScene()
    {
            Instance.SetNextLevel();
            Instance.GetTree().ChangeSceneToPacked(Instance._levels[Instance._currentLevel]);
    }

    public static void LoadMainScene()
    {
        Instance._currentLevel = 0; // Reset level counter
        Instance.GetTree().ChangeSceneToPacked(Instance._mainScene);    
    }
}
