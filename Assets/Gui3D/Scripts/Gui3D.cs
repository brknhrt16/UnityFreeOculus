using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

namespace Gui3D
{
	public class Gui3D : MonoBehaviour
	{
		public bool UseCursor = true;
		public bool MoveCursorWithMouse = true;
	    public GameObject Cursor;
		
		public Camera[] GuiCameras;
		
		public Gui3DObject HoverObject
		{
			private set;
			get;
		}
		
		void Start()
		{
			GuiCameras = this.GetComponentsInChildren<Camera>();
		}
		
		void Update()
		{
			Camera guiCamera = null;
			if (MoveCursorWithMouse)
			{
				foreach (Camera guiCam in GuiCameras)
				{
					if (guiCam.pixelRect.Contains(new Vector2(Input.mousePosition.x, Input.mousePosition.y)))
					{
						guiCamera = guiCam;
					}
				}
			}
			else
			{
				guiCamera = GuiCameras[0];
			}
			
			if (UseCursor == true && guiCamera != null)
			{
				// Ray
				if (MoveCursorWithMouse)
				{
					Vector3 projectionPosition = Input.mousePosition;
					projectionPosition.z = 1000;
					Cursor.transform.position = guiCamera.ScreenToWorldPoint(projectionPosition);
				}
				Vector3 projectedPosition = guiCamera.WorldToScreenPoint(Cursor.transform.position);
				Ray ray = guiCamera.ScreenPointToRay(projectedPosition);
				
				// Raycast Hit
				RaycastHit hit;
				
				if (Physics.Raycast(ray, out hit, 1000, 1 << LayerMask.NameToLayer("Gui3D")))
				{
					print (hit.transform.gameObject);
					HoverObject = hit.transform.gameObject.GetComponent<Gui3DObject>();
					if (HoverObject != null) 
					{
						Vector3 newPosition = HoverObject.gameObject.transform.position;
						newPosition.y += .1f;
						HoverObject.gameObject.transform.position = newPosition;
					}
				}
				else
				{
					HoverObject = null;
				}
			}
		}
	}
}
