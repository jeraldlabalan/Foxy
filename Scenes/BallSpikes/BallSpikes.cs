using Godot;
using System;

public partial class BallSpikes : PathFollow2D
{
	[Export] private float _speed = 50.0f; // Speed of the ball spikes
	[Export] private float _spinSpeed = 400.0f; // Speed of the ball spikes


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Progress += _speed * (float)delta; // Move the ball spikes along the path
		Rotation += _spinSpeed * (float)delta; // Spin the ball spikes around their center
	}
}
