using UnityEngine;
using System.Collections;

namespace Gui3D {
	public class Gui3DButton : Gui3DObject {
		
		public bool HoverPress = false;
		public bool ClickPress = false;
		public string InputName = null;
		
		private int hoverStartTime = 0;
		public int HoverDelayMilliseconds = 2000;
		
		public delegate void OnPushEvent();
		public event OnPushEvent OnPush;
		
		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			if (ClickPress)
			{
				// Ray
				Ray ray = new Ray(GetGui3D().GuiCamera.gameObject.transform.position, GetGui3D().Cursor.transform.position);
				
				// Raycast Hit
				RaycastHit hit;
				
				// TODO: Create object that dispatches click events rather than casting n rays.
				if (Input.GetMouseButtonUp(0) && Physics.Raycast(ray, out hit, 1000))
				{
					// If we click it
					if (hit.transform.gameObject == gameObject)
					{
						// Notify of the event!
						OnPush();
					}
				}
			}
			if (HoverPress)
			{
				// Ray
				Ray ray = new Ray(GetGui3D().GuiCamera.gameObject.transform.position, GetGui3D().Cursor.transform.position);
				
				// Raycast Hit
				RaycastHit hit;
				
				// TODO: Create object that dispatches click events rather than casting n rays.
				if (Physics.Raycast(ray, out hit, 1000))
				{
					// If we click it
					if (hit.transform.gameObject == gameObject)
					{
						if (hoverStartTime - Time.time >= HoverDelayMilliseconds)
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
