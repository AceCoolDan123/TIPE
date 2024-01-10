using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
public class Hider : Entity
{
    #region Variables
    [SerializeField]
    private Collider _fluid;
    private ActionClass.ActionFunc _goToFluidAction;
    #endregion

    #region Unity Methods
    private void Awake() 
    {
        controller = GetComponent<CharacterController>();   
    }
    private void Start() 
    {    
        _goToFluidAction = GotoNearestFluid;
        InitGoals();
        InitActions();
        planning = new Planning(goals, nbGoals, actions, nbActions);
        ExecuteActions();
    }
    private void Update() 
    {
        SenseAround();    
    }
    #endregion

    #region Init Methods
    private bool GotoNearestFluid()
    {
        Vector3 dest = worldSeen.nearestFluidPos;
        Debug.Log(dest);
        controller.Move(dest);
        // while (transform.position != dest) { wait; }
        return true;
    }

    private bool HideGoal(World world)
    {
        return world.isHide;
    }

    protected override void InitGoals()
    {
        nbGoals = 1;
        goals = new Planning.Goal[nbGoals];
        goals[0] = HideGoal; 
    }

    protected override void InitActions()
    {
        nbActions = 1;
        actions = new ActionClass[1];
        actions[0] = new GotoFluidAction(defaultWorld(), _goToFluidAction, this); 
    }
    #endregion

    #region Other Methods
    protected override void SenseAround()
    {
        Vector3 pos = _fluid.ClosestPoint(transform.position);
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

    protected override void ExecuteActions()
    {
        if (queueActions.Count == 0)
        {
            queueActions = planning.CreatePlanning(worldSeen);
        }
        ActionClass action = queueActions.Dequeue();
        ActionClass.ActionFunc PerformAction = action.PerformAction;
        bool result = PerformAction.Invoke();
        if (!result) 
        {
            planning.CreatePlanning(worldSeen);
        }
    }
    #endregion

    #region Debug Functions
    public void CreatePlanning(InputAction.CallbackContext ctx)
    {
        queueActions = planning.CreatePlanning(worldSeen);
    }
    #endregion
}