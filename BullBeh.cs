using Godot;
using System;

public class BullBeh : KinematicBody2D
{
    [Export]
    float pushForce = 10, matadorStunTime = 2;

    float pushScale = 100;
    public override void _Ready()
    {
        
    }

    public void _on_Hitbox_area_entered(Node body)
    {
        Node parent = body.GetParent();
        if(parent is Matador)
        {
            Matador matador = parent as Matador;
            Vector2 pushDir = GlobalTransform.origin.DirectionTo(matador.GlobalTransform.origin);
            matador.impulsePush(pushDir, matador.velocity.Length() + pushForce * pushScale);
            matador.stun(matadorStunTime);
        }
    }
}
