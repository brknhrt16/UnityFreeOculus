using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

namespace Gui3D
{
	public class Gui3D : MonoBehaviour
	{		
		// Use this for initialization
		void Start ()
		{
			OVRCameraController cameraController = transform.parent.GetComponent<OVRCameraController>();
			float eyeHeight = cameraController.EyeCenterPosition.y;
			float neckHeight = cameraController.NeckPosition.y;
			transform.localPosition = new Vector3(transform.localPosition.x, eyeHeight + neckHeight, transform.localPosition.z);
		}
		
		// Update is called once per frame
		void Update ()
		{
			
		}		
	}
}
