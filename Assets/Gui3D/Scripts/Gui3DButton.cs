using UnityEngine;
using System.Collections;

namespace Gui3D {
	public class Gui3DButton : Gui3DObject {
		
		public bool HoverPress = false;
		public bool ClickPress = false;
		public string InputName = null;
		
		private float hoverStartTime = 0;
		public float HoverDelaySeconds = 2;
		
		public delegate void OnPushEvent();
		public event OnPushEvent OnPush;
		
		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			if (ClickPress)
			{
				if (Input.GetMouseButtonUp(0) && GetGui3D().HoverObject == this)
				{
					// Notify of the event!
					OnPush();
				}
			}
			if (HoverPress)
			{
				if (GetGui3D().HoverObject == this)
				{
					if (hoverStartTime - Time.time >= HoverDelaySeconds)
					{
						// Notify of the event!
						OnPush();
					}
				}
				else
				{
					hoverStartTime = Time.time;
				}
			}
			if (InputName != null)
			{
				if (Selected == true && Input.GetButtonUp(InputName))
				{
					OnPush();
				}
			}
		}
	}
}
