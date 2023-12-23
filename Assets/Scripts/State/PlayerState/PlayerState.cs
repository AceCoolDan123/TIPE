using UnityEngine;
public abstract class PlayerState : HState<PlayerStateEnum>
{
    protected Player player;
    protected PlayerFSM fsm;
    protected PlayerInputs playerInputs;

    public PlayerState(PlayerStateEnum id, Player player)
    {
        this.id = id;
        this.player = player;
        playerInputs = player.GetComponent<PlayerInputs>();
        fsm = player.Fsm;
        currentChildState = PlayerStateEnum.NONE;
        currentParentState = PlayerStateEnum.NONE;
    } 
}