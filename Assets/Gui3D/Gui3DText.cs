using System;
using UnityEngine;

namespace Gui3D
{
	public class Gui3DText : Gui3DComponent
	{
		public Gui3DText (Rect boundingBox, float depth, string text)
		{
			gameObject = new GameObject("TextField");
			gameObject.AddComponent<TextMesh>();
			gameObject.AddComponent<MeshRenderer>();
			MeshRenderer meshRender = gameObject.GetComponent<MeshRenderer>();
			meshRender.material = (Material)Resources.GetBuiltinResource(typeof(Material), "Arial.ttf");
			gameObject.GetComponent<TextMesh>().text = text;
			Font font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
			gameObject.GetComponent<TextMesh>().font = font;
			gameObject.transform.localPosition = new Vector3(boundingBox.x, boundingBox.y, depth);
			gameObject.transform.localScale = new Vector3(boundingBox.width, boundingBox.height, 1);
			this.boundingBox = boundingBox;
			this.depth = depth;
		}
		
		public Rect boundingBox
		{
			get; //TODO: update gameObject
			set;
		}
		
		public float depth
		{
			get; //TODO: update gameObject
			set;
		}
		
		public GameObject gameObject
		{
			private set;
			get;
		}
	}
}

