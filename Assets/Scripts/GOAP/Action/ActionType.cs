using System.Collections;
using System.Collections.Generic;
using System;

public abstract class ActionType<T> where T : struct
{
    // the preconditions of the action : true = the action can be performed // false = can't do the action
    public abstract bool CanPerform(T world);
    // perform the action on the real game, the preconditions are supposed to be satisfied
    public abstract void PerformAction();
    // modify its consequences on the world
    public abstract void Result(T world);
}
