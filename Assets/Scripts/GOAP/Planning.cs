using System.Collections.Generic;

public class Planning<T>
{
    T worldGoal;

    public List<ActionType<T>> ChoosePlanning(ActionType<T>[] actions, T worldSeen)
    {
        List<ActionType<T>> actionSequence = new List<ActionType<T>>();
        return actionSequence;
    }
}