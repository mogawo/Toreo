using Godot;
using System;

public class ShiftKeyUI : TextureButton
{
    Timer dashCoolddown;
    bool canDash = true;
    public override void _Ready()
    {
        dashCoolddown = GetNode<Timer>("DashCooldown");
        
    }

    public override void _Process(float delta)
    {
        if(Input.IsActionJustPressed("dash") && canDash)
        {
            dashCoolddown.WaitTime = MatadorMovement.dashCD;
            Pressed = true;
            canDash = false;
            dashCoolddown.Start();
        }

        if(dashCoolddown.IsStopped() && !canDash)
        {
            Pressed = false;
            canDash = true;
        }
    }
}
