using Godot;
using System;

public partial class BulletBase : Area2D
{
	private Vector2 _direction = Vector2.Right; // Default direction
	private double _lifeSpan = 20.0f;
	private double _lifeTime = 0.0f;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AreaEntered += OnAreaEntered; // Connect the AreaEntered signal
		//Setup(new Vector2(20.0f, -40.0f), 3.0f, 50.0f); // Setup the bullet with default values
	}

	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Check if expired
		CheckExpired(delta); // Check if the bullet has expired
		// Update the position
		Position += _direction * (float)delta; // Move the bullet in the direction
	}

	public void Setup(Vector2 dir, float lifeSpan, float speed)
	{
		// Set the direction and lifespan
		_direction = dir.Normalized() * speed; // Normalize the direction
		_lifeSpan = lifeSpan; // Set the lifespan
		GD.Print(_direction);
	}

	public void CheckExpired(double delta)
	{
		// QueueFree if expired
		_lifeTime += delta; // Update the life time

		if(_lifeTime > _lifeSpan)
		{
			SetProcess(false); // Stop the process
			QueueFree(); // Remove the bullet if expired
		}

		
	}

	private void OnAreaEntered(Area2D area)
	{
		QueueFree(); // Remove the bullet when it enters an area
	}
}
