using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Gui3D
{
	public class Gui3DMenu : Gui3DObject 
	{
		public bool UseMenuIndex = false;
		public List<GameObject> MenuObjects;
		
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
					if((child.gameObject.GetComponent<Gui3DObject>()).Selectable == true)
					{
						MenuObjects.Add(child.gameObject);
					}
				}
			}
			MenuObjects.Sort(Gui3DMenuUtils.LocationCompare);
		}
	}
}
