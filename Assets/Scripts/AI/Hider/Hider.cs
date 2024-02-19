using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
public class Hider : Entity
{
    #region Variables
    [SerializeField]
    private Collider _fluid;
    #endregion

    #region Unity Methods
    private void Awake() 
    {
        controller = GetComponent<CharacterController>();   
    }
    private void Start() 
    {    
        InitFSM();
        InitGoals();
        InitActions();
        planning = new Planning(goals, nbGoals, actions, nbActions);
        SenseAround();
        ExecuteActions();
    }
    private void Update() 
    {
        SenseAround();
        goapFSM.OnUpdate();
    }
    #endregion

    #region Init Methods
    private void InitFSM()
    {
        goapFSM = new GoapFSM();
        GoapState gotoState = new GotoGoapState(this);
        GoapState use = new UseObjectGoapState(this);
        goapFSM.AddState(gotoState);
        goapFSM.AddState(use);
        isUsingObject = false;
        goapFSM.SetState(GoapStateEnum.GOTO);
    }

    private void GotoNearestFluid(World worldSeen)
    {
        destinationGoto = worldSeen.nearestFluidPos;
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
        Action<World> gotoFluid = GotoNearestFluid; 
        actions[0] = new GotoFluidAction(defaultWorld(), gotoFluid); 
    }
    #endregion

    #region Other Methods
    protected override void SenseAround()
    {
        worldSeen.nearestFluidPos = _fluid.ClosestPoint(transform.position);
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
        Action<World> PerformAction = action.PerformAction;
        PerformAction?.Invoke(worldSeen);
    }
    #endregion

    #region Debug Functions
    public void CreatePlanning(InputAction.CallbackContext ctx)
    {
        queueActions = planning.CreatePlanning(worldSeen);
    }
    #endregion
}