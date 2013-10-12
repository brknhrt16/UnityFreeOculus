using UnityEngine;
using System.Collections;

public class StartOculus : MonoBehaviour {
	
	private IEnumerator DelayResetForward()
	{
		yield return new WaitForSeconds(.5f);
		Oculus.ResetForward();
	}
	
	// Use this for initialization
	void Start () {
		if (!Oculus.Connected)
		{
			Oculus.Connect();
			StartCoroutine(DelayResetForward());
		}
	}
	
	void OnDestroy () {
		if (Oculus.Connected)
		{
			Oculus.Disconnect();
		}
	}
}
