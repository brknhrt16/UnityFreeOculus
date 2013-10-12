using UnityEngine;
using System.Collections;

public class TrackOculus : MonoBehaviour {

	// Use this for initialization
	void Start () {
		foreach (AddPreRenderEvent eventComponent in gameObject.GetComponentsInChildren<AddPreRenderEvent>())
		{
			eventComponent.OnPreRendered += new AddPreRenderEvent.OnPreRenderEvent(Track);
		}
		foreach (AddPreRenderEvent eventComponent in gameObject.GetComponents<AddPreRenderEvent>())
		{
			eventComponent.OnPreRendered += new AddPreRenderEvent.OnPreRenderEvent(Track);
		}
	}
	
	// Update is called once per frame
	void Track () {
		transform.localRotation = Oculus.GetQuaternion();
	}
}
