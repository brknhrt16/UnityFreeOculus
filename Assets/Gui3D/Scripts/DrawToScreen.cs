using UnityEngine;
using System.Collections;

namespace Gui3D
{
	public class DrawToScreen : MonoBehaviour
	{
		
		public Texture Image = null;
		public bool UseImageSize = true;
		public bool CenterImage = true;
		public Vector2 Size = new Vector2(1f, 1f);
		public Vector2 Offset = new Vector2(0f, 0f);
		
		// Use this for initialization
		void Start () {
			// TODO: Update for changed texture
			if (UseImageSize)
			{
				Size.x = Image.width;
				Size.y = Image.height;
			}
			if (CenterImage)
			{
				Offset.x = -Image.width / 2f;	
				Offset.y = -Image.height / 2f;
			}
		}
		
		// Update is called once per frame
		void Update () {
			
		}
		
		void OnGUI()
		{
			Gui3DObject guiObject = gameObject.GetComponent<Gui3DObject>();
			foreach (Camera camera in guiObject.GetGui3D().GuiCameras)
			{
				Vector3 screenPosition = camera.WorldToScreenPoint(gameObject.transform.position);
				GUI.DrawTexture(new Rect(screenPosition.x + Offset.x, camera.pixelHeight - screenPosition.y + Offset.y, Size.x, Size.y), Image);
			}
		}
	}
}