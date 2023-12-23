using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour 
{
  #region Player parameters
  [Header("Player")]
	[Tooltip("Move speed of the character in m/s")]
	[SerializeField] private float moveSpeed = 4.0f;
	[Tooltip("Sprint speed of the character in m/s")]
	[SerializeField] private float sprintSpeed = 6.0f;
	[Tooltip("Rotation speed of the character")]
	[SerializeField] private float rotationXSpeed = 1.0f;
	[SerializeField] private float rotationYSpeed = 1.0f;
	[Tooltip("Acceleration and deceleration")]
	[SerializeField] private float speedChangeRate = 10.0f;

	[Space(10)]
	[Tooltip("The height the player can jump")]
	[SerializeField] private float jumpHeight = 1.2f;
	[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
	[SerializeField] private float gravity = -15.0f;

	[Space(10)]
	[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
	[SerializeField] private float jumpTimeout = 0.1f;
	[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
	[SerializeField] private float fallTimeout = 0.15f;
	[Header("Player Grounded")]
	[Tooltip("Useful for rough ground")]
	[SerializeField] private float groundedOffset = -0.14f;
	[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
	[SerializeField] private float groundedRadius = 0.5f;
	[Tooltip("What layers the character uses as ground")]
	public LayerMask GroundLayers;

	[Header("Cinemachine")]
	[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
	public GameObject CinemachineCameraTarget;
	[Tooltip("How far in degrees can you move the camera up")]
	[SerializeField] private float topClamp = 90.0f;
	[Tooltip("How far in degrees can you move the camera down")]
	[SerializeField] private float bottomClamp = -90.0f;
   #endregion

  // player
	private float _verticalVelocity, _jumpTimeoutDelta, _fallTimeoutDelta;

  private PlayerFSM _fsm = new PlayerFSM();
  private bool _isRunning, _isGrabbing, _isGrounded, _isJumping;

  #region getters and setters
  public PlayerFSM Fsm { get { return _fsm; } }
  public bool IsRunning { get { return _isRunning; } set { _isRunning = value; } }
  public bool IsGrabbing { get { return _isGrabbing; } set { _isGrabbing = value; } }
  public bool IsJumping { get { return _isJumping; } set { _isJumping = value; } }
  public float VerticalVelocity { get { return _verticalVelocity;} set { _verticalVelocity = value; } }
  public float JumpTimeoutDelta { get { return _jumpTimeoutDelta; } set { _jumpTimeoutDelta = value;}}
  public float FallTimeoutDelta { get { return _fallTimeoutDelta;} set { _fallTimeoutDelta = value;}}
  public float MoveSpeed { get { return moveSpeed;}  set { moveSpeed = value; } }
  public float SprintSpeed { get { return sprintSpeed;}}
  public float RotationXSpeed { get { return rotationXSpeed;}}
  public float RotationYSpeed { get { return rotationYSpeed;}}
  public float SpeedChangeRate { get { return speedChangeRate;}}
  public float JumpHeight { get { return jumpHeight;}}
  public float Gravity { get { return gravity;}}
  public float JumpTimeout { get { return jumpTimeout;}}
  public float FallTimeout { get { return fallTimeout;}}
  public float GroundedOffset { get { return groundedOffset;}}
  public float GroundedRadius { get { return groundedRadius;}}
  public float TopClamp { get { return topClamp;}}
  public float BottomClamp { get { return bottomClamp;}}
  #endregion


  #region Unity Methods
  private void Start() 
  {
    SetupStates();
  }
  private void Update() 
  {
    _fsm.OnUpdate();
  }
  private void LateUpdate()
  {
    _fsm.OnLateUpdate();
  }
  #endregion

  #region Other Methods
  private void SetupStates()
  {
    //Init all player states
    LookPlayerState lookState = new LookPlayerState(this);
    GroundedPlayerState groundedState = new GroundedPlayerState(this);
    MovePlayerState moveState = new MovePlayerState(this);
    JumpPlayerState jumpState = new JumpPlayerState(this);

    _fsm.AddState(moveState);
    _fsm.AddState(lookState);
    _fsm.AddState(jumpState);
       
    _fsm.SetRootState(PlayerStateEnum.LOOK); 
  }
  #endregion

}