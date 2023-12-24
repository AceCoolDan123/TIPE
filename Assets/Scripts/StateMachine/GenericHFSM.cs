using System.Collections;
using System.Collections.Generic;

// T : Enum States type
public class GenericHFSM<T>
{  
    private HState<T> _rootState;
    private Dictionary<T, HState<T>> _dict = new Dictionary<T, HState<T>>();

    public void AddState(HState<T> state)
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

        HState<T> state = _dict[stateID];

        if (state == null)
        {
            return;
        }
        _rootState = state; 
        _rootState.FindChildState();
        _rootState.OnEnter();
    }

    public void OnUpdate()
    {
        UpdateAllParentedStates();
    }

    public void OnLateUpdate()
    {
        LateUpdateAllParentedStates();
    }

    public void SetParentChildRelation(T id_parent, T id_child)
    {
        if (!_dict.ContainsKey(id_child) || !_dict.ContainsKey(id_parent)) { return; }
        _dict[id_parent].CurrentChildStateID = id_child;
        _dict[id_child].CurrentParentStateID = id_parent;
        _dict[id_child].FindChildState();
    }

    public void ChangeState(T oldID, T newID)
    {
        if (!_dict.ContainsKey(oldID) || !_dict.ContainsKey(newID)) { return; }
        ExitStateAndChildren(oldID);
        SetParentChildRelation(_dict[oldID].CurrentParentStateID, newID);
        EnterStateAndChildren(newID);
    }

    #region Parent-Child Path Functions
    private void UpdateAllParentedStates()
    {
        _rootState.OnUpdate();
        T tmpID = _rootState.ID;
        while (_dict.ContainsKey(_dict[tmpID].CurrentChildStateID))
        {
            _dict[_dict[tmpID].CurrentChildStateID].OnUpdate();
            tmpID = _dict[tmpID].CurrentChildStateID;
        }
    }

    private void LateUpdateAllParentedStates()
    {
        _rootState.OnLateUpdate();
        T tmpID = _rootState.ID;
        while (_dict.ContainsKey(_dict[tmpID].CurrentChildStateID))
        {
            _dict[_dict[tmpID].CurrentChildStateID].OnLateUpdate();
            tmpID = _dict[tmpID].CurrentChildStateID;
        }
    }

    private void ExitStateAndChildren(T id)
    {
        if (!_dict.ContainsKey(id)) { return; }
        _dict[id].OnExit();
        T tmpID = id;
        while (_dict.ContainsKey(_dict[tmpID].CurrentChildStateID))
        {
            _dict[_dict[tmpID].CurrentChildStateID].OnExit();
            tmpID = _dict[tmpID].CurrentChildStateID;
        }
    }

    private void EnterStateAndChildren(T id)
    {
        if (!_dict.ContainsKey(id)) { return; }
        _dict[id].OnEnter();
        T tmpID = id;
        while (_dict.ContainsKey(_dict[tmpID].CurrentChildStateID))
        {
            _dict[_dict[tmpID].CurrentChildStateID].OnEnter();
            tmpID = _dict[tmpID].CurrentChildStateID;
        }
    }
    #endregion
}
