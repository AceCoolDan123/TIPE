using System;
using UnityEngine;

public class GotoFluidAction : ActionClass
{
    public GotoFluidAction(World unvalidWorld, Action<World> performAction, Entity entity)
    : base (unvalidWorld, performAction, entity) {}

    public override bool CanPerform(World world)
    {
        if (world.canMove == false)
        {
            return false;
        }
        return true;
    }

    public override void ChangeWorld(ref World world)
    {
        world.isInFluid = true;
    }

    public override int Cost(World world)
    {
        return 10;
    }

    public override int Advantage(World world)
    {
        //the player is not seen but don't care
        if (world.player.isNull)
        {
            return 0; // the best advantage possible
        }
        if (world.nearestFluidPos == null)
        {
            // there's no fluid to go !
            return 100; 
        }
        if (Tools.IsLookingAtMe(world.player.position, world.player.look, entity.transform.position))
        {
            return 100;
        }
        
        return 0;
    }
}