using UnityEngine;
using System.Collections;

public class StartOculus : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (!Oculus.Connected)
		{
			Oculus.Connect();
		}
	}
	
	void OnDestroy () {
		if (Oculus.Connected)
		{
			Oculus.Disconnect();
		}
	}
}
