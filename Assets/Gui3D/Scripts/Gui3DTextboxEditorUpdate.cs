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
			
			GameObject boundingObject = gameObject.GetComponent<Gui3DTextbox>().BoundingObject;
			if (boundingObject)
			{
				float size = 0;
				int numChars = 0;
				boundingObject.GetComponent<MeshFilter>().sharedMesh.RecalculateBounds();
				float boundingSize = boundingObject.GetComponent<MeshFilter>().sharedMesh.bounds.size.x;
				//print(boundingSize);
				TextMesh textMesh = gameObject.GetComponentInChildren<TextMesh>();
				CharacterInfo charInfo = textMesh.font.characterInfo[0];
				//print (charInfo.width);
				//print(charInfo.width * textMesh.transform.localScale.x);
				//print (textMesh.transform.);
				/*while (textMesh.font.GetCharacterInfo(Value[Value.Length - numChars], charInfo, textMesh.fontSize) && size + charInfo.width * gameObject.transform.localScale.x)
				{
					//BoundingObject.GetComponent<MeshFilter>().mesh.bounds;
				}*/
				gameObject.GetComponentInChildren<TextMesh>().text = gameObject.GetComponent<Gui3DTextbox>().Value;
			}
		}
	}
}