using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
public abstract class Entity : MonoBehaviour
{
    protected CharacterController controller;
    protected World worldSeen;
    public World WorldSeen { get {return worldSeen;} }
    protected int nbActions;
    protected ActionClass[] actions;
    protected int nbGoals;
    protected Planning.Goal[] goals;
    protected Queue<ActionClass> queueActions = new Queue<ActionClass>();
    protected Planning planning;


    protected abstract void InitActions();
    protected abstract void InitGoals();
    protected abstract void SenseAround();
    protected abstract void ExecuteActions();
}