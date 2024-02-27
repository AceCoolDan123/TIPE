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
    [SerializeField]
    private GameObject appearance;
    private Collider _col;
    #endregion

    #region Unity Methods
    private void Awake() 
    {
        controller = GetComponent<CharacterController>();
        _col = GetComponent<Collider>();
    }
    private void Start() 
    {    
        appearance.SetActive(true);
        destinationGoto = transform.position;
        InitWorld();
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

    #region Perform Action Methods
    private void GotoNearestFluid(World worldSeen)
    {
        if (worldSeen.nearestFluidPos != null) 
        {
            destinationGoto = (Vector3)worldSeen.nearestFluidPos;
        }
    }

    private void HideInFluid(World worldSeen)
    {
        _col.isTrigger = true;
        appearance.SetActive(false);
    }
    #endregion

    #region Init Methods
    private void InitWorld()
    {
        worldSeen.isHide = false;
        worldSeen.isInFluid = false;
        worldSeen.canMove = true;
    }

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

    private bool HideGoal(World world)
    {
        if (world.isHide == true)
        {
            return true;
        }
        return false;
    }

    protected override void InitGoals()
    {
        nbGoals = 1;
        goals = new Planning.Goal[nbGoals];
        goals[0] = HideGoal; 
    }

    protected override void InitActions()
    {
        nbActions = 2;
        actions = new ActionClass[nbActions];
        Action<World> gotoFluid = GotoNearestFluid; 
        Action<World> hideInFluid = HideInFluid; 
        actions[0] = new GotoFluidAction(new World(player, null, null, null, false, null), gotoFluid, this); 
        actions[1] = new HideInFluidAction(new World(player, null, null, null, null, false), hideInFluid, this); 
    }
    #endregion

    #region Other Methods
    protected override void SenseAround()
    {
        worldSeen.nearestFluidPos = _fluid.ClosestPoint(transform.position);
        if (HaveFindPlayerPos())
        {
            worldSeen.player = new PlayerIntels();
            worldSeen.player.position = player.position;
            worldSeen.player.look = player.look;
            worldSeen.player.isNull = false;
        }
        else
        {
            worldSeen.player.isNull = true;
        }
    } 
    private bool HaveFindPlayerPos()
    {
        // 7 is the Player's layer Mask
        Collider[] colliders = Physics.OverlapSphere(transform.position, LookRange, 7); 
        float dist = float.PositiveInfinity;
        bool res = false;
        foreach (Collider col in colliders)
        {
            res = true;
            float tmp = Vector3.Distance(transform.position, col.transform.position);
            if (tmp < dist) 
            {
                dist = tmp;
                player.position = col.transform.position;
                player.look = col.transform.forward;
            }
        }
        // there is at least one player
        return res;
    }

    protected override void ExecuteActions()
    {
        if (actionStack.Count == 0)
        {
            actionStack = planning.CreatePlanning(worldSeen);
        }
        ActionClass action = actionStack.Pop();
        Action<World> PerformAction = action.PerformAction;
        PerformAction?.Invoke(worldSeen);
    }
    #endregion

    #region Debug Functions
    public void CreatePlanning(InputAction.CallbackContext ctx)
    {
        actionStack = planning.CreatePlanning(worldSeen);
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, LookRange);
    }
    #endregion
}