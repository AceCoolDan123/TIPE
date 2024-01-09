using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planning
{
    public delegate bool Goal( World worldStruct );
    private int _nbGoals;
    // the highest goals' priority is the 0 index
    private readonly Goal[] _goals;
    // it is the index in reality of _goals
    private int _currentGoal;
    private readonly ActionsGraph _actionsGraph;
    private int _nbActions { get { return _actionsGraph.nbStates; } }


    public Planning(Goal[] goals, int nbGoals, ActionClass[] states, int nbStates)
    {
        _nbGoals = nbGoals;
        _goals = new Goal[_nbGoals];
        for (int i = 0; i < _nbGoals; i ++)
        {
            _goals[i] = goals[i];
        }
        _actionsGraph = new ActionsGraph(states, nbStates, GenerateTransitions(states, nbStates));
    }

    private List<int>[] GenerateTransitions(ActionClass[] states, int nbStates)
    {
        List<int>[] transitions = new List<int>[nbStates];
        World worldBuffer;

        for (int i = 0; i < nbStates; i ++)
        {
            transitions[i] = new List<int>();

            for (int j = 0; j < nbStates; j ++)
            {
                // an action can't have a transition to itself
                if (i == j) { continue; }
                worldBuffer = states[j].UnvalidWorld;
                // the action can't be performed anyway
                if (!states[i].CanPerform(worldBuffer)) { continue; }
                states[i].ChangeWorld(ref worldBuffer);
                if (states[j].CanPerform(worldBuffer))
                {
                    transitions[i].Add(j);
                }
            }
        }

        return transitions;
    }
    
    #region A* search
    public Queue<ActionClass> CreatePlanning(World worldSeen)
    {
        Queue<ActionClass> actionSequence = new Queue<ActionClass>();
        PriorityQueue dists = new PriorityQueue(_nbActions);
        // each world after i action has been performed
        World[] worldsSeen = new World[_nbActions];
        World worldBuffer;
        bool[] isFinalAction = new bool[_nbActions];
        int[] preds = new int[_nbActions];

        // Initializing relevant intels for the A* search
        for (int i = 0; i < _nbActions; i ++)
        {
            worldsSeen[i] = worldSeen;
            preds[i] = -1;
            // Worldhe Agent can perform these actions first
            if (_actionsGraph.states[i].CanPerform(worldSeen))
            {
                dists.EnqueueRange(i, _actionsGraph.states[i].Cost(worldSeen));
                Debug.Log(_actionsGraph.states[i]);
            }
            else
            {
                dists.EnqueueRange(i, Int32.MaxValue);
            }

            // affecting the search for the most important goal at first
            _currentGoal = 0;
            isFinalAction[i] = false;

            while (_currentGoal < _nbGoals)
            {
                worldBuffer = worldSeen;
                _actionsGraph.states[i].ChangeWorld(ref worldBuffer);
                if(_goals[_currentGoal](worldBuffer))
                {
                    isFinalAction[i] = true;
                }
                _currentGoal ++;
            }
        }

        // the crux of the A* search
        while (dists.Count > 0)
        {
            int actionIndex = dists.Dequeue();

            _actionsGraph.states[actionIndex].ChangeWorld(ref worldsSeen[actionIndex]);
            worldBuffer = worldsSeen[actionIndex];
            if (isFinalAction[actionIndex]) 
            {  
                int i = actionIndex;
                actionSequence.Enqueue(_actionsGraph.states[i]);
                while (preds[i] != -1)
                {
                    i = preds[i];
                    actionSequence.Enqueue(_actionsGraph.states[i]);
                }
                break;
            }

            foreach (int item in _actionsGraph.transitions[actionIndex])
            {
                if (!dists.IsHere(item)) { continue; }
                int h = _actionsGraph.states[item].Cost(worldBuffer);
                if (h < dists.GetPriority(item))
                {
                    dists.ChangePriority(item, h);
                    worldsSeen[item] = worldBuffer;
                }
            }
        }
        return actionSequence;
    }
    #endregion
    
}