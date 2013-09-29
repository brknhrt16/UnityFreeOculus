using UnityEngine;
using System.Collections;

namespace Gui3D
{
	public class Gui3DUtils : MonoBehaviour 
	{
		public static Gui3D GetNearestGui3D(GameObject obj)
		{
			Gui3D Gui = obj.transform.root.GetComponent<Gui3D>();
			if(Gui == null)
			{
				Gui = obj.transform.root.GetComponentInChildren<Gui3D>();
			}
			if(Gui == null)
			{
				Debug.LogWarning("Gui3D not a parent of " + obj.transform.gameObject.name);
				GameObject.Find("Gui3D").GetComponent<Gui3D>();
			}
			
			if(Gui == null)
			{
				Debug.LogError("Could not find a Gui3D");
			}
			
			return Gui;
		}
	}
}