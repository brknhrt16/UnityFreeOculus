using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

namespace Gui3D
{
	public class Gui3D : MonoBehaviour
	{
	    public GameObject Cursor;
		public UfobCamera GuiCamera;
		
		public Gui3DObject HoverObject
		{
			private set;
			get;
		}
		
		void Update()
		{
			// Ray
			Ray ray = new Ray(GuiCamera.gameObject.transform.position, Cursor.transform.position);
			
			// Raycast Hit
			RaycastHit hit;
			
			if (Physics.Raycast(ray, out hit, 1000))
			{
				HoverObject = hit.transform.gameObject.GetComponent<Gui3DObject>();
			}
			else
			{
				HoverObject = null;
			}
		}
	}
}
