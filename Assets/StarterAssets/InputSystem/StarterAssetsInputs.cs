using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool fire;
		public bool fired;
		public bool firePressed {
			get {
				if(fire && !fired) {

					fired = true;
					return true;

				} else {

					return false;

				}
			}
		}

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

		public virtual void Start(){
		}

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
        public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		public void OnFire(InputValue value)
		{
			if (cursorInputForLook) {

                var isPress = value.Get<float>() == 1f?true:false;
                FireInput(isPress);

            }
		}
#endif

		public void MoveInput(Vector2 newMoveDirection)
		{
			if (!GameManagerBase.ClientReady)
				return;

			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
            if (!GameManagerBase.ClientReady)
                return;

            look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
            if (!GameManagerBase.ClientReady)
                return;

            jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
            if (!GameManagerBase.ClientReady)
                return;

            sprint = newSprintState;
		}

		public void FireInput(bool newFireState)
		{
            if (!GameManagerBase.ClientReady)
                return;

            if (!newFireState)
				fired = false;

            fire = newFireState;
		}
		
		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}