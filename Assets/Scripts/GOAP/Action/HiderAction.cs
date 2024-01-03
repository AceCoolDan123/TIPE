public class HiderAction : ActionType<HiderWorld> 
{
    public HiderAction(HiderWorld unvalidWorld)
    : base(unvalidWorld) {}

    public override bool CanPerform(HiderWorld world) 
    {
        return true;    
    }

    public override void PerformAction() {}

    public override void ChangeWorld(ref HiderWorld world) {}
}