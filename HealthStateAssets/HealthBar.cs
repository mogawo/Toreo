using Godot;
using System;

public class HealthBar : VBoxContainer
{
    [Export]
    int maxHealth = 3;

    int currentOrb = 0;
    PackedScene healthNode = GD.Load<PackedScene>("res://HealthStateAssets/HealthOrb.tscn");

    bool alive = true; //Placeholder

    public override void _Ready()
    {
        if(GetChildCount() > maxHealth)
        {
            for (int i = maxHealth; i < GetChildCount(); i++)
            {
                GetChild(i).QueueFree();
                GD.Print("More Health Orbs than allowed! HealthBar.cs:", 
                    new System.Diagnostics.StackFrame(0, true).GetFileLineNumber());
            }
        }
        else if(GetChildCount() < maxHealth)
        {
            for (int i = GetChildCount(); i < maxHealth; i++)
            {
                HealthOrb emptyOrb = healthNode.Instance<HealthOrb>();
                emptyOrb.ChangeState(HealthOrb.EMPTY);
                AddChild(emptyOrb);
            }
        }
    }

    public override void _Process(float delta)
    {
        if(alive && Input.IsActionJustPressed("dash"))
        {
            HealthOrb orb = (HealthOrb)GetChild(currentOrb);
            
            if(orb.isEmpty())
            {
                orb = (HealthOrb)GetChild(++currentOrb);
            }
            orb.tickDamage();
            
            if(orb.isEmpty() && currentOrb == GetChildCount()-1)
            {
                GD.Print("Your Dead! blargh X_X");
                alive = false;
                return;
            }
            
        }
    }
}
