public enum PlayerStateEnum
{
    GROUNDED,
    JUMP,
    MOVE,
    RUN,
    GRAB,
    LOOK,
}

public class PlayerFSM : GenericFSM<PlayerStateEnum> {}
