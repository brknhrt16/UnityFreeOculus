using UnityEngine;
using System.Collections;

namespace Gui3D
{
	public class Gui3DTextbox : Gui3DButton {
	
		public string Value = "Hello World";
		public GameObject BoundingObject = null;
		
		void FocusInput()
		{
			// TODO: Make utility to find parent menu
			Gui3DMenu menu = gameObject.transform.parent.transform.gameObject.GetComponent<Gui3DMenu>();
			Focused = !Focused;
			menu.Locked = Focused;
		}
		
		float GetGlobalXScale(Transform trans)
		{
			float scale = 1;
			while (trans != null)
			{
				scale *= trans.localScale.x;
				print ("hi" + trans.localScale.x);
				trans = trans.parent;
			}
			return scale;
		}
		
		void ShowValue()
		{
			if (BoundingObject)
			{
				float size = 0;
				int numChars = 0;
				BoundingObject.GetComponent<MeshFilter>().sharedMesh.RecalculateBounds();
				float boundingSize = BoundingObject.GetComponent<MeshFilter>().sharedMesh.bounds.size.x * BoundingObject.transform.localScale.x;
				TextMesh textMesh = gameObject.GetComponentInChildren<TextMesh>();
				string text = "";
				//print(charInfo.width * textMesh.transform.localScale.x);
				//print (textMesh.transform.);
				while (numChars < Value.Length)
				{
					CharacterInfo charInfo;
					textMesh.font.GetCharacterInfo(Value[Value.Length - (numChars + 1)], out charInfo, textMesh.fontSize);
					float charWidth = charInfo.width * textMesh.gameObject.transform.localScale.x * .1f;
					if (charWidth + size > boundingSize)
					{
						break;
					}
					text = Value[Value.Length - (numChars + 1)] + text;
					size += charWidth;
					numChars++;
					//BoundingObject.GetComponent<MeshFilter>().mesh.bounds;
				}
				gameObject.GetComponentInChildren<TextMesh>().text = text;
			}
		}
		
		protected override void Start()
		{
	        base.Start();
			OnPush += new OnPushEvent(FocusInput);
			gameObject.GetComponentInChildren<TextMesh>().text = Value;
			ShowValue();
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