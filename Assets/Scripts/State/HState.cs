public abstract class HState<T> : State<T> 
{
    protected T currentChildStateID;
    protected T currentParentStateID;

    public T CurrentChildStateID { get { return currentChildStateID; } set { currentChildStateID = value; } }
    public T CurrentParentStateID { get { return currentParentStateID; } set { currentParentStateID = value; } }

    public abstract void FindChildState();
}