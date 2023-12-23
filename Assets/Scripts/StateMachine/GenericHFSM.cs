using System.Collections;
using System.Collections.Generic;

// T : Enum States type
public class GenericHFSM<T>
{  
    private State<T> _rootState;
    private Dictionary<T, State<T>> _dict = new Dictionary<T, State<T>>();

    public void AddState(State<T> state)
    {
        if(_dict.ContainsKey(state.ID))
        {
            return;
        }

        _dict.Add(state.ID, state);
    }

    public void SetRootState(T stateID)
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
        
        _rootState = state; 
    }

    public void OnUpdate()
    {
        _rootState.OnUpdate();
        /*State<T> state = _rootState;
        while (state.CurrentChildState != NULL)
        {
            _dict_rootState.CurrentChildState
        }*/
    }

    public void OnLateUpdate()
    {
        _rootState.OnLateUpdate();
    }
}
