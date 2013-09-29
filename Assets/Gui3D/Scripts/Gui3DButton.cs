using UnityEngine;
using System.Collections;

namespace Gui3D {
	public class Gui3DButton : Gui3DObject {
		
		public bool HoverPress = false;
		public bool ClickPress = false;
		public string InputName = "";
		
		private float hoverStartTime = 0;
		public float HoverDelaySeconds = 2;
		
		public delegate void OnPushEvent();
		public event OnPushEvent OnPush;
		
		/// <summary>
		/// Push this button, notifying all handlers.
		/// </summary>
		public void Push()
		{
			if (OnPush != null)
			{
				OnPush();
			}
		}
		
		bool IsHovering()
		{
			if (GetGui3D().HoverObject == this)
			{
				return true;
			}
			else
			{
				Gui3DObject[] guiObjects = gameObject.GetComponentsInChildren<Gui3DObject>();
				foreach (Gui3DObject guiObject in guiObjects)
				{
					if (guiObject == GetGui3D().HoverObject)
					{
						return true;
					}
				}
			}
			return false;
		}
		
		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			if (ClickPress)
			{
				if (IsHovering() && Input.GetMouseButtonUp(0))
				{
					// Notify of the event!
					Push();
				}
			}
			if (HoverPress)
			{
				if (IsHovering())
				{
					if (Time.time - hoverStartTime >= HoverDelaySeconds)
					{
						// Notify of the event!
						Push();
						hoverStartTime = Time.time;
					}
				}
				else
				{
					hoverStartTime = Time.time;
				}
			}
			if (InputName != "")
			{
				if (Selected == true && Input.GetButtonUp(InputName))
				{
					Push();
				}
			}
		}
	}
}
