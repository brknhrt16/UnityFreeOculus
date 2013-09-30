using UnityEngine;
using System.Collections;

namespace Gui3D
{
	public class ChangeText : MonoBehaviour 
	{
		public GameObject Obj = null;
		public string ChangeTo = "Hello World";
		public bool ChangeOnce = false;
		
		private string OriginalMessage = "";
		private bool Changed = false;
		
		// Use this for initialization
		void Start () 
		{
			gameObject.GetComponent<Gui3DButton>().OnPush += new Gui3DButton.OnPushEvent(UpdateMessage);
			if(Obj != null && !ChangeOnce)
			{
				TextMesh text = Obj.GetComponent<TextMesh>();
				if(text != null)
				{
					OriginalMessage = text.text;
				}
			}
		}
		
		// Update is called once per frame
		void Update () 
		{
		
		}
		
		void UpdateMessage()
		{
			if(ChangeOnce && Changed)
			{
				return;
			}
			if((Obj != null))
			{				
				TextMesh text = Obj.GetComponent<TextMesh>();
				
				if(text != null)
				{
					
					if(ChangeOnce)
					{
						text.text = ChangeTo;
						Changed = true;
					}
					else
					{
						if(Changed)
						{
							text.text = OriginalMessage;
						}
						else
						{
							text.text = ChangeTo;
						}
						Changed = !Changed;
					}
				}
			}
		}
	}
}