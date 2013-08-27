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

using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(DIYRiftCamera))]
public class DIYRiftCameraEditor : Editor
{
	private DIYRiftCamera _riftCamera;
	void OnEnable()
	{
		_riftCamera = target as DIYRiftCamera;
	}
	
	public override void OnInspectorGUI()
	{
		EditorGUIUtility.LookLikeControls(110,30);
		
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.BeginVertical("box");
		
		// Display Interaxial Distance
		_riftCamera.InteraxialDist = (float)EditorGUILayout.IntSlider(
			new GUIContent("Interaxial (mm)",
				"Distance (in millimeters) between eyes/lenses."),
			(int)_riftCamera.InteraxialDist, 45, 80);
		
		// Display Zero Prlx Distance
	    _riftCamera.ZeroPrlxDist = EditorGUILayout.Slider(
			new GUIContent("Zero Prlx Dist (M)",
				"Distance (in meters) at which left and right images overlap exactly."),
			_riftCamera.ZeroPrlxDist, 0.1f, 100.0f);
		
		// Display Toed-In Option
	    _riftCamera.ToedIn = EditorGUILayout.Toggle(
			new GUIContent("Toed-In ",
				"Angle cameras inward to converge."),
			_riftCamera.ToedIn,
			GUILayout.MaxWidth(120));
		
		// Display Cross-eyed Option
	 	_riftCamera.SwapLeftRight = EditorGUILayout.Toggle(
			new GUIContent("Cross-eyed",
				"Swap Left and Right cameras for cross-eyed viewing."),
			_riftCamera.SwapLeftRight,
			GUILayout.MaxWidth(120));
		
		// Display Horizontal Image Transform
		_riftCamera.HIT = EditorGUILayout.Slider(
			new GUIContent("H I T",
				"Horizontal Image Transform (default 0)"),
			_riftCamera.HIT, -5.0f, 5.0f);
		
	 	EditorGUILayout.EndVertical();
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.BeginVertical("box");
		
		// Display Render Order
		_riftCamera.RenderOrderDepth = EditorGUILayout.IntSlider(
			new GUIContent("Render Order",
				"Increment this for each new 3D Camera in the scene. (default 0)"),
			_riftCamera.RenderOrderDepth, 0, 10);
		
		// Construct Layers popup
		GUIContent[] layers = new GUIContent[30];
		for (int i = 0; i < layers.Length; ++i) {
			string layername = LayerMask.LayerToName(i);
			if (System.String.IsNullOrEmpty(layername)) {
				layers[i] = new GUIContent(i.ToString(), layername);
			} else {
				layers[i] = new GUIContent(i.ToString() + " (" + layername + ")", layername);
			}
		}
		
		// Display Layers Popup
		_riftCamera.guiOnlyLayer = EditorGUILayout.Popup(
			new GUIContent("GUI Layer",
				"Set this to the layer that all 'floating' GUI items live on."),
			_riftCamera.guiOnlyLayer, layers);
		
     	EditorGUILayout.EndVertical();
		EditorGUILayout.EndHorizontal();
		
		EditorGUIUtility.LookLikeControls(210,30);
		
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.BeginVertical("box");
		
		// Display Stereo Shader option
		_riftCamera.UseStereoShader = EditorGUILayout.Toggle(
			new GUIContent("Use Barrel Warp Shader (Pro-only)", 
				"Enable to handle barreling and warping effect required by the 'Rift'. Sorry, Unity Pro required."),
			_riftCamera.UseStereoShader,
			GUILayout.MaxWidth(220));
		
		// Display Barrel Warp Material
		_riftCamera.StereoBarrelWarpMaterial = EditorGUILayout.ObjectField(
			new GUIContent("Barrel Warp Material",
				"Assign material that uses the Barrel Warp shader."),
			_riftCamera.StereoBarrelWarpMaterial,
			typeof(Material), true) as Material;
		
		EditorGUILayout.EndVertical();
		EditorGUILayout.EndHorizontal();
		
		if (GUI.changed) {
             EditorUtility.SetDirty(_riftCamera);
    	}
	}
}