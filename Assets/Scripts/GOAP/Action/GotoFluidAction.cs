using System;
using UnityEngine;

public class GotoFluidAction : ActionClass
{
    public GotoFluidAction(World unvalidWorld, Action<World> performAction)
    : base (unvalidWorld, performAction) {}

    public override bool CanPerform(World world)
    {
        return world.canMove;
    }

    public override void ChangeWorld(ref World world)
    {
        world.isInFluid = true;
        world.isHide = true; // just for debug
    }

    public override int Cost(World world)
    {
        return 10;
    }
}