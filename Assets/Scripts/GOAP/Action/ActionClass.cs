using System.Collections;
using System;

public abstract class ActionClass
{
    protected Entity entity;
    // this is relevant for the transitions' construction of the action graph
    private readonly World _unvalidWorld;
    public World UnvalidWorld { get { return _unvalidWorld; } }
    // perform the action in the real world, the preconditions are supposed to be satisfied
    public Action<World> PerformAction;

    public ActionClass(World unvalidWorld, Action<World> performAction, Entity entity)
    {
        _unvalidWorld = unvalidWorld;
        PerformAction = performAction; 
        this.entity = entity;
    }

    // the preconditions of the action : true = the action can be performed // false = can't do the action
    public abstract bool CanPerform(World world);
    // modify its consequences on the world
    public abstract void ChangeWorld(ref World world);
    // Heuristic estimation of the action's cost
    public abstract int Cost(World world);
    // Heuristic estimation of the action's advantage
    public abstract int Advantage(World world);

}
