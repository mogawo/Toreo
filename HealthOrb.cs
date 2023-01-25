using Godot;
using System;

public class HealthOrb : Node2D
{
    Sprite[] orbSprites = new Sprite[4];
    int state;
    const int full = 0, half = 1, empty = 2;
    public override void _Ready()
    {
        Godot.Collections.Array gArray = GetChildren();
        gArray.CopyTo(orbSprites, 0);

        orbSprites[0].Visible = true;
        orbSprites[3].Visible = false;
    }

    public override void _Process(float delta)
    {
        switch(state)
        {
            case empty:
                orbSprites[1].Visible = false;
                orbSprites[2].Visible = false;
                break;
            case half:
                orbSprites[1].Visible = true;
                orbSprites[2].Visible = false;
                break;
            case full:
                orbSprites[1].Visible = false;
                orbSprites[2].Visible = true;
                break;
            default:
                break;
        }
    }

    public void crack(bool toCrack = true)
    {
        orbSprites[3].Visible = toCrack;
    }

    public void tickDamage()
    {
        switch(state)
        {
            case full:
                state = half;
                break;
            case half:
                state = empty;
                break;
            case empty:
                crack();
                break;
        }
    }

    public void tickHeal()
    {
        switch(state)
        {
            case half:
                state = full;
                break;
            case empty:
                state = half;
                break;
        }
    }
}
