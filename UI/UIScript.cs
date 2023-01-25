using Godot;
using System;

public class UIScript : CanvasLayer
{
    Timer dashCoolddown;
    Matador matador;
    TextureButton shiftKey;
    bool mouseClickShift = false;
    public override void _Ready()
    {
        shiftKey = GetNode<TextureButton>("ShiftKeyUI");
        dashCoolddown = GetNode<Timer>("ShiftKeyUI/DashCooldown");
        matador = GetParent<Matador>();
    }

    public override void _Process(float delta)
    {
        ShaderMaterial textureShade = (shiftKey.Material as ShaderMaterial);
        if(Input.IsActionJustPressed("dash") && matador.canDash)
        {
            dashCoolddown.WaitTime = Matador.dashCD;
            shiftKey.Pressed = true;
            matador.canDash = false;
            mouseClickShift = false;
            dashCoolddown.Start();
            textureShade.SetShaderParam("alpha",  0.5f);
        }

        if(dashCoolddown.IsStopped() && !matador.canDash)
        {
            shiftKey.Pressed = false;
            matador.canDash = true;
            textureShade.SetShaderParam("alpha",  1);
        }

        shiftKey.Material = textureShade;
    }
}
