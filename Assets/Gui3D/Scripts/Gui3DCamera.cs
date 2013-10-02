using UnityEngine;
using System.Collections;

public class Gui3DCamera : MonoBehaviour {
	
	public delegate void OnPreRenderEvent();
	public event OnPreRenderEvent OnPreRendered;
	
	// Use this for initialization
	void Start ()
	{
		gameObject.camera.transparencySortMode = TransparencySortMode.Orthographic;
	}
	
	void OnPreRender()
	{
		if (OnPreRendered != null)
		{
			OnPreRendered();
		}
	}
}
