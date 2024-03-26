using System;
public class HideInFluidAction : ActionClass
{
    public HideInFluidAction(World unvalidWorld, Action<World> PerformAction, Entity entity)
    : base(unvalidWorld, PerformAction, entity) {}

    public override bool CanPerform(World world)
    {
        if (world.isInFluid == false)
        {
            return false;
        }
        return true;
    }

    public override void ChangeWorld(ref World world)
    {
        world.isHide = true;
        world.canMove = false;
    }

    public override int Cost(World world)
    {
        return 1;
    }

    public override int Advantage(World world)
    {
        // maybe calculate if the player is nearby in order to flee rather than hiding
        return 0;
    }
}