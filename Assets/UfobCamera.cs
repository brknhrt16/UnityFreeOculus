using UnityEngine;
using System.Collections;

public class UfobCamera : MonoBehaviour 
{
	private bool HmdMode = true;
	public bool OverrideToHmdMode = false;
	public GameObject [] HmdObjects;
	public GameObject [] NonHmdObjects;
	
	// Use this for initialization
	void Start () 
	{
		if(!OverrideToHmdMode)
		{
			GetHmdMode();
		}
		if(HmdObjects.Length == 0)
		{
			GetHmdObjects();
		}
		if(NonHmdObjects.Length == 0)
		{
			GetNonHmdObjects();
		}
		if(HmdMode)
		{
			Disable(NonHmdObjects);
			Enable(HmdObjects);
		}
		else
		{
			Enable(NonHmdObjects);
			Disable(HmdObjects);
		}
	}
	
	void GetHmdMode()
	{
		HmdMode = false;
		string [] args = System.Environment.GetCommandLineArgs();
		foreach(string arg in args)
		{
			if(arg == "-hmd")
			{
				HmdMode = true;
			}
		}
	}
	
	void GetHmdObjects()
	{
		HmdObjects = new GameObject[1];
		HmdObjects[0] = transform.Find("HmdCamera").gameObject;
	}
	
	void GetNonHmdObjects()
	{
		NonHmdObjects = new GameObject[1];
		NonHmdObjects[0] = transform.Find("NonHmdCamera").gameObject;
	}
	
	void Disable(GameObject [] objects)
	{
		foreach(GameObject o in objects)
		{
			o.SetActive(false);
		}
	}
	
	void Enable(GameObject [] objects)
	{
		foreach(GameObject o in objects)
		{
			o.SetActive(true);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	
}
