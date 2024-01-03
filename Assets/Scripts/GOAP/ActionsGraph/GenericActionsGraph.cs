using System;

public struct GenericActionsGraph<T> where T : struct
{
    public readonly ActionType<T>[] states;
    public readonly int nbStates; 
    public readonly bool[][] transitions;

    public GenericActionsGraph(ActionType<T>[] states, int nbStates, bool[][] transitions)
    {
        this.nbStates = nbStates;
        this.states = new ActionType<T>[this.nbStates];
        this.transitions = new bool[this.nbStates][];
        // Array and Matrix copies here
        for (int i = 0; i < this.nbStates; i ++)
        {
            this.states[i] = states[i];
            this.transitions[i] = new bool[this.nbStates];
            for (int j = 0; j < this.nbStates; j ++)
            {
                this.transitions[i][j] = transitions[i][j];
            }
        }
    }
}