using UnityEngine;
using System.Collections;

public class Gui3DCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.camera.transparencySortMode = TransparencySortMode.Orthographic;
	}
}
