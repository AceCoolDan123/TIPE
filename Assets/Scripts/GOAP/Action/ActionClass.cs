using System.Collections;
using System;

public abstract class ActionClass
{
    // this is relevant for the transitions' construction of the action graph
    private readonly World _unvalidWorld;
    public World UnvalidWorld { get { return _unvalidWorld; } }
    // while performing an action, if the function return true, the action has succeeded, on the contrary, it has failed
    public delegate bool ActionFunc();
    // perform the action on the real game, the preconditions are supposed to be satisfied
    public readonly ActionFunc PerformAction;

    public ActionClass(World unvalidWorld, ActionFunc performAction)
    {
        _unvalidWorld = unvalidWorld;
        PerformAction = performAction; 
    }

    // the preconditions of the action : true = the action can be performed // false = can't do the action
    public abstract bool CanPerform(World world);
    // modify its consequences on the world
    public abstract void ChangeWorld(ref World world);
    // Heuristic estimation
    public abstract int Cost(World world);

}
