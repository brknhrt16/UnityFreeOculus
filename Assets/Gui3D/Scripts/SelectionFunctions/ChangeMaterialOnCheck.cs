using UnityEngine;
using System.Collections;
using System;
using System.Reflection;

namespace Gui3D
{
	public class ChangeMaterialOnCheck : MonoBehaviour 
	{
	
		public GameObject Obj = null;
		public Material SelectedMaterial = null;
		public Material DeselectedMaterial = null;
		
		private bool Checked = false;
			
		// Use this for initialization
		void Start () 
		{
			MeshRenderer mesh = Obj.GetComponent<MeshRenderer>();
			if(mesh != null)
			{
				if(gameObject.GetComponent<Gui3DCheckBox>().Checked)
				{
					mesh.material = SelectedMaterial;
					Checked = true;
				}
				else
				{
					Checked = false;
					mesh.material = DeselectedMaterial;
				}
				
			}
			gameObject.GetComponent<Gui3DCheckBox>().OnPush += new Gui3DButton.OnPushEvent(ToggleMaterial);
		}
		
		
		void ToggleMaterial()
		{
			Checked = !Checked;
			if((Obj != null) && (SelectedMaterial != null) && (DeselectedMaterial != null))
			{				
				MeshRenderer mesh = Obj.GetComponent<MeshRenderer>();
				if(mesh != null)
				{
					if(Checked)
					{
						mesh.material = SelectedMaterial;
					}
					else
					{
						mesh.material = DeselectedMaterial;
					}
				}
			}
		}
	}
}
