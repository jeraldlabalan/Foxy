using Godot;
using System;

public partial class Shooter : Node2D
{
	[Export] private float _speed = 50.0f;
	[Export] private float _lifeSpan = 10.0f;
	[Export] private GameObjectType _bulletKey;
	[Export] private float _shootDelay = 0.7f;
	[Export] private AudioStreamPlayer2D _sound;
	[Export] private Timer _shootTimer;

	private bool _canShoot = true;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_shootTimer.Timeout += OnShootTimerTimeout; // Connect the shoot timer timeout signal
	}

	
	public void Shoot(Vector2 direction)
	{
		if(_canShoot == false) return; // Check if the player can shoot

		_canShoot = false; // Set the shoot flag to false

		SignalManager.EmitOnCreateBullet(
			GlobalPosition,
			direction,
			_speed,
			_lifeSpan,
			(int)_bulletKey
		);
		SoundManager.PlayClip(_sound, SoundManager.SOUND_LASER); // Play the shooting sound
		_shootTimer.Start();
	}

	private void OnShootTimerTimeout()
	{
		_canShoot = true; // Reset the shoot flag
	}

	
}
