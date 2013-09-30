using UnityEngine;
using System.Collections;

namespace Gui3D
{
	
	public class ToggleMouse : MonoBehaviour 
	{
		
		public bool ToggleState = true;
		public bool SetToLocked = true;
		public bool HideCursor = true;
		
		public bool SetCursorPosition = false;
		public Vector3 CursorPosition = new Vector3(0, 0, 1000);
		
		// Use this for initialization
		void Start () 
		{
			gameObject.GetComponent<Gui3DButton>().OnPush += new Gui3DButton.OnPushEvent(Toggle);
		}
		
		// Update is called once per frame
		void Update () 
		{
		
		}
		
		void Toggle()
		{
			bool newState = !Screen.lockCursor;
			if (!ToggleState)
			{
				newState = SetToLocked;	
			}
			Screen.lockCursor = newState;
			GameObject cursorObject = gameObject.GetComponent<Gui3DObject>().GetGui3D().Cursor;
			if (cursorObject != null && cursorObject.GetComponent<Gui3DCursor>() != null)
			{
				cursorObject.GetComponent<Gui3DCursor>().MoveCursorWithMouse = !newState;
			}
			if (HideCursor && cursorObject != null && cursorObject.GetComponent<DrawToScreen>() != null)
			{
				cursorObject.GetComponent<DrawToScreen>().enabled = !newState;
			}
			if (SetCursorPosition && cursorObject != null)
			{
				cursorObject.transform.position = CursorPosition;
			}
		}
	}
}
