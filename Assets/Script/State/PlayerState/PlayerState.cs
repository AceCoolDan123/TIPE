using UnityEngine;
public abstract class PlayerState : State<PlayerStateEnum>
{
    protected Player player;
    protected PlayerFSM fsm;
    protected PlayerInputs playerInputs;
    protected PlayerState subState;
    protected PlayerState superState;

    public PlayerState(PlayerStateEnum pse, Player player)
    {
        id = pse;
        this.player = player;
        playerInputs = player.GetComponent<PlayerInputs>();
        fsm = player.Fsm;
    } 

    protected void SetSuperState(PlayerState superState)
    {
        this.superState = superState;
    }  

    protected void SetSubState(PlayerState subState)
    {
        this.subState = subState;
        subState.SetSuperState(this.subState);
    }  
}