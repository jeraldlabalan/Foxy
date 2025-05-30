using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const string GroupName = "Player"; // Group name for the player

	private enum PlayerState
	{
		Idle,
		Run,
		Jump,
		Fall,
		Hurt
	}

	private PlayerState _state = PlayerState.Idle; // Current state of the player
	private bool _invincible = false; // Flag to check if the player is invincible

	private const float GRAVITY = 690.0f;
	private const float JUMP_VELOCITY = -260.0f;
	private const float MAX_FALL = 400.0f;
	private const float RUN_SPEED = 120.0f;

	private static readonly Vector2 HURT_JUMP = new Vector2(0, -130.0f); // Jump velocity when hurt

	[Export] private float _yFallOff = 100.0f; // Y fall off value
	[Export] private Sprite2D _sprite2D; // Reference to the sprite node
	[Export] private int _lives = 5;
	[Export] private AudioStreamPlayer2D _sound; // Reference to the sound player
	[Export] private AnimationPlayer _animationPlayer; // Reference to the animation player
	[Export] private AnimationPlayer _animationPlayerInv; // Reference to the animation player
	[Export] private Area2D _hitBox; // Reference to the hitbox area
	[Export] private Timer _invincibleTimer; // Reference to the invincibility timer
	[Export] private Timer _hurtTimer; // Reference to the hurt timer

	[Export] private Label _debugLabel; // Reference to the debug label
	[Export] private Shooter _shooter; // Reference to the shooter node

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_invincibleTimer.Timeout += OnInvincibleTimerTimeout; // Connect the invincibility timer timeout signal
		_hurtTimer.Timeout += OnHurtTimerTimeout; // Connect the hurt timer timeout signal
		_hitBox.AreaEntered += OnHitBoxAreaEntered; // Connect the hitbox area entered signal

		SignalManager.EmitOnPlayerHit(_lives);
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		Velocity = GetInput((float)delta); // Get the input velocity
		MoveAndSlide(); // Move the player based on the velocity
		CalculateStates(); // Calculate the player state
		Shoot();
		UpdateDebugLabel(); // Update the debug label
		FallenOff(); // Check if the player has fallen off the screen

	}

	private void UpdateDebugLabel()
	{
		string status = "";
		status += $"Floor: {IsOnFloor()}\n";
		status += $"State: {_state}\n";
		status += $"Velocity: {Velocity.X:0f}, {Velocity.Y:0f}";
		_debugLabel.Text = status; // Update the debug label
	}

	private void Shoot()
	{
		if (Input.IsActionJustPressed("shoot"))
		{
			_shooter.Shoot(_sprite2D.FlipH ? Vector2.Left : Vector2.Right); // Call the shoot method
		}
	}


	private void FallenOff()
	{
		if (GlobalPosition.Y < _yFallOff) return;

		_lives = 1;

		ReduceLives(); // Reduce lives if the player has fallen off the screen
	}

	private Vector2 GetInput(float delta)
	{
		Vector2 newVelocity = Velocity;
		newVelocity.X = 0; // Reset the X velocity
		newVelocity.Y += GRAVITY * delta; // Apply gravity


		if (_state == PlayerState.Hurt) return newVelocity; // If the player is hurt, do not change the state
		

		// Get the left and right inputs
		if (Input.IsActionPressed("left"))
		{
			newVelocity.X = -RUN_SPEED;
			_sprite2D.FlipH = true; // Flip the sprite

		}

		if (Input.IsActionPressed("right"))
		{
			newVelocity.X = RUN_SPEED;
			_sprite2D.FlipH = false; // Flip the sprite
		}

		// Set the speed
		// Flip the sprite

		// If jump pressed and on floor, set Y velocity to jump velocity
		if (IsOnFloor() && Input.IsActionJustPressed("jump"))
		{
			newVelocity.Y = JUMP_VELOCITY;
			SoundManager.PlayClip(_sound, SoundManager.SOUND_JUMP); // Play jump sound
		}

		// Clamp the fall in the y direction, Mathf.Clamp
		newVelocity.Y = Mathf.Clamp(newVelocity.Y, JUMP_VELOCITY, MAX_FALL);

		return newVelocity;
	}

	private void CalculateStates()
	{

		if (_state == PlayerState.Hurt) return; // If the player is hurt, do not change the state
		
		PlayerState newState;

		if (IsOnFloor())
		{
			if (Velocity.X == 0)
				newState = PlayerState.Idle;
			else
				newState = PlayerState.Run;
		}
		else
		{
			if (Velocity.Y > 0)
				newState = PlayerState.Fall;
			else
				newState = PlayerState.Jump;
		}

		SetState(newState); // Set the new state
	}

	private void SetState(PlayerState newState)
	{
		if (newState == _state) return; // If the state is the same, do nothing

		_state = newState; // Set the new state

		switch (_state)
		{
			case PlayerState.Idle:
				_animationPlayer.Play("idle");
				break;
			case PlayerState.Run:
				_animationPlayer.Play("run");
				break;
			case PlayerState.Jump:
				_animationPlayer.Play("jump");
				break;
			case PlayerState.Fall:
				_animationPlayer.Play("fall");
				break;
			case PlayerState.Hurt:
				ApplyHurtJump(); // Apply the hurt jump
				break;
			
		}
	}

	private void ApplyHurtJump()
	{
		Velocity = HURT_JUMP; // Set the velocity to the hurt jump velocity
		_animationPlayer.Play("hurt"); // Play the hurt animation
		_hurtTimer.Start(); // Start the hurt timer
	}


	private void GetInvincible()
	{
		_invincible = true;
		_animationPlayerInv.Play("invincible"); // Play the invincible animation
		_invincibleTimer.Start(); // Start the invincibility timer
	}

	private bool ReduceLives()
	{
		_lives--; // Reduce the lives by 1
		GD.Print( _lives);
		SignalManager.EmitOnPlayerHit(_lives); // Emit the player hit signal with the new lives count
		if (_lives <= 0)
		{
			SetPhysicsProcess(false); // Stop the physics process if lives are 0
			GD.Print("Dead", _lives);
			return false; // Return false if no lives left
		}
		return true; // Return true if lives are still available
	}

	private void ApplyHit()
	{
		if (_invincible) return; // If the player is invincible, do nothing
		if (!ReduceLives()) return; // Reduce lives and check if the player is still alive

		GetInvincible();
		SetState(PlayerState.Hurt); // Set the state to hurt
		SoundManager.PlayClip(_sound, SoundManager.SOUND_DAMAGE); // Play hurt sound
	}



	private void OnHitBoxAreaEntered(Area2D area)
	{
		ApplyHit();
	}

	private void OnInvincibleTimerTimeout()
	{
		_invincible = false;
		_animationPlayerInv.Play("RESET"); // Play the RESET animation

	}
	
	private void OnHurtTimerTimeout()
	{
		SetState(PlayerState.Idle); // Set the state to idle after hurt
	}
}
