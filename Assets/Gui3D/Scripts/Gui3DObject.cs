using System;
using UnityEngine;

namespace Gui3D
{
	public class Gui3DObject : MonoBehaviour
	{
		public int MenuIndex = 0;
		public bool Selectable = false;
		public bool Selected = false;
		
		/// <summary>
		/// Gets the Gui3D containing this object.
		/// </summary>
		/// <returns>
		/// The Gui3D object.
		/// </returns>
		public Gui3D GetGui3D()
		{
			// TODO: Allow multiple Gui3Ds by traversing parents?
			return GameObject.Find("Gui3D").GetComponent<Gui3D>();
		}
	}
}

