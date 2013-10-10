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
				Offset.x = -Size.x / 2f;	
				Offset.y = -Size.y / 2f;
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
				if(camera.enabled)
				{
					Vector3 screenPosition = camera.WorldToScreenPoint(gameObject.transform.position);
					
					Rect imageRect = new Rect(screenPosition.x + Offset.x, camera.pixelHeight - screenPosition.y + Offset.y, Size.x, Size.y);
					Rect clampedRect = imageRect;
					if (camera.pixelRect.xMin > clampedRect.xMin)
					{
						clampedRect.xMin = camera.pixelRect.xMin;
					}
					if (camera.pixelRect.yMin > clampedRect.yMin)
					{
						clampedRect.yMin = camera.pixelRect.yMin;
					}
					if (camera.pixelRect.xMax < clampedRect.xMax)
					{
						clampedRect.xMax = camera.pixelRect.xMax;
					}
					if (camera.pixelRect.yMax < clampedRect.yMax)
					{
						clampedRect.yMax = camera.pixelRect.yMax;
					}
					Rect texCoordRect = new Rect(
						(clampedRect.xMin - imageRect.xMin) / imageRect.width,
						(imageRect.yMax - clampedRect.yMax) / imageRect.height,
						clampedRect.width / imageRect.width,
						clampedRect.height / imageRect.height);
					
					if (clampedRect.width > 0 && clampedRect.height > 0)
					{
						GUI.DrawTextureWithTexCoords(clampedRect, Image, texCoordRect);
					}
				}
			}
		}
	}
}