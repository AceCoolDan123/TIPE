using UnityEngine;
public class LookPlayerState : PlayerState
{
	private LookPlayerState _lookPlayerState;
	private const float _threshold = 0.01f;
    private const float deltaTimeMultiplier = 1.0f;
	
    private float _cinemachineTargetPitch, _rotationVelocity;
    
    public LookPlayerState(Player player)
	: base(PlayerStateEnum.LOOK, player)
    {
        this.player = player;
    }

    public override void OnEnter() 
	{
		FindChildState();
	}
    public override void OnUpdate() {}
    public override void OnLateUpdate() 
    {
        CameraRotation();
    }
    public override void OnExit() {}

    private void CameraRotation()
	{
		// if there is an input
		if (playerInputs.look.sqrMagnitude >= _threshold)
		{
		//Don't multiply mouse input by Time.deltaTime
		
		_cinemachineTargetPitch += playerInputs.look.y * player.RotationYSpeed * deltaTimeMultiplier;
        _rotationVelocity = playerInputs.look.x * player.RotationXSpeed * deltaTimeMultiplier;

			// clamp our pitch rotation
			_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, player.BottomClamp, player.TopClamp);

			// Update Cinemachine camera target pitch
			player.CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);

			// rotate the player left and right
			player.transform.Rotate(Vector3.up * _rotationVelocity);
		}
	}

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
		{
			if (lfAngle < -360f) lfAngle += 360f;
			if (lfAngle > 360f) lfAngle -= 360f;
			return Mathf.Clamp(lfAngle, lfMin, lfMax);
		}

    protected override void CheckTransitions() {}

	protected override void FindChildState()
	{
		currentChildState = PlayerStateEnum.GROUNDED;
	}
}