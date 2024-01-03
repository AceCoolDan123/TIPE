using System.Collections;
using System.Collections.Generic;
using System;

public abstract class ActionType<T> where T : struct
{
    // this is relevant for the transitions' construction of the action graph
    private readonly T _unvalidWorld;
    public T UnvalidWorld { get { return _unvalidWorld; } }

    public ActionType(T unvalidWorld)
    {
        _unvalidWorld = unvalidWorld;
    }

    // the preconditions of the action : true = the action can be performed // false = can't do the action
    public abstract bool CanPerform(T world);
    // perform the action on the real game, the preconditions are supposed to be satisfied
    public abstract void PerformAction();
    // modify its consequences on the world
    public abstract void Result(T world);
}
