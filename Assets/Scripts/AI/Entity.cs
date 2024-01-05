using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    protected CharacterController controller;
    protected World worldSeen;
    public World WorldSeen { get {return worldSeen;} }

    protected abstract void SenseAround();
}