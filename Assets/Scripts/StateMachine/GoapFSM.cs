using UnityEngine;
public enum GoapStateEnum
{
    GOTO,
    USEOBJECT,
}

public class GoapFSM : GenericFSM<GoapStateEnum> {}