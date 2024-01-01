public class HiderPlanning : Planning<HiderWorld> 
{
    public HiderPlanning(Goal[] goals, int nbGoals, ActionType<HiderWorld>[] states, int nbStates, bool[][] transitions) 
    : base(goals, nbGoals, states, nbStates, transitions) {}
}