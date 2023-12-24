using UnityEngine;
public class GroundedPlayerState : PlayerState
{
  public GroundedPlayerState(Player player): base(PlayerStateEnum.GROUNDED, player) {}

  public override void OnEnter() 
  {
    Debug.Log("Enter Grounded State");
  }
  public override void OnUpdate() 
  {
    Grounded();
    CheckTransitions();
  }
  public override void OnLateUpdate() {}
  public override void OnExit() 
  { 
    Debug.Log("Exit Grounded State");
  }

  private void Grounded()
  {
	  // stop our velocity dropping infinitely when grounded
	  if (player.VerticalVelocity < 0.0f)
	  {
	  	player.VerticalVelocity = -2f;
	  }
	  // jump timeout
	  if (player.JumpTimeoutDelta >= 0.0f)
	  {
	  	player.JumpTimeoutDelta -= Time.deltaTime;
	  }
  }

  protected override void CheckTransitions()
  {
    if (playerInputs.jump && player.JumpTimeoutDelta <= 0.0f)
		{
      fsm.ChangeState(id, PlayerStateEnum.JUMP);
		}
  }

  public override void FindChildState()
  {
    fsm.SetParentChildRelation(id, PlayerStateEnum.MOVE);
  }
}