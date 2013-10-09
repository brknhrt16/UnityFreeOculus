using UnityEngine;
using System.Collections;

namespace Gui3D
{
	[ExecuteInEditMode]
	public class Gui3DTextboxEditorUpdate : MonoBehaviour
	{
		public bool LeftAlign = false;
		
		void Update()
		{
			if (gameObject.GetComponent<Gui3DTextbox>().BoundingObject && LeftAlign)
			{
				var textMeshTransform = gameObject.GetComponentInChildren<TextMesh>().gameObject.transform;
				textMeshTransform.localPosition = new Vector3(gameObject.GetComponent<Gui3DTextbox>().BoundingObject.GetComponent<MeshFilter>().sharedMesh.bounds.min.x, textMeshTransform.localPosition.y, textMeshTransform.localPosition.z);
			}
			
			gameObject.GetComponentInChildren<TextMesh>().gameObject.transform.localScale = new Vector3(
				gameObject.GetComponentInChildren<TextMesh>().gameObject.transform.localScale.y * gameObject.transform.localScale.y / gameObject.transform.localScale.x,
				gameObject.GetComponentInChildren<TextMesh>().gameObject.transform.localScale.y,
				gameObject.GetComponentInChildren<TextMesh>().gameObject.transform.localScale.z);
		}
	}
}