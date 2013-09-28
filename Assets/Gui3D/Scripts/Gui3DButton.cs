using UnityEngine;
using System.Collections;

namespace Gui3D {
	public class Gui3DButton : Gui3DObject {
		
		public bool HoverPress = false;
		public bool ClickPress = false;
		public string InputName = null;
		
		private int hoverStartTime = 0;
		
		public delegate void OnPushEvent();
		public event OnPushEvent OnPush;
		
		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			if (HoverPress)
			{
				// Ray
				Ray ray = new Ray(GetGui3D().GuiCamera.gameObject.transform.position, GetGui3D().Cursor.transform.position);
				
				// Raycast Hit
				RaycastHit hit;
				
				// TODO: Create object that dispatches click events rather than casting n rays.
				if (Input.GetButtonUp(InputName) && Physics.Raycast(ray, out hit, 1000))
				{
					// If we click it
					if (hit.transform.gameObject == gameObject)
					{
						// Notify of the event!
						OnPush();
					}
				}
			}
		}
	}
}
