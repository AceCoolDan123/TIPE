using UnityEngine;

public struct PlayerIntels
{
    public bool isNull;
    public Vector3 position;
    public Vector3 look;
};

public struct World
{
    public PlayerIntels player;
    public Vector3? nearestFluidPos;
    public Vector3? nearestObjectPos;
    public bool? isHide;
    public bool? canMove;
    public bool? isInFluid;

    public World(PlayerIntels player, Vector3? nearestFluidPos, Vector3? nearestObjectPos, bool? isHide, bool? canMove, bool? isInFluid)
    {
        this.player.isNull = player.isNull;
        this.player.position = player.position;
        this.player.look = player.look;
        this.nearestFluidPos = nearestFluidPos;
        this.isHide = isHide;
        this.canMove = canMove;
        this.isInFluid = isInFluid;
        this.nearestObjectPos = nearestObjectPos;
    }
};