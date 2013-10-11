using UnityEngine;
using System.Collections;

namespace Gui3D
{

	public class Gui3DMenuUtils : MonoBehaviour
	{
		public static int IndexCompare(Gui3DObject obj1, Gui3DObject obj2)
		{
			if((obj1 == null) && (obj2 == null))
			{
				Debug.LogError("Gui3DMenuUtils: Objects are null");
				return 0;
			}
			else if(obj1 == null)
			{
				Debug.LogError("Gui3DMenuUtils: Object1 is null");
				return 1;
			}
			else if(obj2 == null)
			{
				Debug.LogError("Gui3DMenuUtils: Object2 is null");
				return -1;	
			}
			
			if(obj1.MenuIndex < obj2.MenuIndex)
			{
				return -1;
			}
			else if(obj1.MenuIndex > obj2.MenuIndex)
			{
				return 1;
			}
			else return LocationCompare(obj1, obj2);
		}
		
		public static int LocationCompare(Gui3DObject obj1, Gui3DObject obj2)
		{
			if((obj1 == null) && (obj2 == null))
			{
				Debug.LogError("Gui3DMenuUtils: Objects are null");
				return 0;
			}
			else if(obj1 == null)
			{
				Debug.LogError("Gui3DMenuUtils: Object1 is null");
				return 1;
			}
			else if(obj2 == null)
			{
				Debug.LogError("Gui3DMenuUtils: Object2 is null");
				return -1;	
			}
			
			Vector3 object1position = obj1.gameObject.transform.position;
			Vector3 object2position = obj2.gameObject.transform.position;
			
			CompareValue xcompare = Compare(object1position.x, object2position.x);
			CompareValue ycompare = Compare(object1position.y, object2position.y);
			CompareValue zcompare = Compare(object1position.z, object2position.z);
			
			if(ycompare == CompareValue.MORE)
			{
				return -1;
			}
			else if(ycompare == CompareValue.LESS)
			{
				return 1;
			}
			
			if(xcompare == CompareValue.LESS)
			{
				return -1;
			}
			else if(xcompare == CompareValue.MORE)
			{
				return 1;
			}
			
			if(zcompare == CompareValue.LESS)
			{
				return -1;
			}
			else if(zcompare == CompareValue.MORE)
			{
				return 1;
			}
			
			return 0;
		}
		
		public enum CompareValue
		{
			LESS = -1,
			EQUAL = 0,
			MORE = 1
		};
		
		public static CompareValue Compare(float a, float b)
		{
			if(a < b)
			{
				return CompareValue.LESS;
			}
			else if(a > b)
			{
				return CompareValue.MORE;
			}
			else return CompareValue.EQUAL;
		}
		
		
		public static Gui3DMenu GetParentMenu(Transform trans)
		{
			Transform parent = trans.parent;
			if (parent != null)
			{
				Gui3DMenu menu = parent.gameObject.GetComponent<Gui3DMenu>();
				if (menu != null)
				{
					return menu;
				}
				GetParentMenu(parent);
			}
			return null;
		}
	}
}
