using UnityEngine;
public class JumpPlayerState : PlayerState
{
	private const float _terminalVelocity = 53.0f;
    private bool _isGrounded;
    public JumpPlayerState(Player player): base(PlayerStateEnum.JUMP, player) {}

    public override void OnEnter() 
    {
        Debug.Log("Enter Jump state");
        // JUMP forest !!!
		// the square root of H * -2 * G = how much velocity needed to reach desired height
        player.VerticalVelocity = Mathf.Sqrt(player.JumpHeight * -2f * player.Gravity);

        // reset the jump timeout timer
        player.JumpTimeoutDelta = player.JumpTimeout;
    }
    public override void OnUpdate() 
    {
        Falling();
        GroundedCheck();
        CheckTransitions();
    }

    public override void OnLateUpdate() {}
    public override void OnExit() 
    {
        player.FallTimeoutDelta = player.FallTimeout;
        Debug.Log("Exit Jump state");
    }

    private void Falling()
    {
			// fall timeout
			if (player.FallTimeoutDelta >= 0.0f)
			{
				player.FallTimeoutDelta -= Time.deltaTime;
			}			
		
		// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
		if (player.VerticalVelocity < _terminalVelocity)
		{
			player.VerticalVelocity += player.Gravity * Time.deltaTime;
		}
    }
    private void GroundedCheck()
	{
		// set sphere position, with offset
		Vector3 spherePosition = new Vector3(player.transform.position.x, player.transform.position.y + player.GroundedOffset, player.transform.position.z);
		_isGrounded = Physics.CheckSphere(spherePosition, player.GroundedRadius, player.GroundLayers, QueryTriggerInteraction.Ignore);
	}

    protected override void CheckTransitions()
    {
        if(_isGrounded)
        {
            fsm.ChangeState(id, PlayerStateEnum.GROUNDED);
        }
    } 

    public override void FindChildState()
    {
        fsm.SetParentChildRelation(id, PlayerStateEnum.MOVE);    
    }
}
