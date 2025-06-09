using Godot;
using System;
using System.Collections.Generic;

public partial class ScrollingBg : Node2D
{
    private Dictionary<int, List<Texture2D>> BGFiles;

    [Export(PropertyHint.Range, "1,4")] private int _bgNumber = 1;
    [Export] private float _mirrorX = 1920.0f;
    [Export] private float _yOffset = 0;

    private void InitBgs()
    {
        BGFiles = new Dictionary<int, List<Texture2D>>
        {
            {1, new List<Texture2D>
                {
                    GD.Load<Texture2D>("res://assets/backgrounds/game_background_1/layers/sky.png"),
                    GD.Load<Texture2D>("res://assets/backgrounds/game_background_1/layers/clouds_1.png"),
                    GD.Load<Texture2D>("res://assets/backgrounds/game_background_1/layers/clouds_2.png"),
                    GD.Load<Texture2D>("res://assets/backgrounds/game_background_1/layers/clouds_3.png"),
                    GD.Load<Texture2D>("res://assets/backgrounds/game_background_1/layers/clouds_4.png"),
                    GD.Load<Texture2D>("res://assets/backgrounds/game_background_1/layers/rocks_1.png"),
                    GD.Load<Texture2D>("res://assets/backgrounds/game_background_1/layers/rocks_2.png")
                }
            },
            {2, new List<Texture2D>
                {
                    GD.Load<Texture2D>("res://assets/backgrounds/game_background_2/layers/sky.png"),
                    GD.Load<Texture2D>("res://assets/backgrounds/game_background_2/layers/birds.png"),
                    GD.Load<Texture2D>("res://assets/backgrounds/game_background_2/layers/clouds_1.png"),
                    GD.Load<Texture2D>("res://assets/backgrounds/game_background_2/layers/clouds_2.png"),
                    GD.Load<Texture2D>("res://assets/backgrounds/game_background_2/layers/clouds_3.png"),
                    GD.Load<Texture2D>("res://assets/backgrounds/game_background_2/layers/pines.png"),
                    GD.Load<Texture2D>("res://assets/backgrounds/game_background_2/layers/rocks_1.png"),
                    GD.Load<Texture2D>("res://assets/backgrounds/game_background_2/layers/rocks_2.png"),
                    GD.Load<Texture2D>("res://assets/backgrounds/game_background_2/layers/rocks_3.png")
                }
            },
            {3, new List<Texture2D>
                {
                    GD.Load<Texture2D>("res://assets/backgrounds/game_background_3/layers/sky.png"),
                    GD.Load<Texture2D>("res://assets/backgrounds/game_background_3/layers/clouds_1.png"),
                    GD.Load<Texture2D>("res://assets/backgrounds/game_background_3/layers/clouds_2.png"),
                    GD.Load<Texture2D>("res://assets/backgrounds/game_background_3/layers/ground_1.png"),
                    GD.Load<Texture2D>("res://assets/backgrounds/game_background_3/layers/ground_2.png"),
                    GD.Load<Texture2D>("res://assets/backgrounds/game_background_3/layers/ground_3.png"),
                    GD.Load<Texture2D>("res://assets/backgrounds/game_background_3/layers/plant.png"),
                    GD.Load<Texture2D>("res://assets/backgrounds/game_background_3/layers/rocks.png")
                }
            },
            {4, new List<Texture2D>
                {
                    GD.Load<Texture2D>("res://assets/backgrounds/game_background_4/layers/sky.png"),
                    GD.Load<Texture2D>("res://assets/backgrounds/game_background_4/layers/clouds_1.png"),
                    GD.Load<Texture2D>("res://assets/backgrounds/game_background_4/layers/clouds_2.png"),
                    GD.Load<Texture2D>("res://assets/backgrounds/game_background_4/layers/ground.png"),
                    GD.Load<Texture2D>("res://assets/backgrounds/game_background_4/layers/rocks.png")
                }
            }
        };
    }

    public override void _Ready()
    {
        InitBgs();
        AddBackgrounds();
        Position = new Vector2(0, _yOffset);
    }
    
    private float GetIncrement()
    {
        return 1.0f / BGFiles[_bgNumber].Count;
    }

    private Sprite2D GetSprite(Texture2D t)
    {
        var sprite = new Sprite2D();
        sprite.Texture = t;
        sprite.Centered = false;
        return sprite;
    }

    private void AddLayer(Texture2D t, float timeOffset)
    {
        var sprite = GetSprite(t);

        var parLa = new Parallax2D();
        parLa.ZIndex = -10;
        parLa.RepeatSize = new Vector2(_mirrorX, 0);
        parLa.ScrollScale = new Vector2(timeOffset, 1);
        parLa.AddChild(sprite);

        AddChild(parLa);
    }

    private void AddBackgrounds()
    {
        var inc = GetIncrement();
        var timeOffset = inc;
        var filesList = BGFiles[_bgNumber];

        for (int index = 0; index < filesList.Count; index++)
        {
            var bgFile = filesList[index];
            if (index == 0)
            {
                AddLayer(bgFile, 0);
            }
            else
            {
                AddLayer(bgFile, timeOffset);
                timeOffset += inc;
            }
        }
    }
}
