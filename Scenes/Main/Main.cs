using Godot;
using System;

public partial class Main : Control
{

	private PackedScene _highscoreLabel = GD.Load<PackedScene>(

		"res://Scenes/HighscoreLabel/HighscoreLabel.tscn"
	);

	[Export] private GridContainer _gridContainer;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//ScoreManager.SimulateAddingAndSavingSomeScores();
		SetScores();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("shoot"))
		{
			ScoreManager.ResetScore();
			GameManager.LoadNextLevelScene();
		}

		if (Input.IsActionJustPressed("quit"))
		{
			GetTree().Quit();
		}
	}

	private void SetScores()
	{

		foreach (GameScore score in ScoreManager.Instance.ScoresHistory)
		{
			var hsl = _highscoreLabel.Instantiate<HighscoreLabel>();
			_gridContainer.AddChild(hsl);
			hsl.SetHighScoreText(score);
		}

	}
}
