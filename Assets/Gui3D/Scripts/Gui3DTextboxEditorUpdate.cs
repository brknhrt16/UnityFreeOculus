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
				var mesh = gameObject.GetComponent<Gui3DTextbox>().BoundingObject.GetComponent<MeshFilter>();
				textMeshTransform.localPosition = new Vector3(mesh.sharedMesh.bounds.min.x * mesh.transform.localScale.x + mesh.transform.position.x, textMeshTransform.localPosition.y, textMeshTransform.localPosition.z);
			}
			
			gameObject.GetComponentInChildren<TextMesh>().gameObject.transform.localScale = new Vector3(
				gameObject.GetComponentInChildren<TextMesh>().gameObject.transform.localScale.y * gameObject.transform.localScale.y / gameObject.transform.localScale.x,
				gameObject.GetComponentInChildren<TextMesh>().gameObject.transform.localScale.y,
				gameObject.GetComponentInChildren<TextMesh>().gameObject.transform.localScale.z);
			
			GameObject boundingObject = gameObject.GetComponent<Gui3DTextbox>().BoundingObject;
			if (boundingObject)
			{
				string Value = gameObject.GetComponent<Gui3DTextbox>().Value;
				float size = 0;
				int numChars = 0;
				boundingObject.GetComponent<MeshFilter>().sharedMesh.RecalculateBounds();
				float boundingSize = boundingObject.GetComponent<MeshFilter>().sharedMesh.bounds.size.x * boundingObject.transform.localScale.x;
				TextMesh textMesh = gameObject.GetComponentInChildren<TextMesh>();
				string text = "";
				//print(charInfo.width * textMesh.transform.localScale.x);
				//print (textMesh.transform.);
				while (numChars < Value.Length)
				{
					CharacterInfo charInfo;
					textMesh.font.GetCharacterInfo(Value[Value.Length - (numChars + 1)], out charInfo, textMesh.fontSize);
					float charWidth = charInfo.width * textMesh.gameObject.transform.localScale.x * .1f;
					if (charWidth + size > boundingSize)
					{
						break;
					}
					text = Value[Value.Length - (numChars + 1)] + text;
					size += charWidth;
					numChars++;
					//BoundingObject.GetComponent<MeshFilter>().mesh.bounds;
				}
				gameObject.GetComponentInChildren<TextMesh>().text = text;
			}
		}
	}
}