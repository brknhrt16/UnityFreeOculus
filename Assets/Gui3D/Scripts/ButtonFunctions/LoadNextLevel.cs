using UnityEngine;
using System.Collections;

namespace Gui3D
{
	
	public class LoadNextLevel : MonoBehaviour 
	{		
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
			int NextLevel = Application.loadedLevel+1;
			if(NextLevel >= Application.levelCount)
			{
				return;
			}
			Application.LoadLevel(NextLevel);
		}
	}
}
