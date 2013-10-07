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
		private bool Toggle = true;
			
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
			gameObject.GetComponent<Gui3DCheckBox>().OnCheck += new Gui3DCheckBox.OnCheckEvent(OnCheck);
			gameObject.GetComponent<Gui3DCheckBox>().OnUnCheck += new Gui3DCheckBox.OnUnCheckEvent(OnUnCheck);
		}
		
		void OnCheck()
		{
			if((Obj != null) && (SelectedMaterial != null) && (DeselectedMaterial != null))
			{				
				MeshRenderer mesh = Obj.GetComponent<MeshRenderer>();
				if(mesh != null)
				{
					mesh.material = SelectedMaterial;
				}
			}
		}
		
		void OnUnCheck()
		{
			if((Obj != null) && (SelectedMaterial != null) && (DeselectedMaterial != null))
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
