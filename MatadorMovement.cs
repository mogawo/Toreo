using Godot;
using System;

public class MatadorMovement : KinematicBody2D
{
    [Export]
    float movementSpeed, accel, dashMultiplier, speedMultiplier = 1;

    [Export]
    float timeToRecover, dashCooldown;

    [Export]
    int aliveAfterImage;

    bool canMove = true, canDash = true;
    Vector2 velocity = Vector2.Zero;
    Vector2 motionInput = Vector2.Zero;
    Timer recoveryTimer, dashTimer;

    PackedScene afterImageScene = GD.Load<PackedScene>("res://AfterImage.tscn");

    //Used for the UI to get accurate times
    static public float dashCD; 

    public override void _Ready()
    {
        recoveryTimer = new Timer();
        dashTimer = new Timer();
        recoveryTimer.ProcessMode = Timer.TimerProcessMode.Physics;
        dashTimer.ProcessMode = Timer.TimerProcessMode.Physics;

        recoveryTimer.WaitTime = timeToRecover;
        dashTimer.WaitTime = dashCooldown;

        recoveryTimer.OneShot = true;
        dashTimer.OneShot = true;

        AddChild(recoveryTimer);
        AddChild(dashTimer);

        dashCD = dashCooldown;
    }

    public override void _PhysicsProcess(float delta)
    {
        matadorMove(delta);
    }

    private void dashAfterImages(float delta)
    {
        AfterImage aft = afterImageScene.Instance<AfterImage>();
        AddChild(aft);
        aft.Transform = Transform;
        aft.SetAsToplevel(true);
        
    }

    private void matadorMove(float delta)
    {
        motionInput = canMove ? getPlayerInput() : Vector2.Zero;
        if(Input.IsActionJustPressed("dash") && canDash)
        {
            speedMultiplier = dashMultiplier;
            canDash = false;
            dashTimer.Start();
            recoveryTimer.Start();
            AfterImage.colorIndex = 0;
            AfterImage.currExisting = 0;
        }

        velocity = velocity.LinearInterpolate(motionInput * movementSpeed * speedMultiplier, accel * delta);
        MoveAndSlide(velocity);

        speedMultiplier = 1;
        canDash = dashTimer.IsStopped();
        canMove = recoveryTimer.IsStopped();
        
        if(!canMove && !AfterImage.isMaxedExisitng())
        {
            dashAfterImages(delta);
        }
    }

    Vector2 getPlayerInput()
    {
        return new Vector2(Input.GetActionStrength("right") - Input.GetActionStrength("left"), Input.GetActionStrength("back")-Input.GetActionStrength("forward")).Normalized();
    }
}
