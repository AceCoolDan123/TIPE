public class HiderAction : ActionType<HiderWorld> 
{
    public override bool CanPerform(HiderWorld world) 
    {
        return true;    
    }

    public override void PerformAction() {}

    public override void Result(HiderWorld world) {}
}