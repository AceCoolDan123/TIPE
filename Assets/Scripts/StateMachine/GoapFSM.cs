public enum GoapStateEnum
{
    GOTO,
    ANIMATE,
    USEOBJECT,
}

public class GoapFSM : GenericFSM<GoapStateEnum> {}