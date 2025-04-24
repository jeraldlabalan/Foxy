using Godot;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class ScoreManager : Node
{
    private const string SCORE_FILE = "user://SCORES.dat";
    private const int MAX_SCORES = 10;

    private int _score = 0;

    private List<GameScore> _scoresHistory = new ();

    public List<GameScore> ScoresHistory => _scoresHistory;

    public static int Score => Instance._score;

    public static ScoreManager Instance { get; private set; }

    public override void _Ready()
    {
        Instance = this;
        LoadScoresHistory();
        ConnectSignals();
    }

    private void ConnectSignals()
    {
        SignalManager.Instance.OnBossKilled += OnBossKilled;
        SignalManager.Instance.OnPickupHit += OnPickupHit;
        SignalManager.Instance.OnEnemyHit += OnEnemyHit;
        SignalManager.Instance.OnGameOver += OnGameOver;
    }

    public void UpdateScore(int points)
    {
        _score += points;
        SignalManager.EmitOnScoreUpdated();
    }

    public static void ResetScore()
    {
        Instance._score = 0;
    }

    private void SaveScores()
    {
        _scoresHistory.Add(new GameScore
        {
            DateAchieved = DateTime.Now,
            Score = _score
        });

        _scoresHistory = _scoresHistory
            .OrderByDescending(s => s.Score)
            .Take(MAX_SCORES)
            .ToList();

        using var file = FileAccess.Open(SCORE_FILE, FileAccess.ModeFlags.Write);

        if (file != null)
        {
            var jsonStr = JsonConvert.SerializeObject(_scoresHistory.Take(MAX_SCORES).ToList());
            file.StoreString(jsonStr);
        }
    }

    private void LoadScoresHistory()
    {
        using var file = FileAccess.Open(SCORE_FILE, FileAccess.ModeFlags.Read);
        if (file != null)
        {
            var jsonData = file.GetAsText();
            if (!string.IsNullOrEmpty(jsonData))
            {
                _scoresHistory = JsonConvert.DeserializeObject<List<GameScore>>(jsonData) ?? new();
                _scoresHistory.Sort((a, b) => b.Score.CompareTo(a.Score));
            }
        }
    }

    private void OnGameOver()
    {
        SaveScores();
    }

    private void OnBossKilled(int points)
    {
        UpdateScore(points);
    }

    private void OnPickupHit(int points)
    {
        UpdateScore(points);
    }

    private void OnEnemyHit(int points, Vector2 _enemyPosition)
    {
        UpdateScore(points);
    }
}
