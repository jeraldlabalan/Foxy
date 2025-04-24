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
	
		SignalManager.Instance.OnCreateBullet += OnCreateBullet;
	}

	private void AddObject(Node node)
	{
		AddChild(node);
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
