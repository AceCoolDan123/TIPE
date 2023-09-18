using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;
		/*
		public void OnMove(InputAction.CallbackContext ctx)
		{
			move = ctx.ReadValue<Vector2>();
		}

		public void OnLook(InputAction.CallbackContext ctx)
		{
			if(cursorInputForLook)
			{
				look = ctx.ReadValue<Vector2>();
			}
		}

		public void OnJump(InputAction.CallbackContext ctx)
		{
			jump = ctx.performed;
		}

		public void OnSprint(InputAction.CallbackContext ctx)
		{
			sprint = ctx.performed;
		}
		
		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}*/
	}
	
