using System;
using System.Collections.Generic;

public struct ActionsGraph
{
    public readonly ActionClass[] states;
    public readonly int nbStates; 
    public readonly List<int>[] transitions;

    public ActionsGraph(ActionClass[] states, int nbStates, List<int>[] transitions)
    {
        this.nbStates = nbStates;
        this.states = new ActionClass[this.nbStates];
        this.transitions = new List<int>[this.nbStates];
        // Array and Matrix copies here
        for (int i = 0; i < this.nbStates; i ++)
        {
            this.states[i] = states[i];
            this.transitions[i] = new List<int>();
            // copying the list
            foreach(int item in transitions[i])
            {
                this.transitions[i].Add(item);
            }
        }
    }
}