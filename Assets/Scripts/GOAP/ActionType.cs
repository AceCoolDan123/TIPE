using System.Collections;
using System.Collections.Generic;
using System;

public abstract class ActionType<T>
{
    private delegate bool preconditions( T world );
    private Action<T> effects;
    public abstract void PerformAction();
}
