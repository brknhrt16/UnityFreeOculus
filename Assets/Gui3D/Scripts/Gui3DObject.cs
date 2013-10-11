using System;
using UnityEngine;

namespace Gui3D
{
	public class Gui3DObject : MonoBehaviour
	{
		public int MenuIndex = 0;
		public bool Selectable = false;
		public bool Selected = false;
		public bool Focused = false;
		
		public delegate void OnSelectEvent();
		public event OnSelectEvent OnSelect;
		
		public delegate void OnDeselectEvent();
		public event OnDeselectEvent OnDeselect;
		
		/// <summary>
		/// Gets the Gui3D containing this object.
		/// </summary>
		/// <returns>
		/// The Gui3D object.
		/// </returns>
		public Gui3D GetGui3D()
		{
			Gui3D Gui = transform.root.GetComponent<Gui3D>();
			if(Gui == null)
			{
				Gui = transform.root.GetComponentInChildren<Gui3D>();
			}
			if(Gui == null)
			{
				Debug.LogWarning("Gui3D not a parent of " + transform.gameObject.name);
				GameObject.Find("Gui3D").GetComponent<Gui3D>();
			}
			
			if(Gui == null)
			{
				Debug.LogError("Could not find a Gui3D");
			}
			// TODO: Allow multiple Gui3Ds by traversing parents?
			return Gui;
		}		
		
		public void Select()
		{
			Selected = true;
			if (OnSelect != null)
			{
				OnSelect();
			}
		}
		
		public void Deselect()
		{
			Selected = false;
			if (OnDeselect != null)
			{
				OnDeselect();
			}
		}
	}
}

