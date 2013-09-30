using UnityEngine;
using System.Collections;

namespace Gui3D
{
	
	public class RestartGame : MonoBehaviour 
	{
		
		// Use this for initialization
		void Start () 
		{
			gameObject.GetComponent<Gui3DButton>().OnPush += new Gui3DButton.OnPushEvent(Restart);
		}
		
		// Update is called once per frame
		void Update () 
		{
		
		}
		
		void Restart()
		{
			Application.LoadLevel(0);
		}
	}
}
