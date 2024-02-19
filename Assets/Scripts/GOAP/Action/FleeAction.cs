using System;
public class FleeAction : ActionClass
{
    public FleeAction(World unvalidWorld, Action<World> PerformAction)
    : base(unvalidWorld, PerformAction) {}

    public override bool CanPerform(World worldSeen)
    {
        return worldSeen.canMove;
    }

    public override void ChangeWorld(ref World world)
    {
        // nonesense
    }

    public override int Cost(World worldSeen)
    {
        return 20;
    }
}