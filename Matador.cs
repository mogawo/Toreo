using Godot;
using System;

public class Matador : KinematicBody2D
{
    [Export]
    float movementSpeed = 200, movementSmoothing = 0.1f, dashMultiplier = 50, speedMultiplier = 1;

    [Export]
    float timeToRecover = 0.3f, dashCooldown = 2;

    [Export]
    int distanceToSpawns = 4;

    public bool canDash = true;
    bool canSpawn = false;
    public Vector2 velocity = Vector2.Zero;
    Vector2 motionInput = Vector2.Zero;
    Timer dashTimer, stunTimer;

    Vector2 impulse = Vector2.Zero;

    PackedScene afterImageScene = GD.Load<PackedScene>("res://AfterImage.tscn");

    AnimatedSprite statusEffect;

    static public float dashCD;

    public override void _Ready()
    {
        dashTimer = new Timer();
        stunTimer = new Timer();

        dashTimer.ProcessMode = Timer.TimerProcessMode.Physics;
        stunTimer.ProcessMode = Timer.TimerProcessMode.Physics;

        stunTimer.WaitTime = timeToRecover;
        dashTimer.WaitTime = dashCooldown;

        stunTimer.OneShot = true;
        dashTimer.OneShot = true;

        AddChild(dashTimer);
        AddChild(stunTimer);

        dashCD = dashCooldown;

        statusEffect = GetChild<AnimatedSprite>(0);
    }

    public override void _PhysicsProcess(float delta)
    {
        matadorMove(delta);
    }

    private void dashAfterImages(float delta)
    {
        Node lastChild = GetChild<Node>(GetChildCount() - 1);
        if(lastChild is AfterImage)
        {
            if((lastChild as Node2D).GlobalPosition.DistanceSquaredTo(GlobalPosition) < distanceToSpawns * distanceToSpawns)
            {
                canSpawn = false;
            }
        }
        // AfterImage lastSpawn = GetChild<AfterImage>(GetChildCount() - 1);
        AfterImage aft = afterImageScene.Instance<AfterImage>();
        AddChild(aft);
        aft.Transform = Transform;
        aft.SetAsToplevel(true);

    }

    private void matadorMove(float delta)
    {
        if(stunTimer.IsStopped())
        {
            motionInput = getPlayerInput();
            statusEffect.Playing = false;
            statusEffect.Visible = false;
            canDash = true;
        }
        else 
        {
            motionInput = Vector2.Zero;
        }

        if (Input.IsActionJustPressed("dash") && canDash)
        {
            speedMultiplier = dashMultiplier;
            stunTimer.WaitTime = timeToRecover;
            canDash = false;
            canSpawn = true;
            dashTimer.Start();
            stunTimer.Start();
        }
        velocity += impulse;
        velocity = velocity.LinearInterpolate(motionInput * movementSpeed * speedMultiplier, movementSmoothing);
        MoveAndSlide(velocity);
        impulse = Vector2.Zero;
        speedMultiplier = 1;
        canDash = stunTimer.IsStopped() ? dashTimer.IsStopped() : false;

        if (!stunTimer.IsStopped() && canSpawn)
        {
            dashAfterImages(delta);
        }
    }

    Vector2 getPlayerInput()
    {
        return new Vector2(Input.GetActionStrength("right") - Input.GetActionStrength("left"), Input.GetActionStrength("back") - Input.GetActionStrength("forward")).Normalized();
    }

    public void impulsePush(Vector2 direction, float power)
    {
        impulse = direction * power;
    }

    public void stun(float seconds)
    {
        stunTimer.WaitTime = seconds;
        stunTimer.Start();
        statusEffect.Visible = true;
        statusEffect.Playing = true;
    }
}
