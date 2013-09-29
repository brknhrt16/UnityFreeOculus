using UnityEngine;
using System.Collections;

namespace Gui3D
{
	
	public class ToggleObject : MonoBehaviour 
	{
	
		public GameObject Obj = null;
		
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
			if(Obj != null)
			{
				Obj.SetActive(!Obj.activeSelf);
			}
		}
	}
}
