public abstract class GoapState : State<GoapStateEnum>
{
    protected Entity entity;
    public GoapState(GoapStateEnum id, Entity entity)
    {
        this.id = id; 
        this.entity = entity;
    }

    public override void OnEnter() {}
    public override void OnUpdate() {}
    public override void OnLateUpdate() {}
    public override void OnExit() {}
    protected override void CheckTransitions() {}
}