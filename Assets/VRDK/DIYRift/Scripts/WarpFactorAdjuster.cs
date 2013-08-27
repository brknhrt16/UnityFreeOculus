/************************************************************
 *                                                          *
 *      VRDK - The Virtual-Reality Development Kit          *
 *                                                          *
 *      Package: Do-it-yourself Rift Kit Camera 1.0         *
 *                                                          *
 *      This package is offered free and open source.       *
 *      Please support us by 'liking' us on facebook at     *
 *      http://www.facebook.com/VRcade                      *
 *                                                          *
 *      Author: Dave Ruddell (dave@vrcade.com)              *
 *              Mind-Games Intermedia, LLC / VRcade         *
 *                                                          *
 ************************************************************/

using UnityEngine;
using System.Collections;

public class WarpFactorAdjuster : MonoBehaviour
{
	public Material StereoBarrelWarpMaterial;
	
	public KeyCode BarrelUp = KeyCode.Minus;
	public KeyCode BarrelDown = KeyCode.Equals;
	public float BarrelFactorDelta = 0.1f;
	
	public KeyCode WarpUp = KeyCode.LeftBracket;
	public KeyCode WarpDown = KeyCode.RightBracket;
	public float WarpFactorDelta = 2.0f;
	
	// Use this for initialization
	void Start()
	{
		if (StereoBarrelWarpMaterial == null) {
			Debug.LogError("Cannot run without Material assigned to StereoBarrelWarpMaterial.");
		}
	}
	
	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(BarrelUp) && !Input.GetKeyDown(BarrelDown)) {
			StereoBarrelWarpMaterial.SetFloat("_BarrelFactor", 
				StereoBarrelWarpMaterial.GetFloat("_BarrelFactor") + BarrelFactorDelta);
		}
		else if (Input.GetKeyDown(BarrelDown) && !Input.GetKeyDown(BarrelUp)) {
			StereoBarrelWarpMaterial.SetFloat("_BarrelFactor", 
				StereoBarrelWarpMaterial.GetFloat("_BarrelFactor") - BarrelFactorDelta);
		}
		
		if (Input.GetKeyDown(WarpUp) && !Input.GetKeyDown(WarpDown)) {
			StereoBarrelWarpMaterial.SetFloat("_WarpFactor", 
				StereoBarrelWarpMaterial.GetFloat("_WarpFactor") - WarpFactorDelta);
		}
		else if (Input.GetKeyDown(WarpDown) && !Input.GetKeyDown(WarpUp)) {
			StereoBarrelWarpMaterial.SetFloat("_WarpFactor", 
				StereoBarrelWarpMaterial.GetFloat("_WarpFactor") - WarpFactorDelta);
		}
	
	}
}
