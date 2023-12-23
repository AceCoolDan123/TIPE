public enum PlayerStateEnum
{
    NONE,
    GROUNDED,
    JUMP,
    MOVE,
    RUN,
    GRAB,
    LOOK,
}

public class PlayerFSM : GenericHFSM<PlayerStateEnum> {}
