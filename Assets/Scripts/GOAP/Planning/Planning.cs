using System;
using System.Collections;
using System.Collections.Generic;

public abstract class Planning<T> where T : struct
{
    public delegate bool Goal( T worldStruct );
    private int _nbGoals;
    // the highest goals' priority is the 0 index
    private readonly Goal[] _goals;

    public Planning(Goal[] goals, int nbGoals)
    {
        Array.Copy(goals, _goals, nbGoals);
        _nbGoals = nbGoals;
    }

    public Queue<ActionType<T>> ChoosePlanning(ActionType<T>[] actions, int nbActions, T worldSeen)
    {
        Queue<ActionType<T>> actionSequence = new Queue<ActionType<T>>();
        
        for (int i = 0; i < _nbGoals; i ++)
        {
            // the goal has been satisfied, nothing to do
            if (_goals[i](worldSeen)) { return actionSequence; }

            for (int k = 0; k < nbActions; k ++)
            {

            } 
        }

        return actionSequence;
    }
}