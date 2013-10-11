using UnityEngine;
using System.Collections;

public class AddPreRenderEvent : MonoBehaviour {

	public delegate void OnPreRenderEvent();
	public event OnPreRenderEvent OnPreRendered;
	
	void OnPreRender()
	{
		if (OnPreRendered != null)
		{
			OnPreRendered();
		}
	}
		
}
