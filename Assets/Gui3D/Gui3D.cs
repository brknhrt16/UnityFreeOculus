using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

namespace Gui3D
{
	public class Gui3D : MonoBehaviour
	{
	    public string[] cameraNames = new string[0];
		
		private int hasInitialized = 30;
		
		// Use this for initialization
		void Start ()
		{
			/*OVRCameraController cameraController = null;//transform.parent.GetComponent<OVRCameraController>();
			if (cameraController != null)
			{
				float eyeHeight = cameraController.EyeCenterPosition.y;
				float neckHeight = cameraController.NeckPosition.y;
				transform.localPosition = new Vector3(transform.localPosition.x, eyeHeight + neckHeight, transform.localPosition.z);
			}*/
		}
		
		// Update is called once per frame
		void Update ()
		{
			IntializeCameras();
			
		}
		
		void IntializeCameras()
		{
			if (hasInitialized == 0)
			{
				hasInitialized--;
				
				GameObject firstCamera = null;
				GameObject cameraHolderObject = new GameObject("GuiCameraHolder");
				if (cameraNames.Length >= 1)
				{
					firstCamera = GameObject.Find(cameraNames[0]);
					print (firstCamera.transform.position);
				}
				
				foreach (string cameraName in cameraNames)
				{
					var cameraObject = GameObject.Find(cameraName);
					var camera = GameObject.Find(cameraName).GetComponent<Camera>();
					
					GameObject dupCameraObject = new GameObject(cameraName + "GuiCamera");
					dupCameraObject.transform.parent = cameraHolderObject.transform;
					Camera dupCamera = dupCameraObject.AddComponent<Camera>();
					dupCamera.CopyFrom(camera);
					dupCamera.clearFlags = CameraClearFlags.Depth;
					dupCamera.cullingMask = 1 << LayerMask.NameToLayer("Gui3D");
				}	
				
				if (cameraNames.Length >= 1)
				{
					cameraHolderObject.transform.position = -firstCamera.transform.position;
					cameraHolderObject.transform.rotation = Quaternion.Inverse(firstCamera.transform.rotation);	
				}
				
			}
			else if(hasInitialized > 0)
			{
				hasInitialized--;
			}
		}
	}
}
