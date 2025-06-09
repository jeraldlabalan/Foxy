using Godot;
using System;
using System.Collections.Generic;

class TargetDistanceTime
{
	public Vector2 PositionTo;

	public float Time;

	public TargetDistanceTime(Vector2 positionFrom, Vector2 positionTo, float speed)
	{

		Time = positionFrom.DistanceTo(positionTo) / speed;
		PositionTo = positionTo;

	}
	
	public override string ToString()
	{
		return $"TargetDistanceTime: PositionTo={PositionTo}, Time={Time}";
	}

}

public partial class MovingPlatformAnim : AnimatableBody2D
{

	[Export] private Godot.Collections.Array<Marker2D> _points = new();
	[Export] private float _speed = 150.0f;

	private List<TargetDistanceTime> _targetPoints = new();
	private Tween _tween;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		if (_points.Count < 2) return;
		SignalManager.Instance.OnGameOver += OnGameOver;
		SignalManager.Instance.OnLevelComplete += OnGameOver;

		GlobalPosition = _points[0].GlobalPosition;
		SetUpPoints();
		RunTween();

		
	}

	private void KillTween()
	{
		if (_tween != null)
		{
			_tween.Kill();
		}
	}

   

    private void OnGameOver()
	{
		KillTween();
		
	}

	public override void _Process(double delta)
	{
		// No need to process anything here, the tween handles the movement
	}

    private void SetUpPoints()
	{

		for (int i = 0; i < _points.Count - 1; i++)
		{
			_targetPoints.Add(new TargetDistanceTime(
				_points[i].GlobalPosition,
				_points[i + 1].GlobalPosition,
				_speed
			));
		}
		_targetPoints.Add(new TargetDistanceTime(
				_points[_points.Count - 1].GlobalPosition,
				_points[0].GlobalPosition,
				_speed
			));
	}

	public override void _ExitTree()
	{
		KillTween();

		SignalManager.Instance.OnGameOver -= OnGameOver;
		SignalManager.Instance.OnLevelComplete -= OnGameOver;
	}

	private void RunTween()
	{
		_tween = GetTree().CreateTween();
		_tween.SetLoops(0);


		foreach (TargetDistanceTime tp in _targetPoints)
		{
			_tween.TweenProperty(
				this,
				PropertyName.GlobalPosition.ToString(),
				tp.PositionTo,
				tp.Time
			);
		}
		_tween.TweenInterval(0.02f); // Small delay to ensure smooth transition
	}
}
