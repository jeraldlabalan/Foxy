using Godot;
using System;

public partial class Checkpoint : Area2D
{

	const string TRIGGER_PATH = "parameters/conditions/on_trigger";

	[Export] private AnimationTree _animationTree; // Animation tree for the checkpoint
	[Export] private AudioStreamPlayer2D _sound; // Audio player for checkpoint sound

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AreaEntered += OnAreaEntered; // Connect the area entered signal
		SignalManager.Instance.OnBossKilled += OnBossKilled; // Connect the boss killed signal

	}
	
	public override void _ExitTree()
	{
		SignalManager.Instance.OnBossKilled -= OnBossKilled; // Disconnect the boss killed signal

	}

	private void OnBossKilled(int points)
	{
		SetDeferred(PropertyName.Monitoring, true); // Enable monitoring for the checkpoint
		_animationTree.Set(TRIGGER_PATH, true); // Start the checkpoint animation when the boss is killed
	}

    private void OnAreaEntered(Area2D area)
	{
		SignalManager.EmitOnLevelComplete(); // Emit a signal when the checkpoint is reached
		SoundManager.PlayClip(_sound, SoundManager.SOUND_WIN); // Play the checkpoint sound
	}

}
