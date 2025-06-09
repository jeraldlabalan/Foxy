using Godot;
using System;

public partial class ShakeCam : Camera2D
{
	[Export] private Timer _shakeTimer; // Timer for the shake effect
	[Export] private double _shakeAmount = 5.0f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetProcess(false); // Disable processing by default
		SignalManager.Instance.OnPlayerHit += OnPlayerHit; // Connect to the player hit signal
		_shakeTimer.Timeout += OnShakeTimerTimeout; // Connect to the shake timer timeout signal
	}

	public override void _ExitTree()
	{
		SignalManager.Instance.OnPlayerHit -= OnPlayerHit; // Disconnect from the player hit signal
		_shakeTimer.Timeout -= OnShakeTimerTimeout; // Disconnect from the shake timer timeout signal
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Offset = GetRandomOffset(); // Apply a random offset to the camera for shaking effect
	}

	private void OnPlayerHit(int lives)
	{
		_shakeTimer.Start(); // Start the shake timer when the player is hit
		SetProcess(true); // Enable processing to allow the camera to shake
	}
	
	private Vector2 GetRandomOffset()
	{
		return new Vector2(
			(float)GD.RandRange(-_shakeAmount, _shakeAmount),
			(float)GD.RandRange(-_shakeAmount, _shakeAmount)
		);
	}

	private void OnShakeTimerTimeout()
	{
		SetProcess(false); // Disable processing when the shake timer times out
		Offset = Vector2.Zero; // Reset the camera offset
	}
}
