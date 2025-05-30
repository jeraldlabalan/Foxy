using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Hud : Control
{
	[Export] private Label _scoreLabel;
	[Export] private HBoxContainer _hbHearts;

	private List<TextureRect> _hearts;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_hearts = _hbHearts.GetChildren().OfType<TextureRect>().ToList();
		OnScoreUpdated();
		ConnectSignals();
	}

	private void ConnectSignals()
	{
		SignalManager.Instance.OnPlayerHit += OnPlayerHit;
		SignalManager.Instance.OnScoreUpdated += OnScoreUpdated;
	}

    private void OnScoreUpdated()
    {
        _scoreLabel.Text = ScoreManager.Score.ToString("D4");
    }


    private void OnPlayerHit(int lives)
    {
        for (int i = 0; i < _hearts.Count; i++)
		{
			_hearts[i].Visible = lives > i;
		}
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
