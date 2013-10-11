using UnityEngine;
using System.Collections;

public class UfobCamera : MonoBehaviour 
{
	private bool HmdMode = true;
	public bool HeadTrack = true;
	public bool OverrideToHmdMode = false;
	public GameObject [] HmdObjects;
	public GameObject [] NonHmdObjects;
	
	// Use this for initialization
	void Awake () 
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
		if(gameObject.GetComponent<TrackOculus>() != null)
		{
			if(HeadTrack)
			{
				gameObject.GetComponent<TrackOculus>().enabled = true;
			}
			else
			{
				gameObject.GetComponent<TrackOculus>().enabled = false;
			}
		}
	}
	
	void GetHmdMode()
	{
		HmdMode = PlayerPrefs.GetInt("HmdMode", 0) == 1 ? true : false;
		string [] args = System.Environment.GetCommandLineArgs();
		foreach(string arg in args)
		{
			if(arg == "-hmd")
			{
				HmdMode = true;
			}
			if(arg == "off")
			{
				HmdMode = false;
			}
		}
		PlayerPrefs.SetInt("HmdMode", HmdMode ? 1 : 0);
		PlayerPrefs.Save();
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
	
	/*Vector3 ScreenToWorldPoint(Vector3 position)
	{
		this.GetGui3D();
	}*/
	
	// Update is called once per frame
	void Update () 
	{
		if(gameObject.GetComponent<TrackOculus>() != null)
		{
			if(HeadTrack)
			{
				gameObject.GetComponent<TrackOculus>().enabled = true;
			}
			else
			{
				gameObject.GetComponent<TrackOculus>().enabled = false;
			}
		}
	}
	
	
}
