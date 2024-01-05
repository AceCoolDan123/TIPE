using UnityEngine;

public struct World
{
    // if a value is null, this value is not considered, don't care ! 
    public Vector3 playerPos;
    public Vector3 nearestFluidPos;
    public bool isHide;
    public bool canMove;
    public bool isInFluid;
};