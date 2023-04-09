using UnityEngine;
using Mirror;

namespace StarterAssets
{
	[RequireComponent(typeof(CharacterController))]
	public partial class FirstPersonController : PlayerBase
	{
		[Header("Player")]
		[Tooltip("Move speed of the character in m/s")]
		public float MoveSpeed = 4.0f;
		[Tooltip("Sprint speed of the character in m/s")]
		public float SprintSpeed = 6.0f;
		[Tooltip("Rotation speed of the character")]
		public float RotationSpeed = 1.0f;
		[Tooltip("Acceleration and deceleration")]
		public float SpeedChangeRate = 10.0f;

		[Space(10)]
		[Tooltip("The height the player can jump")]
		public float JumpHeight = 1.2f;
		[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
		public float Gravity = -15.0f;

		[Space(10)]
		[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
		public float JumpTimeout = 0.1f;
		[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
		public float FallTimeout = 0.15f;

		[Header("Player Grounded")]
		[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
		[SyncVar]
		public bool Grounded = true;
		[Tooltip("Useful for rough ground")]
		public float GroundedOffset = -0.14f;
		[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
		public float GroundedRadius = 0.5f;
		[Tooltip("What layers the character uses as ground")]
		public LayerMask GroundLayers;

		[Header("Cinemachine")]
		public GameObject prefab_Camera;
		public GameObject prefab_CinematicVirtualCamera;
		[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
		public GameObject CinemachineCameraTarget;
		[Tooltip("How far in degrees can you move the camera up")]
		public float TopClamp = 90.0f;
		[Tooltip("How far in degrees can you move the camera down")]
		public float BottomClamp = -90.0f;
		private Quaternion lastCameraRot;

		// cinemachine
		private float _cinemachineTargetPitch;

		// player
		private float _speed;
		private float _rotationVelocity;
		private float _verticalVelocity;
		private float _terminalVelocity = 53.0f;

		// timeout deltatime
		private float _jumpTimeoutDelta;
		private float _fallTimeoutDelta;

	
		private CharacterController _controller;
		private GameObject _mainCamera;

		private const float _threshold = 0.01f;

		private bool IsCurrentDeviceMouse
		{
			get
			{
				#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
				return PlayerInputInstance._playerInput.currentControlScheme == "KeyboardMouse";
				#else
				return false;
				#endif
			}
		}

		public virtual void Awake()
		{
			if(isLocalPlayer){
				// get a reference to our main camera
				if (_mainCamera == null) {
					_mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
				}
			}
		}

        public override void Initialize() {

            base.Initialize();

			_controller = GetComponent<CharacterController>();

			// reset our timeouts on start
			_jumpTimeoutDelta = JumpTimeout;
			_fallTimeoutDelta = FallTimeout;

			if(isLocalPlayer){

				//Spawn the camera
				var cameraObject = Instantiate(prefab_Camera);
				var cinematicCamera = Instantiate(prefab_CinematicVirtualCamera);

				//Setup the camera
				var cinematicComp = cinematicCamera.GetComponent<Cinemachine.CinemachineVirtualCamera>();
				cinematicComp.Follow = CinemachineCameraTarget.transform;

			}
		}

        public override void Update()
		{
			base.Update();

			if(isLocalPlayer){

				Client_JumpInputCheck();
				Client_Move();
                SC_Move();

            }

			if(isServer){

				Server_JumpAndGravity();
				Server_GroundedCheck();
				SC_Move();

			}
		}

        public virtual void LateUpdate()
		{
			if(isLocalPlayer){

				Client_CameraRotation();
				SC_CameraRotation();

            }

			if(isServer){
				SC_CameraRotation();
			}
		}

        #region Ground Checking

        [Server]
		private void Server_GroundedCheck()
		{
			// set sphere position, with offset
			Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
			Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
		}

        #endregion

        #region Camera and Character Rotation

		float sc_RotationVelocity = 0f;

		[Client]
		private void Client_CameraRotation()
		{
			// if there is an input
			if (PlayerInputInstance.instance.look.sqrMagnitude >= _threshold)
			{
				//Don't multiply mouse input by Time.deltaTime
				float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
				
				_cinemachineTargetPitch += PlayerInputInstance.instance.look.y * RotationSpeed * deltaTimeMultiplier;
				_rotationVelocity = PlayerInputInstance.instance.look.x * RotationSpeed * deltaTimeMultiplier;

				// clamp our pitch rotation
				_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

                // Update Cinemachine camera target pitch
                lastCameraRot = CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);
				sc_RotationVelocity = _rotationVelocity;

                // rotate the player left and right
                //transform.Rotate(Vector3.up * _rotationVelocity);
                Client_SendRotationVelocity(_rotationVelocity, lastCameraRot);

			}else{

                sc_RotationVelocity = 0f;
                Client_SendRotationVelocity(0f, lastCameraRot);

			}
			
		}

		[Command]
		private void Client_SendRotationVelocity(float velocity, Quaternion cameraLocalRot){

			sc_RotationVelocity = velocity;
			CinemachineCameraTarget.transform.localRotation = cameraLocalRot;

        }

		private void SC_CameraRotation(){

			transform.Rotate(Vector3.up * sc_RotationVelocity);

		}

        #endregion

        #region Movement

		bool sc_IsSprint = false;
		bool sc_AnalogMovement = false;
		Vector2 sc_InputMove;

		[Client]
		private void Client_Move(){

			if(sc_IsSprint != PlayerInputInstance.instance.sprint || sc_AnalogMovement != PlayerInputInstance.instance.analogMovement || sc_InputMove != PlayerInputInstance.instance.move){

                sc_IsSprint = PlayerInputInstance.instance.sprint;
                sc_AnalogMovement = PlayerInputInstance.instance.analogMovement;
                sc_InputMove = PlayerInputInstance.instance.move;

                Client_SyncMovement(sc_IsSprint, sc_IsSprint, sc_InputMove);

			}

		}

		[Command]
		private void Client_SyncMovement(bool sprint, bool analogMovement, Vector2 move){

			sc_IsSprint = sprint;
			sc_AnalogMovement = analogMovement;
			sc_InputMove = move;

		}

		/// <summary>
		/// This function to call on Local Server and Server
		/// </summary>
		private void SC_Move()
		{
			// set target speed based on move speed, sprint speed and if sprint is pressed
			float targetSpeed = sc_IsSprint ? SprintSpeed : MoveSpeed;

			// a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

			// note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is no input, set the target speed to 0
			//if (PlayerInputInstance.instance.move == Vector2.zero) targetSpeed = 0.0f;
			if (sc_InputMove == Vector2.zero) targetSpeed = 0.0f;

			// a reference to the players current horizontal velocity
			float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

			float speedOffset = 0.1f;
			//float inputMagnitude = PlayerInputInstance.instance.analogMovement ? PlayerInputInstance.instance.move.magnitude : 1f;
			float inputMagnitude = sc_AnalogMovement ? sc_InputMove.magnitude : 1f;

			// accelerate or decelerate to target speed
			if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset) {
				// creates curved result rather than a linear one giving a more organic speed change
				// note T in Lerp is clamped, so we don't need to clamp our speed
				_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

				// round speed to 3 decimal places
				_speed = Mathf.Round(_speed * 1000f) / 1000f;
			} else {
				_speed = targetSpeed;
			}

			// normalise input direction
			//Vector3 inputDirection = new Vector3(PlayerInputInstance.instance.move.x, 0.0f, PlayerInputInstance.instance.move.y).normalized;
			Vector3 inputDirection = new Vector3(sc_InputMove.x, 0.0f, sc_InputMove.y).normalized;

			// note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is a move input rotate player when the player is moving
			//if (PlayerInputInstance.instance.move != Vector2.zero) {
			if (sc_InputMove != Vector2.zero) {
				// move
				//inputDirection = transform.right * PlayerInputInstance.instance.move.x + transform.forward * PlayerInputInstance.instance.move.y;
				inputDirection = transform.right * sc_InputMove.x + transform.forward * sc_InputMove.y;
			}

			// move the player
			_controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
		}

        #endregion

        #region Jump

        bool s_isJump = false;

		//Run on client
		private void Client_JumpInputCheck(){

			if(PlayerInputInstance.instance.jump){
				CmdJumpAndGravity();
			}

			PlayerInputInstance.instance.jump = false;

		}

		private void Server_JumpAndGravity()
		{
			if (Grounded)
			{
				// reset the fall timeout timer
				_fallTimeoutDelta = FallTimeout;

				// stop our velocity dropping infinitely when grounded
				if (_verticalVelocity < 0.0f)
				{
					_verticalVelocity = -2f;
				}

				// Jump
				if (s_isJump && _jumpTimeoutDelta <= 0.0f)
				{
					// the square root of H * -2 * G = how much velocity needed to reach desired height
					_verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
				}

				// jump timeout
				if (_jumpTimeoutDelta >= 0.0f)
				{
					_jumpTimeoutDelta -= Time.deltaTime;
				}
			}
			else
			{
				// reset the jump timeout timer
				_jumpTimeoutDelta = JumpTimeout;

				// fall timeout
				if (_fallTimeoutDelta >= 0.0f)
				{
					_fallTimeoutDelta -= Time.deltaTime;
				}

				// if we are not grounded, do not jump
				//PlayerInputInstance.instance.jump = false;
			}

			// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
			if (_verticalVelocity < _terminalVelocity)
			{
				_verticalVelocity += Gravity * Time.deltaTime;
			}

			s_isJump = false;
		}

		[Command]
		private void CmdJumpAndGravity(){

			s_isJump = true;

		}

        #endregion

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
		{
			if (lfAngle < -360f) lfAngle += 360f;
			if (lfAngle > 360f) lfAngle -= 360f;
			return Mathf.Clamp(lfAngle, lfMin, lfMax);
		}

		private void OnDrawGizmosSelected()
		{
			Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
			Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

			if (Grounded) Gizmos.color = transparentGreen;
			else Gizmos.color = transparentRed;

			// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
			Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
		}
	}
}