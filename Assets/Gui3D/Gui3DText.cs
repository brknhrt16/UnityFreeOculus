using System;
using UnityEngine;

namespace Gui3D
{
	public class Gui3DText : Gui3DComponent
	{
		public Gui3DText (Rect boundingBox, float depth, string text)
		{
			this.boundingBox = boundingBox;
			this.depth = depth;
			gameObject = new TextMesh();
			TextMesh textMesh = gameObject as TextMesh;
			textMesh.guiText = text;
			textMesh.transform.localPosition = new Vector3(boundingBox.x, boundingBox.y, depth);
			textMesh.transform.localScale = new Vector3(boundingBox.width, boundingBox.height, 1);
		}
		
		Rect boundingBox
		{
			get;
			set;
		}
		
		float depth
		{
			get;
			set;
		}
		
		GameObject gameObject
		{
			private set;
			get;
		}
	}
}

