using System;
using System.Collections;
using System.Collections.Generic;

public abstract class Planning<T> where T : struct
{
    public delegate bool Goal( T worldStruct );
    private int _nbGoals;
    // the highest goals' priority is the 0 index
    private readonly Goal[] _goals;
    // it is the index in reality of _goals
    private int _currentGoal;
    private readonly GenericActionsGraph<T> _actionsGraph;
    private int _nbActions { get { return _actionsGraph.nbStates; } }


    public Planning(Goal[] goals, int nbGoals, ActionType<T>[] states, int nbStates)
    {
        Array.Copy(goals, _goals, nbGoals);
        _nbGoals = nbGoals;
        _actionsGraph = new GenericActionsGraph<T>(states, nbStates, GenerateTransitions(states, nbStates));
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
                // the action can't be performed anyway
                if (!states[i].CanPerform(worldBuffer)) { continue; }
                states[i].ChangeWorld(ref worldBuffer);
                if (states[j].CanPerform(worldBuffer))
                {
                    transitions[i][j] = true;
                }
                else
                {
                    transitions[i][j] = false;
                }
            }
        }

        return transitions;
    }
    
    #region A* search
    public Queue<ActionType<T>> ChoosePlanning(T worldSeen)
    {
        // affecting the search for the most important goal at first
        _currentGoal = 0;

        Queue<ActionType<T>> actionSequence = new Queue<ActionType<T>>();
        PriorityQueue<int> dists = new PriorityQueue<int>(_nbActions);
        bool[] isFinalAction = new bool[_nbActions];
        int[] preds = new int[_nbActions];

        for (int i = 0; i < _nbActions; i ++)
        {
            preds[i] = -1;
            if (_actionsGraph.states[i].CanPerform(worldSeen))
            {
                dists.EnqueueRange(i, 0);
            }
            else
            {
                dists.EnqueueRange(i, Int32.MaxValue);
            }

            T worldBuffer;
            while (_currentGoal < _nbGoals)
            {
                worldBuffer = worldSeen;
                _actionsGraph.states[i].ChangeWorld(ref worldBuffer);
                if(_goals[_currentGoal](worldBuffer))
                {

                }
            }
        }

        return actionSequence;
    }
    #endregion
    
}