using UnityEngine;
using System.Collections;
using System;
using System.Reflection;

namespace Gui3D
{
	public class ToggleComponent : MonoBehaviour 
	{
	
		public GameObject Obj = null;
		public string ComponentName = "";
		
		public bool ChangeOnce = false;
		
		private bool Changed = false;
		
		// Use this for initialization
		void Start () 
		{
			gameObject.GetComponent<Gui3DButton>().OnPush += new Gui3DButton.OnPushEvent(Toggle);
		}
		
		// Update is called once per frame
		void Update () 
		{
		
		}
		
		void Toggle()
		{
			if(ChangeOnce && Changed)
			{
				return;
			}
			if((Obj != null) && (ComponentName != ""))
			{				
				MonoBehaviour component = Obj.GetComponent(ComponentName) as MonoBehaviour;
				
				if(component != null)
				{
					component.enabled = !component.enabled;
					if(ChangeOnce)
					{
						Changed = true;
					}
				}
			}
		}
	}
}
