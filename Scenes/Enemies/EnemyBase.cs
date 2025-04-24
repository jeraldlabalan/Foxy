using Godot;
using System;

[GlobalClass]
public partial class EnemyBase : CharacterBody2D
{
	[Export] protected int _points = 1; // Points to be awarded when the enemy is defeated
	[Export] protected float _speed = 30.0f; // Speed of the enemy
	[Export] private float _yFallOff = 100.0f; // Y fall off value


	[Export] protected VisibleOnScreenNotifier2D _visibleOnScreenNotifier2D; // Reference to the visible notifier
	[Export] protected AnimatedSprite2D _animatedSprite2D; // Reference to the animated sprite
	[Export] protected Area2D _hitBox; // Reference to the hitbox area

	protected Player _playerRef; // Reference to the player
	protected float _gravity = 800.0f; // Gravity value

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_playerRef = (Player)GetTree().GetFirstNodeInGroup(Player.GroupName); // Get the player reference
		_visibleOnScreenNotifier2D.ScreenEntered += OnScreenEntered; // Connect the screen entered signal
		_visibleOnScreenNotifier2D.ScreenExited += OnScreenExited; // Connect the screen exited signal
		_hitBox.AreaEntered += OnHitBoxAreaEntered; // Connect the hitbox entered signal
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		FallenOff(); // Check if the enemy has fallen off the screen
	}

	protected virtual void OnScreenEntered()
	{
	}

	protected virtual void OnScreenExited()
	{
	}

	private void OnHitBoxAreaEntered(Area2D area)
	{
		Die(); // Call the Die method when the hitbox area is entered
	}

	private void Die()
	{	
		SignalManager.EmitOnEnemyHit(_points, GlobalPosition); // Emit the enemy hit signal
		SignalManager.EmitOnCreateObject(GlobalPosition, (int)GameObjectType.Explosion);
		SignalManager.EmitOnCreateObject(GlobalPosition, (int)GameObjectType.Pickup);
		SetPhysicsProcess(false); // Stop the physics process
		QueueFree(); // Remove the enemy from the scene
	}

	private void FallenOff()
	{
		if(GlobalPosition.Y > _yFallOff)
		{
			SetPhysicsProcess(false); // Stop the physics process
			QueueFree(); // Remove the enemy from the scene
		}
	}

	
}
