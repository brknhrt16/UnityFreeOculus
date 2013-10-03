using UnityEngine;
using System.Collections;

namespace Gui3D
{
	public class Gui3DTextbox : Gui3DButton {
	
		public string Value = "Hello World";
		public bool Focused = false;
		
		void FocusInput()
		{
			// TODO: Make utility to find parent menu
			Gui3DMenu menu = gameObject.transform.parent.transform.gameObject.GetComponent<Gui3DMenu>();
			Focused = !Focused;
			menu.Locked = Focused;
		}
		
		void Start()
		{
	        base.Start();
			OnPush += new OnPushEvent(FocusInput);
			gameObject.GetComponentInChildren<TextMesh>().text = Value;
	    }
		
		protected override void Update()
		{
			base.Update();
			if (Selected && Focused)
			{
		        foreach (char c in Input.inputString)
				{
		            if (c == "\b"[0])
					{
		                if (Value.Length != 0)
						{
		                    Value = Value.Substring(0, Value.Length - 1);
						}
					}  
		            else
					{
		                if (c == "\n"[0] || c == "\r"[0])
						{
		                    //print("User entered his name: " + guiText.text);
						}
		                else
						{
		                    Value += c;
						}
					}
		        }
				gameObject.GetComponentInChildren<TextMesh>().text = Value;
			}
	    }
	}
}