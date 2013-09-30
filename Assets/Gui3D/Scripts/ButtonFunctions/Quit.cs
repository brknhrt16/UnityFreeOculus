using UnityEngine;
using System.Collections;

namespace Gui3D
{
	
	public class Quit : MonoBehaviour 
	{
		
		// Use this for initialization
		void Start () 
		{
			gameObject.GetComponent<Gui3DButton>().OnPush += new Gui3DButton.OnPushEvent(QuitGame);
		}
		
		// Update is called once per frame
		void Update () 
		{
		
		}
		
		void QuitGame()
		{
			Application.Quit();
		}
	}
}
