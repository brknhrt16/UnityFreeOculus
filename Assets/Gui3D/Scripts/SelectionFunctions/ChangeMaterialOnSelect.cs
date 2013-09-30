using UnityEngine;
using System.Collections;
using System;
using System.Reflection;

namespace Gui3D
{
	public class ChangeMaterialOnSelect : MonoBehaviour 
	{
	
		public GameObject Obj = null;
		public Material SelectedMaterial = null;
		public Material DeselectedMaterial = null;
		
		
		// Use this for initialization
		void Start () 
		{
			gameObject.GetComponent<Gui3DObject>().OnSelect += new Gui3DObject.OnSelectEvent(Select);
			gameObject.GetComponent<Gui3DObject>().OnDeselect += new Gui3DObject.OnDeselectEvent(Deselect);
		}
		
		// Update is called once per frame
		void Update () 
		{
		
		}
		
		void Select()
		{
			if((Obj != null) && (SelectedMaterial != null))
			{				
				MeshRenderer mesh = Obj.GetComponent<MeshRenderer>();
				if(mesh != null)
				{
					mesh.material = SelectedMaterial;
				}
			}
		}
		void Deselect()
		{
			if((Obj != null) && (DeselectedMaterial != null))
			
			{				
				MeshRenderer mesh = Obj.GetComponent<MeshRenderer>();
				if(mesh != null)
				{
					mesh.material = DeselectedMaterial;
				}
			}
		}
	}
}
