using Godot;
using System;

public partial class SignalManager : Node
{
    [Signal]
    public delegate void OnEnemyHitEventHandler(int points, Vector2 enemyPosition);

    [Signal]
    public delegate void OnPickupHitEventHandler(int points);

    [Signal]
    public delegate void OnBossKilledEventHandler(int points);

    [Signal]
    public delegate void OnPlayerHitEventHandler(int lives);

    [Signal]
    public delegate void OnLevelStartEventHandler(int lives);

    [Signal]
    public delegate void OnLevelCompleteEventHandler();

    [Signal]
    public delegate void OnGameOverEventHandler();

    [Signal]
    public delegate void OnScoreUpdatedEventHandler();
    
    [Signal]
    public delegate void OnCreateObjectEventHandler(Vector2 position, int gameObjectType);
    
    [Signal]
    public delegate void OnCreateBulletEventHandler(Vector2 position, Vector2 direction, float speed, float lifeSpan, int gameObjectType);

    
    
    public static SignalManager Instance { get; private set; }
	

    public override void _EnterTree()
    {
        Instance = this;
    }


    public static void EmitOnEnemyHit(int points, Vector2 enemyPosition)
    {
        Instance.EmitSignal(SignalName.OnEnemyHit, points, enemyPosition);
    }

    public static void EmitOnPickupHit(int points)
    {
        Instance.EmitSignal(SignalName.OnPickupHit, points);
    }

    public static void EmitOnBossKilled(int points)
    {
        Instance.EmitSignal(SignalName.OnBossKilled, points);
    }

    public static void EmitOnPlayerHit(int lives)
    {
        Instance.EmitSignal(SignalName.OnPlayerHit, lives);
    }

    public static void EmitOnLevelStart(int lives)
    {
        Instance.EmitSignal(SignalName.OnLevelStart, lives);
    }
    

    public static void EmitOnLevelComplete()
    {
        Instance.EmitSignal(SignalName.OnLevelComplete);
    }

    public static void EmitOnGameOver()
    {
        Instance.EmitSignal(SignalName.OnGameOver);
    }

    public static void EmitOnScoreUpdated()
    {
        Instance.EmitSignal(SignalName.OnScoreUpdated);
    }

    public static void EmitOnCreateObject(Vector2 position, int gameObjectType)
    {
        Instance.EmitSignal(SignalName.OnCreateObject, position, gameObjectType);
    }

    public static void EmitOnCreateBullet(Vector2 position, Vector2 direction, float speed, float lifeSpan, int gameObjectType)
    {
        Instance.EmitSignal(SignalName.OnCreateBullet, position, direction, speed, lifeSpan, gameObjectType);
    }
}
