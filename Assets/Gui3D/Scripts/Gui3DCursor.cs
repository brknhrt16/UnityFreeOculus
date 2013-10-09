using UnityEngine;
using System.Collections;

namespace Gui3D
{
	public class Gui3DCursor : Gui3DObject
	{
		
		public bool MoveCursorWithMouse = true;
		public bool CenterCursor = false;
		
		// Use this for initialization
		void Start () {
			// TODO: Place on all gui cameras rather than just active ones to allow 
			//       in game mode switching
			foreach (Camera camera in GetGui3D().GuiCameras)
			{
				if(camera.GetComponent<Gui3DCamera>() != null)
				{
					
					camera.GetComponent<Gui3DCamera>().OnPreRendered += new Gui3DCamera.OnPreRenderEvent(OnPreRender);
				}				
			}
		}
		
		// OnPreRender is called once before each camera's render
		void OnPreRender () {
			if (MoveCursorWithMouse)
			{
				Vector3 projectionPosition = Input.mousePosition;
				projectionPosition.z = 1000;
				Camera guiCamera = GetGui3D().GetMouseCamera();
				if (guiCamera)
				{
					transform.position = guiCamera.ScreenToWorldPoint(projectionPosition);
				}
			}
			else if (CenterCursor)
			{
				Camera guiCamera = GetGui3D().GetMouseCamera();
				if (guiCamera)
				{
					transform.position = guiCamera.transform.position + guiCamera.transform.forward.normalized * 1000;
				}
			}
		}
	}
}