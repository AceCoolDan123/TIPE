using System.Collections;
using System.Collections.Generic;

// T : Enum States type
public class GenericFSM<T>
{  
    private State<T> _currentState;
    private Dictionary<T, State<T>> _dict = new Dictionary<T, State<T>>();

    public void AddState(State<T> state)
    {
        if(_dict.ContainsKey(state.ID))
        {
            return;
        }

        _dict.Add(state.ID, state);
    }

    public void SetCurrentState(T stateID)
    {
        if (!_dict.ContainsKey(stateID))
        {
            return;
        }

        State<T> state = _dict[stateID];
        
        if(state == null)
        {
            return;
        }
        
        _currentState = state; 
    }

    public void OnUpdate()
    {
        _currentState.OnUpdate();
    }

    public void OnLateUpdate()
    {
        _currentState.OnLateUpdate();
    }
}
