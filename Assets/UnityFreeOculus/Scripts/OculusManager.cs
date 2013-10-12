using UnityEngine;
using System.Collections;

public class OculusManager : MonoBehaviour {
	
	protected static GameObject OculusManagerInstance;
	
	private IEnumerator DelayResetForward()
	{
		yield return new WaitForSeconds(.25f);
		Oculus.ResetForward();
	}
	
	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad(gameObject);
		if (OculusManagerInstance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			OculusManagerInstance = gameObject;
		}
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
