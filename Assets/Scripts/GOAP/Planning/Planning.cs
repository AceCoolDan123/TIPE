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
    public Stack<ActionClass> CreatePlanning(World worldSeen)
    {
        Stack<ActionClass> actionStack = new Stack<ActionClass>();
        PriorityQueue pq = new PriorityQueue(_nbActions);
        // each world after i action has been performed
        World[] worldsSeen = new World[_nbActions];
        World worldBuffer;
        bool[] isFinalAction = new bool[_nbActions];
        // to find the right action sequence
        int[] preds = new int[_nbActions];
        // to have the dists for each node
        int[] dists = new int[_nbActions];

        // Initializing relevant intels for the Dijkstra search
        for (int i = 0; i < _nbActions; i ++)
        {
            worldsSeen[i] = worldSeen;
            // there are not precedessors
            preds[i] = -1;
            // there is no distance
            dists[i] = Int32.MaxValue;
            // Search the actions that World Agent can perform at start
            if (_actionsGraph.states[i].CanPerform(worldSeen))
            {
                dists[i] = _actionsGraph.states[i].Cost(worldSeen);
                pq.EnqueueRange(i, _actionsGraph.states[i].Cost(worldSeen));
            }
            else
            {
                pq.EnqueueRange(i, Int32.MaxValue);
            }

            // at the beginning we suppose node i is not the final action
            isFinalAction[i] = false;
            
            // searching one goal that is been satisfied by the node i
            _currentGoal = 0;
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
        while (pq.Count > 0)
        {
            // dequeue a node i (an action)
            int i = pq.Dequeue();
            // the world seen by the node i is been modified by the action of the node
            _actionsGraph.states[i].ChangeWorld(ref worldsSeen[i]);
            worldBuffer = worldsSeen[i];
            if (isFinalAction[i]) 
            {  
                actionStack.Push(_actionsGraph.states[i]);
                while (preds[i] != -1)
                {
                    i = preds[i];
                    actionStack.Push(_actionsGraph.states[i]);
                }
                return actionStack;
            }

            foreach (int item in _actionsGraph.transitions[i])
            {
                //if (!pq.IsHere(item)) { continue; }
                int h = _actionsGraph.states[item].Cost(worldBuffer);
                int g = _actionsGraph.states[item].Advantage(worldBuffer);
                int f = h + g;
                if (f + dists[i] < dists[item])
                {
                    // constructing the right path
                    preds[item] = i;
                    dists[item] = h + dists[i];
                    pq.ChangePriority(item, dists[item]);
                    worldsSeen[item] = worldBuffer;
                }
            }
        }
        return actionStack;
    }
    #endregion
    
}