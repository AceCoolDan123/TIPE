using UnityEngine;

public class MovePlayerState : PlayerState
{
    private float _speed;
    private CharacterController _controller;

    public MovePlayerState(Player player): base(PlayerStateEnum.MOVE, player)
    {
        _controller = player.GetComponent<CharacterController>();;
    }

    public override void OnEnter() {}

    public override void OnUpdate() 
    {
        Move();
        CheckTransitions();
    }

    public override void OnLateUpdate() {}

    public override void OnExit() {}

    private void Move()
    {
        // set target speed based on move speed, sprint speed and if sprint is pressed
			// a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon
			// note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is no input, set the target speed to 0
			if (playerInputs.move == Vector2.zero) player.MoveSpeed = 0.0f;

			// a reference to the players current horizontal velocity
			float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

			float speedOffset = 0.1f;
			
            float inputMagnitude = playerInputs.analogMovement ? playerInputs.move.magnitude : 1f;

			// accelerate or decelerate to target speed
			if (currentHorizontalSpeed < player.MoveSpeed - speedOffset || currentHorizontalSpeed > player.MoveSpeed + speedOffset)
			{
				// creates curved result rather than a linear one giving a more organic speed change
				// note T in Lerp is clamped, so we don't need to clamp our speed
				_speed = Mathf.Lerp(currentHorizontalSpeed, player.MoveSpeed * inputMagnitude, Time.deltaTime * player.SpeedChangeRate);

				// round speed to 3 decimal places
				_speed = Mathf.Round(_speed * 1000f) / 1000f;
			}
			else
			{
				_speed = player.MoveSpeed;
			}

			// normalise input direction
			Vector3 inputDirection = new Vector3(playerInputs.move.x, 0.0f, playerInputs.move.y).normalized;

			// note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is a move input rotate player when the player is moving
			if (playerInputs.move != Vector2.zero)
			{
				// move
				inputDirection = player.transform.right * playerInputs.move.x + player.transform.forward * playerInputs.move.y;
			}

			// move the player
			_controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, player.VerticalVelocity, 0.0f) * Time.deltaTime);
    }

    protected override void CheckTransitions()
    {

    }
}