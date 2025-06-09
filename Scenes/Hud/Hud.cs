using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Hud : Control
{
	private const string MOVEABLES = "Moveables";
	[Export] private Label _scoreLabel;
	[Export] private HBoxContainer _hbHearts;
	[Export] private ColorRect _colorRect;
	[Export] private VBoxContainer _vbLevelComplete;
	[Export] private VBoxContainer _vbGameOver;
	[Export] private AudioStreamPlayer _sound;
	[Export] private Timer _continueTimer;

	private List<TextureRect> _hearts;
	private bool _canContinue = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_hearts = _hbHearts.GetChildren().OfType<TextureRect>().ToList();
		OnScoreUpdated();
		ConnectSignals();
	}

	public override void _ExitTree()
	{
		DisconnectSignals();
	}

	private void DisconnectSignals()
	{
		SignalManager.Instance.OnPlayerHit -= OnPlayerHit;
		SignalManager.Instance.OnLevelStart -= OnPlayerHit;
		SignalManager.Instance.OnScoreUpdated -= OnScoreUpdated;
		SignalManager.Instance.OnGameOver -= OnGameOver;
		SignalManager.Instance.OnLevelComplete -= OnLevelComplete;

	}

	private void ConnectSignals()
	{
		SignalManager.Instance.OnPlayerHit += OnPlayerHit;
		SignalManager.Instance.OnLevelStart += OnPlayerHit;
		SignalManager.Instance.OnScoreUpdated += OnScoreUpdated;
		SignalManager.Instance.OnGameOver += OnGameOver;
		SignalManager.Instance.OnLevelComplete += OnLevelComplete;
		_continueTimer.Timeout += OnContinueTimerTimeout;

	}

    private void OnContinueTimerTimeout()
    {
        _canContinue = true;
		
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.

	public override void _Process(double delta)
	{
		if (_canContinue && Input.IsActionJustPressed("shoot"))
		{
			if (_vbLevelComplete.Visible)
			{
				GameManager.LoadNextLevelScene();
			}
			else
			{
				GameManager.LoadMainScene();
			}
		}
		
	}

	private void StopAllMoveables()
	{
		List<Node2D> moveableNodes = GetTree().GetNodesInGroup(MOVEABLES)
		.OfType<Node2D>().ToList();

		foreach (Node2D moveable in moveableNodes)
		{
			moveable.SetPhysicsProcess(false);
			moveable.SetProcess(false);
		}
	}

	private void ShowHud(bool gameOver)
	{
		if (gameOver)
		{
			_vbGameOver.Show();
			_sound.Play();
		}
		else
		{
			_vbLevelComplete.Show();

		}

		_continueTimer.Start();
		_colorRect.Show();
		StopAllMoveables();
	}

    private void OnLevelComplete()
	{
		ShowHud(false);
		
	}


    private void OnGameOver()
    {
        ShowHud(true);
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

    
}
