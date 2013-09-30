using UnityEngine;
using System.Collections;

namespace Gui3D
{
	
	public class ToggleMouse : MonoBehaviour 
	{
		public bool ToggleState = true;
		public bool SetToLocked = true;
		public bool HideCursor = true;
		
		public bool SetLocked = true;
		
		public bool SetMoveWithMouse = true;
		
		public bool SetCursorPosition = false;
		public Vector3 CursorPosition = new Vector3(0, 0, 1000);
		
		// Use this for initialization
		void Start () 
		{
			Screen.lockCursor = true;
			gameObject.GetComponent<Gui3DButton>().OnPush += new Gui3DButton.OnPushEvent(Toggle);
		}
		
		// Update is called once per frame
		void Update () 
		{
		
		}
		
		void Toggle()
		{
			bool newState = SetToLocked;
			if (SetLocked)
			{
				if (ToggleState)
				{
					Screen.lockCursor = newState;
				}
				else
				{
					Screen.lockCursor = !Screen.lockCursor;
				}
			}
			GameObject cursorObject = gameObject.GetComponent<Gui3DObject>().GetGui3D().Cursor;
			if (SetMoveWithMouse && cursorObject != null && cursorObject.GetComponent<Gui3DCursor>() != null)
			{
				if (ToggleState)
				{
					cursorObject.GetComponent<Gui3DCursor>().MoveCursorWithMouse = !cursorObject.GetComponent<Gui3DCursor>().MoveCursorWithMouse;
				}
				else
				{
					cursorObject.GetComponent<Gui3DCursor>().MoveCursorWithMouse = !newState;
				}
			}
			if (HideCursor && cursorObject != null && cursorObject.GetComponent<DrawToScreen>() != null)
			{
				if (ToggleState)
				{
					cursorObject.GetComponent<DrawToScreen>().enabled = !cursorObject.GetComponent<DrawToScreen>().enabled;
				}
				else
				{
					cursorObject.GetComponent<DrawToScreen>().enabled = !newState;
				}
			}
			if (SetCursorPosition && cursorObject != null)
			{
				cursorObject.transform.position = CursorPosition;
			}
		}
		
		void ToggleMoveWithMouse()
		{
			
		}
	}
}
