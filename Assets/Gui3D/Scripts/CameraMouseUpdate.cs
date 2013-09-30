using UnityEngine;
using System.Collections;

namespace Gui3D
{
	public class CameraMouseUpdate : MonoBehaviour {
	
		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
		
		}
		
		void OnPreRender()
		{
			Gui3DUtils.GetNearestGui3D(gameObject).Cursor.transform.position = gameObject.transform.position + gameObject.transform.forward.normalized * 1000;
		}
	}
}
