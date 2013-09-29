using UnityEngine;
using System.Collections;

namespace Gui3D
{
	public class DebugButton : MonoBehaviour {
	
		public string Message = "Hello World";
		
		// Use this for initialization
		void Start () {
			gameObject.GetComponent<Gui3DButton>().OnPush += new Gui3DButton.OnPushEvent(PrintMessage);
		}
		
		// Update is called once per frame
		void Update () {
		
		}
		
		void PrintMessage()
		{
			print(Message);
		}
	}
}