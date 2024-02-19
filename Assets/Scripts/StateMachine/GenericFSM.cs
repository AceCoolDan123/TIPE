using System.Collections;
using System.Collections.Generic;

public class GenericFSM<T>
{
    private State<T> _currentState;
    Dictionary<T, State<T>> _dict = new Dictionary<T, State<T>>(); 
    
    public void AddState(State<T> state)
    {
        // A state has a unique id
        if (_dict.ContainsKey(state.ID) || state == null) { return; }

        _dict.Add(state.ID, state);
    }

    public void SetState(T id)
    {
        if (!_dict.ContainsKey(id)) { return; }
        
        State<T> state = _dict[id];
        
        if (state == null) { return; }
        if (_currentState != null) 
        {
            _currentState.OnExit();
        }
        _currentState = state;
        _currentState.OnEnter();
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