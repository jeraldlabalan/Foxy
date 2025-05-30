using Godot;
using System;

public partial class FruitPickUp : Area2D
{

	[Export] private AnimatedSprite2D _animatedSprite2D;
	[Export] private Timer _lifeTimer;
	[Export] private float _jump = -80.0f;
	[Export] private int _points = 2;

	private float _startY;
	private float _speedY;
	private bool _stopped = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		_speedY = _jump; // Set the initial speed for the jump
		_startY = Position.Y; // Store the initial Y position of the fruit pick up
		_lifeTimer.Timeout += OnLifeTimerTimeout; // Connect the life timer timeout signal
		AreaEntered += OnAreaEntered; // Connect the area entered signal
		PlayRandomAnimation();
	}

	private void PlayRandomAnimation()
	{
		var animaNames = _animatedSprite2D.SpriteFrames.GetAnimationNames();

		if (animaNames.Length > 0)
		{
			string randomName = animaNames[new Random().Next(animaNames.Length)];
			_animatedSprite2D.Play(randomName);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!_stopped) return; // If the fruit pick up is stopped, do not process further

		Position += new Vector2(0, _speedY * (float)delta); // Move the fruit pick up downwards
		_speedY += Gravity * (float)delta; // Apply gravity to the speed

		if (Position.Y > _startY) // If the fruit pick up has moved down by 50 pixels
		{
			_stopped = true;
			
		}

	}


	private void OnLifeTimerTimeout()
	{
		QueueFree(); // Remove the fruit pick up from the scene when the timer times out
	}

	private void OnAreaEntered(Area2D area)
	{

		SignalManager.EmitOnPickupHit(_points); // Emit the player pick up signal with points
		QueueFree(); // Remove the fruit pick up from the scene when an area enters
    }

}
