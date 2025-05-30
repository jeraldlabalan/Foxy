using Godot;
using System;

public partial class Frog : EnemyBase
{
	private const float JUMP_MIN_TIME = 2.0f;
	private const float JUMP_MAX_TIME = 3.0f;
	private const float JUMP_VELOCITY_X = 100.0f;
	private const float JUMP_VELOCITY_Y = 150.0f;
	private static readonly Vector2 JUMP_VELOCITY_R = new Vector2(JUMP_VELOCITY_X, -JUMP_VELOCITY_Y);
	private static readonly Vector2 JUMP_VELOCITY_L = new Vector2(-JUMP_VELOCITY_X, -JUMP_VELOCITY_Y);	

	[Export] private Timer _jumpTimer; // Timer for the frog's jump

	private bool _seenPlayer = false; // Flag to check if the player is seen
	private bool _jump = false;
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready(); // Call the base class ready method
		_jumpTimer.Timeout += OnJumpTimerTimeout; // Connect the timer's timeout signal to the method
	}


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta); // Call the base class physics process method
		
		if(!IsOnFloor())
		{
			Velocity += new Vector2(0, _gravity * (float)delta); // Apply gravity to the frog
		}
		else
		{
			Velocity = new Vector2(0, 0); // Reset velocity when on the floor
			_animatedSprite2D.Play("idle"); // Play the idle animation
		}
		
		
		
		
		ApplyJump();
		MoveAndSlide();
		FlipMe();
	}

	private void FlipMe() 
	{
		_animatedSprite2D.FlipH = _playerRef.GlobalPosition.X > GlobalPosition.X;
	}

	private void ApplyJump()
	{
		if(IsOnFloor() && _seenPlayer && _jump)
		{
			_jump = false; // Reset the jump flag
			_animatedSprite2D.Play("jump"); // Play the jump animation
			Velocity = _animatedSprite2D.FlipH ? JUMP_VELOCITY_R : JUMP_VELOCITY_L; // Set the jump velocity based on the direction
			StartTimer(); // Start the jump timer again
		}
	}

	private void StartTimer()
	{
		_jumpTimer.WaitTime = GD.RandRange(JUMP_MIN_TIME, JUMP_MAX_TIME); // Set a random wait time for the jump timer
		_jumpTimer.Start(); // Start the jump timer
		GD.Print("Time started with ", _jumpTimer.WaitTime);
	}

	private void OnJumpTimerTimeout()
    {
        GD.Print("Timer timeout");
		_jump = true;
    }

	protected override void OnScreenEntered()
	{
		if(_seenPlayer == false)
		{
			_seenPlayer = true; // Set the flag to true when the player is seen
			GD.Print("Frog: Player seen");
			StartTimer(); // Start the jump timer
		}
	}

}
