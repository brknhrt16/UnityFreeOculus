using UnityEngine;
using System.Collections;

namespace Gui3D
{
	
	public class LoadLevel : MonoBehaviour 
	{
	
		public string LevelName = "";
		
		// Use this for initialization
		void Start () 
		{
			gameObject.GetComponent<Gui3DButton>().OnPush += new Gui3DButton.OnPushEvent(Load);
		}
		
		// Update is called once per frame
		void Update () 
		{
		
		}
		
		void Load()
		{
			if(LevelName == "")
			{
				return;
			}
			Application.LoadLevel(LevelName);
		}
	}
}
