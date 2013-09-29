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
			if((Obj != null) && (ComponentName != ""))
			{				
				MonoBehaviour component = Obj.GetComponent(ComponentName) as MonoBehaviour;
				
				if(component != null)
				{
					component.enabled = !component.enabled;
				}
			}
		}
	}
}
