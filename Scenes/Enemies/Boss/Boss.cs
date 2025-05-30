using Godot;
using System;

public partial class Boss : Node2D
{
	[Export] private AnimationTree _animationTree; // Animation tree for the boss
	[Export] private Area2D _trigger;
	[Export] private Area2D _hitBox;
	[Export] private Node2D _visual; // Visual node for the boss 
	[Export] private int _lives = 3; // Number of lives the boss has
	[Export] private int _points = 5;


	private AnimationNodeStateMachinePlayback _stateMachine; // State machine for the boss animations
	private bool _invincible = false; // Flag to check if the boss is invincible
	private Tween _hitTween; // Tween for the hit animation

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_trigger.AreaEntered += OnTriggerAreaEntered;
		_hitBox.AreaEntered += OnHitBoxAreaEntered; // Connect the hit box area entered signal
		_stateMachine = (AnimationNodeStateMachinePlayback)_animationTree.Get("parameters/playback");

	}

	private void SetInvincible(bool value)
	{
		_invincible = value; // Set the invincible flag
		
	}

	private void ReduceLives()
	{
		_lives--; 

		if (_lives <= 0)
		{
			Die(); // If the boss has no lives left, call the Die method
		}
		
	}

	private void Die()
	{
		if(_hitTween != null)
		{
			_hitTween.Kill(); // Stop any ongoing hit tween
		}
		
		SignalManager.EmitOnBossKilled(_points); // Emit a signal when the boss dies
		QueueFree(); // Remove the boss from the scene
	}

	private void TakeDamage()
	{


		if (_invincible) return; // If the boss is invincible, do not take damage

		SetInvincible(true); // Set the invincible flag to true
		_stateMachine.Travel("hit"); // Play the hit animation
		TweenHit();
		ReduceLives(); // Reduce the boss's lives
	}

	private void OnHitBoxAreaEntered(Area2D area)
	{
		TakeDamage();
		GD.Print("Boss hit by: " + area.Name); // Print the name of the area that hit the boss
	}


	private void OnTriggerAreaEntered(Area2D area)
	{
		_animationTree.Set("parameters/conditions/on_trigger", true); // Start the boss animation when the player enters the trigger area
		_hitBox.SetDeferred(Area2D.PropertyName.Monitoring, true); // Enable the hit box area monitoring

	}

	private void TweenHit()
	{
		_hitTween = GetTree().CreateTween(); // Create a new tween
		_hitTween.TweenProperty(_visual, Node2D.PropertyName.Position.ToString(), Vector2.Zero, 1.6f); // Move the visual node up
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
