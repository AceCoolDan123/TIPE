using UnityEngine;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(CharacterController))]
public abstract class Entity : MonoBehaviour
{
    protected CharacterController controller;
    protected World worldSeen;
    protected int nbActions;
    protected ActionClass[] actions;
    protected int nbGoals;
    protected Planning.Goal[] goals;
    protected Queue<ActionClass> queueActions = new Queue<ActionClass>();
    protected Planning planning;
    protected Vector3 destinationGoto; 
    protected GoapFSM goapFSM;
    protected bool isUsingObject;
    protected Action useObject;
    [SerializeField] private float speed;

    #region Getters and setters
    public World WorldSeen { get { return worldSeen; } }
    public Vector3 DestinationGoto {get { return destinationGoto; } }
    public GoapFSM FSM { get { return goapFSM; } } 
    public bool IsUsingObject { get { return isUsingObject; } }
    public Action UseObject { get { return useObject; } }
    public float Speed { get { return speed; } }
    #endregion 

    protected abstract void InitActions();
    protected abstract void InitGoals();
    protected abstract void SenseAround();
    protected abstract void ExecuteActions();
}