using Godot;
using System;

public partial class Explosion : AnimatedSprite2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AnimationFinished += OnAnimationFinished; // Connect the animation finished signal
	}

    private void OnAnimationFinished()
    {
        QueueFree(); // Remove the explosion from the scene when the animation finishes
    }

}
