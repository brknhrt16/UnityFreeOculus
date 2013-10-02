using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Gui3D.DrawToScreen))]
public class DrawToScreenEditor : Editor
{
	public override void OnInspectorGUI()
	{
		this.DrawDefaultInspector();
		EditorGUILayout.HelpBox("Set your Texture Type to \"Cursor\" type or set Size values to powers of 2", MessageType.Info);
	}
}
