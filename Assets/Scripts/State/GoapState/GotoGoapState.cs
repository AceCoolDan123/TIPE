using UnityEngine;
public class GotoGoapState : GoapState
{
    private CharacterController _controller;
    public GotoGoapState(Entity entity) 
    : base(GoapStateEnum.GOTO, entity) 
    {
        _controller = entity.GetComponent<CharacterController>();
    }

    public override void OnUpdate()
    {
        Vector3 direction = new Vector3(entity.DestinationGoto.x - entity.transform.position.x, 0f, entity.DestinationGoto.z - entity.transform.position.z);
        _controller.Move(direction * Time.deltaTime * entity.Speed);
        CheckTransition();
    }

    private void CheckTransition()
    {
        if (entity.IsUsingObject)
        {
            goapFSM.SetState(GoapStateEnum.USEOBJECT);   
        }
    }
}