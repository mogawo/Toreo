using Godot;
using System;

public class HealthOrb : Control
{
    Sprite[] orbSprites = new Sprite[4];
    int state;
    public const int FULL = 0, HALF = 1, EMPTY = 2;
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
            case EMPTY:
                orbSprites[1].Visible = false;
                orbSprites[2].Visible = false;
                break;
            case HALF:
                orbSprites[1].Visible = true;
                orbSprites[2].Visible = false;
                break;
            case FULL:
                orbSprites[1].Visible = false;
                orbSprites[2].Visible = true;
                break;
            default:
                break;
        }
    }

    public void ChangeState(int newState)
    {
        state = newState;
    }

    public void crack(bool toCrack = true)
    {
        orbSprites[3].Visible = toCrack;
    }

    public void tickDamage()
    {
        switch(state)
        {
            case FULL:
                state = HALF;
                break;
            case HALF:
                state = EMPTY;
                break;
            case EMPTY:
                crack();
                break;
        }
    }

    public void tickHeal()
    {
        switch(state)
        {
            case HALF:
                state = FULL;
                break;
            case EMPTY:
                state = HALF;
                break;
        }
    }

    public bool isEmpty()
    {
        return state == EMPTY;
    }
}
