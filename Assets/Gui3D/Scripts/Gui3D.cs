using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

namespace Gui3D
{
	public class Gui3D : MonoBehaviour
	{
		public bool UseCursor = true;
		public bool HideMouse = true;
	    public GameObject Cursor;
		
		public Camera[] GuiCameras;
		
		public Gui3DObject HoverObject
		{
			private set;
			get;
		}
		
		/// <summary>
		/// Gets the camera containing the mouse if any, else an arbitrary camera.
		/// </summary>
		/// <returns>
		/// A camera.
		/// </returns>
		public Camera GetMouseCamera()
		{
			if (Cursor.GetComponent<Gui3DCursor>().MoveCursorWithMouse)
			{
				foreach (Camera guiCam in GuiCameras)
				{
					if (guiCam.pixelRect.Contains(new Vector2(Input.mousePosition.x, Input.mousePosition.y)))
					{
						return guiCam;
					}
				}
			}
			else
			{
				return GuiCameras[0];
			}
			return null;
		}
		
		Gui3DObject GetContainingGuiObject(GameObject obj)
		{
			Gui3DObject guiObject = obj.GetComponent<Gui3DObject>();
			if (guiObject != null)
			{
				return guiObject;
			}
			if (obj.transform.parent == null)
			{
				return null;
			}
			return GetContainingGuiObject(obj.transform.parent.gameObject);
		}
		
		void Start()
		{
			GuiCameras = this.GetComponentsInChildren<Camera>();
		}
		
		void Update()
		{
			if(HideMouse)
			{
				Screen.showCursor = false;
			}
			else
			{
				Screen.showCursor = true;
			}
			
			HoverObject = null;
			if (UseCursor == true && Cursor != null)
			{
				Camera guiCamera = GetMouseCamera();
				if (guiCamera != null)
				{
					// Ray
					Vector3 projectedPosition = guiCamera.WorldToScreenPoint(Cursor.transform.position);
					Ray ray = guiCamera.ScreenPointToRay(projectedPosition);
					
					// Raycast Hit
					RaycastHit hit;
					
					if (Physics.Raycast(ray, out hit, 1000, 1 << LayerMask.NameToLayer("Gui3D")))
					{
						HoverObject = GetContainingGuiObject(hit.transform.gameObject);
					}
				}
			}
		}
	}
}
