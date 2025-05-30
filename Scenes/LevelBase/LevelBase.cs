using Godot;
using System;

public partial class LevelBase : Node2D
{
    public override void _Process(double delta)
    {
       if(Input.IsActionJustPressed("test"))
       {
         SignalManager.EmitOnCreateObject(
            new Vector2(200, -100),
            (int)GameObjectType.Explosion // GameObjectType
        );
       } 
    }
}
