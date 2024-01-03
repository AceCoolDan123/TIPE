using System;
using System.Collections;
using System.Collections.Generic;

public abstract class Planning<T> where T : struct
{
    public delegate bool Goal( T worldStruct );
    private int _nbGoals;
    // the highest goals' priority is the 0 index
    private readonly Goal[] _goals;
    private readonly GenericActionsGraph<T> _actionsGraph;

    public Planning(Goal[] goals, int nbGoals, ActionType<T>[] states, int nbStates)
    {
        Array.Copy(goals, _goals, nbGoals);
        _nbGoals = nbGoals;
        _actionsGraph = new GenericActionsGraph<T>(states, nbStates, GenerateTransitions(states, nbStates));
    }

    // A* search
    public Queue<ActionType<T>> ChoosePlanning(T worldSeen)
    {
        Queue<ActionType<T>> actionSequence = new Queue<ActionType<T>>();
        int[] verticles = new int[_actionsGraph.nbStates];
        int[] preds = new int[_actionsGraph.nbStates];
        
        for (int i = 0; i < _actionsGraph.nbStates; i ++)
        {
            verticles[i] = i;
            preds[i] = -1; 
        }
        
        for (int i = 0; i < _nbGoals; i ++)
        {
            // the goal has been satisfied, nothing to do
            if (_goals[i](worldSeen)) { return actionSequence; }

        }

        return actionSequence;
    }

    private bool[][] GenerateTransitions(ActionType<T>[] states, int nbStates)
    {
        bool[][] transitions = new bool[nbStates][];
        T worldBuffer;

        for (int i = 0; i < nbStates; i ++)
        {
            transitions[i] = new bool[nbStates];

            for (int j = 0; j < nbStates; j ++)
            {
                // an action can't have a transition to itself
                if (i == j) 
                { 
                    transitions[i][j] = false;
                    continue; 
                }
                worldBuffer = states[j].UnvalidWorld;
                //if (worldBuffer)
                //states.[i].Result(worldBuffer)
                //if (states[i].Result)
            }
        }

        return transitions;
    }
}