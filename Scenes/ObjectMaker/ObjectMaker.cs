using Godot;
  using System;		
using System.Collections.Generic;

public partial class ObjectMaker : Node2D
{
	private Dictionary<GameObjectType, PackedScene> _objectScenes = new();



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_objectScenes.Add(GameObjectType.BulletPlayer, GD.Load<PackedScene>(
			"res://Scenes/Bullets/PlayerBullet/PlayerBullet.tscn"

		));
		_objectScenes.Add(GameObjectType.BulletEnemy, GD.Load<PackedScene>(
			"res://Scenes/Bullets/EnemyBullet/EnemyBullet.tscn"
		));

		_objectScenes.Add(GameObjectType.Explosion
		, GD.Load<PackedScene>(
			"res://Scenes/Explosion/Explosion.tscn"
		));

		_objectScenes.Add(GameObjectType.Pickup
		, GD.Load<PackedScene>(
			"res://Scenes/FruitPickup/FruitPickup.tscn"
		));


		SignalManager.Instance.OnCreateBullet += OnCreateBullet;
		SignalManager.Instance.OnCreateObject += OnCreateObject;	
	}

	public override void _ExitTree()
	{
		SignalManager.Instance.OnCreateBullet -= OnCreateBullet;
		SignalManager.Instance.OnCreateObject -= OnCreateObject;
	}


    private void AddObject(Node node)
	{
		AddChild(node);
	}

	private void OnCreateObject(Vector2 position, int gameObjectType)
	{
		GameObjectType goType = (GameObjectType)gameObjectType;
		Node2D newScene = _objectScenes[goType].Instantiate<Node2D>();
		newScene.GlobalPosition = position;
		CallDeferred(MethodName.AddObject, newScene);

	}


	private void OnCreateBullet(
		Vector2 position,
		Vector2 direction,
		float speed,
		float lifeSpan,
		int gameObjectType)
	{
		GameObjectType goType = (GameObjectType)gameObjectType;
		BulletBase newScene = _objectScenes[goType].Instantiate<BulletBase>();
		newScene.GlobalPosition = position;
		newScene.Setup(direction, lifeSpan, speed);
		CallDeferred(MethodName.AddObject, newScene);
	}

}
