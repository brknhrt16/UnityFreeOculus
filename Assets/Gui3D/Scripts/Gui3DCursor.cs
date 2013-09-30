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
		
		}
		
		// Update is called once per frame
		void Update () {
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