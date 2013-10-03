using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Gui3D
{
	public class Gui3DMenu : Gui3DObject 
	{
		public bool UseMenuIndex = false;
		public List<GameObject> MenuObjects;
		
		public string NextButton = "MenuNext";
		public string PreviousButton = "MenuPrevious";
		
		public bool Wrap = true;
		
		public bool UseMouse = true;
		
		private int SelectedIndex = 0;
		
		public bool Locked = false;
		
		// Use this for initialization
		void Start () 
		{
			if(UseMenuIndex)
			{
				FillMenuByIndex();
			}
			else if(MenuObjects.Count <= 0)
			{
				FillMenuAutomatically();
			}
		}
		
		// Update is called once per frame
		void Update () 
		{
			if((MenuObjects.Count <= 0) || Locked)
			{
				return;
			}
			
			MenuObjects[SelectedIndex].GetComponent<Gui3DObject>().Deselect();
				
			if(Input.GetButtonDown(NextButton))
			{
				UpdateIndex(1);
			}
			else if(Input.GetButtonDown(PreviousButton))
			{
				UpdateIndex(-1);
			}
			
			if(UseMouse)
			{
				if (GetGui3D().HoverObject != null)
				{
					GameObject hoverobj = MenuObjects.Find(obj => obj == GetGui3D().HoverObject.gameObject);
					if(hoverobj != null)
					{
						SelectedIndex = MenuObjects.IndexOf(hoverobj);
					}
				}
			}
			
			MenuObjects[SelectedIndex].GetComponent<Gui3DObject>().Select();
			
			
		}
		
		void UpdateIndex(int increment)
		{
			SelectedIndex += increment;
			
			if(SelectedIndex < 0)
			{
				if(Wrap)
				{
					SelectedIndex = MenuObjects.Count - 1;
				}
				else SelectedIndex = 0;
			}
			else if(SelectedIndex > MenuObjects.Count - 1)
			{
				if(Wrap)
				{
					SelectedIndex = 0;
				}
				else SelectedIndex = MenuObjects.Count - 1;
			}
		}
		
		void FillMenuByIndex()
		{
			FillMenuAutomatically();
			MenuObjects.Sort(Gui3DMenuUtils.IndexCompare);
		}
		
		void FillMenuAutomatically()
		{
			MenuObjects = new List<GameObject>();
			foreach(Transform child in transform)
			{
				if(child.gameObject.GetComponent<Gui3DObject>() != null)
				{
					if(child.gameObject.GetComponent<Gui3DObject>().Selectable == true)
					{
						child.gameObject.GetComponent<Gui3DObject>().Deselect();
						MenuObjects.Add(child.gameObject);
					}
				}
			}
			MenuObjects.Sort(Gui3DMenuUtils.LocationCompare);
		}
	}
}
