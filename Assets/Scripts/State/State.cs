public abstract class State<T> 
{      
    protected T id;
    public T ID { get {return id;} } 
    public abstract void OnEnter();
    public abstract void OnUpdate();
    public abstract void OnLateUpdate();
    public abstract void OnExit();   
    protected abstract void CheckTransitions();
}
