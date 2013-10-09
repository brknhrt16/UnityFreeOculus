using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Gui3D
{
	public class Gui3DRadioMenu : Gui3DMenu 
	{
		void OnClick()
		{
			print ("CLICKED");
			
			if (GetGui3D().HoverObject != null)
			{
				Gui3DObject hoverobj = MenuObjects.Find(obj => obj.gameObject == GetGui3D().HoverObject.gameObject);
				if(hoverobj != null)
				{
					SelectedIndex = MenuObjects.IndexOf(hoverobj);
				}
				for(int i = 0; i < MenuObjects.Count; i++)
				{
					if(i != SelectedIndex)
					{
						(MenuObjects[i] as Gui3DCheckBox).UnCheck();
					}
					else
					{
						(MenuObjects[i] as Gui3DCheckBox).Check();
					}
				}
			}
			else
			{
				if(!Locked)
				{
					foreach(Gui3DObject obj in MenuObjects)
					{
						if(!obj.Selected)
						{
							(obj as Gui3DCheckBox).UnCheck();
						}
						else
						{
							(obj as Gui3DCheckBox).Check();
						}
					}
				}
			}
		}
		
		override protected void FillMenuAutomatically()
		{
			MenuObjects = new List<Gui3DObject>();
			foreach(Transform child in transform)
			{
				Gui3DCheckBox obj = child.gameObject.GetComponent<Gui3DCheckBox>();
				if(obj != null)
				{
					if(obj.Selectable == true)
					{
						obj.Deselect();
						obj.UnCheck();
						obj.OnPush += new Gui3DButton.OnPushEvent(OnClick);
						MenuObjects.Add(obj);
					}
				}
			}
			MenuObjects.Sort(Gui3DMenuUtils.LocationCompare);
		}
		
	}
}
