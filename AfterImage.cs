using Godot;
using System;

public class AfterImage : Node2D
{
    static public int colorIndex = 0;

    [Export]
    Color[] pallete = new Color[]{
                                    Color.ColorN("red"),
                                    Color.ColorN("orange"),
                                    Color.ColorN("yellow"),
                                    Color.ColorN("green"),
                                    Color.ColorN("blue"),
                                    Color.ColorN("indigo"),
                                    Color.ColorN("violet")
                                };

    [Export]
    Texture texture;

    [Export]
    float lifetime = 1;

    static public int allowedExisting = 7;

    static public int currExisting = 0;

    Sprite sprite;
    Timer timer;

    public override void _Ready()
    {
        timer = GetNode<Timer>("Timer");
        timer.WaitTime = lifetime;
        timer.Start();
        currExisting++;
        if (colorIndex >= pallete.Length)
        {
            colorIndex = 0;
        }

        sprite = GetNode<Sprite>("Sprite");
        sprite.Texture = texture;
        timer = GetNode<Timer>("Timer");

        ShaderMaterial spriteMat = (sprite.Material as ShaderMaterial);

        Color overlay = pallete[colorIndex];

        overlay.a = 0;
        spriteMat.SetShaderParam("color_overlay", overlay);
        ++colorIndex;

        sprite.Material = spriteMat;
    }

    public override void _Process(float delta)
    {
        if(timer.IsStopped())
        {
            QueueFree();
        }
    }
}
