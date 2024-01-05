using UnityEngine;
using System;

[RequireComponent(typeof(CharacterController))]
public class Hider : Entity
{
    #region Variables
    [SerializeField]
    private Collider _fluid;
    private Planning.Goal[] _goals;
    private int _nbGoals;
    private ActionClass[] _actions;
    private int _nbActions;
    private Action _goToFluidAction;
    private Planning _planning;
    #endregion

    #region Unity Methods
    private void Awake() 
    {
        controller = GetComponent<CharacterController>();    
    }
    private void Start() 
    {    
        InitGoals();
        InitActions();
        _planning = new Planning(_goals, _nbGoals, _actions, _nbActions);
    }
    private void Update() 
    {
        SenseAround();    
    }
    #endregion

    #region Other Methods
    protected override void SenseAround()
    {
        Vector3 pos = _fluid.ClosestPoint(transform.position);
    } 

    private void GotoNearestFluid()
    {
        Vector3 dest = worldSeen.nearestFluidPos;
        controller.Move(dest);
    }

    private bool HideGoal(World world)
    {
        return world.isHide;
    }

    private void InitGoals()
    {
        _nbGoals = 1;
        _goals = new Planning.Goal[_nbGoals];
        _goals[0] = HideGoal; 
    }

    private void InitActions()
    {
        _nbActions = 1;
        _actions = new ActionClass[1];
        _actions[0] = new GotoFluidAction(defaultWorld(), _goToFluidAction, this); 
    }

    private World defaultWorld()
    {
        World world;
        world.playerPos = Vector3.zero;
        world.nearestFluidPos = Vector3.zero;
        world.isHide = false;
        world.canMove = true;
        world.isInFluid = false;
        return world;
    }
    #endregion
}