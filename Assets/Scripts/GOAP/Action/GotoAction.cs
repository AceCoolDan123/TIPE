using System;
using UnityEngine;

public class GotoFluidAction : ActionClass
{
    private CharacterController _controller;
    public GotoFluidAction(World unvalidWorld, Action performAction, Entity entity)
    : base (unvalidWorld, performAction) 
    {
        _controller = entity.GetComponent<CharacterController>();
    }

    public override bool CanPerform(World world)
    {
        return world.canMove;
    }

    public override void ChangeWorld(ref World world)
    {
        world.isInFluid = true;
    }

    public override int Cost(World world)
    {
        return 10;
    }
}