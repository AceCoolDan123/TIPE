public abstract class HState<T> : State<T> 
{
    protected T currentChildState;
    protected T currentParentState;

    public T CurrentChildState { get { return currentChildState; } }
    public T CurrentParentState { get { return currentParentState; } }

    protected abstract void FindChildState();
}