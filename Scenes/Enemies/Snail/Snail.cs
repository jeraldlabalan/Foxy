using Godot;
using System;

public partial class Snail : EnemyBase
{
	
	[Export] private RayCast2D _floorDetection; // Reference to the floor detection raycast
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta); // Call the base class physics process
		
		Vector2 velocity = Velocity;

		if(!IsOnFloor())
		{
			velocity.Y += _gravity * (float)delta; // Apply gravity
		}
		else
		{
			velocity.X = _animatedSprite2D.FlipH ? _speed : -_speed; // Reset Y velocity when on the floor
		}
		
		Velocity = velocity; // Update the velocity
		
		MoveAndSlide(); // Move the enemy based on the velocity
		FlipMe(); // Call the FlipMe method
	}

	private void FlipMe()
	{
		if(IsOnFloor() && (IsOnWall() || !_floorDetection.IsColliding()))
		{
			_animatedSprite2D.FlipH = !_animatedSprite2D.FlipH; // Flip the sprite
			_floorDetection.Position = new Vector2(_floorDetection.Position.X * -1, _floorDetection.Position.Y); // Flip the floor detection position
		}
	}
}
