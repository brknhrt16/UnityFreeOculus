using UnityEngine;
using System.Collections;

namespace Gui3D
{

	public class Gui3DMenuUtils : MonoBehaviour
	{
		public static int IndexCompare(GameObject object1, GameObject object2)
		{
			if((object1 == null) && (object2 == null))
			{
				Debug.LogError("Gui3DMenuUtils: Objects are null");
				return 0;
			}
			else if(object1 == null)
			{
				Debug.LogError("Gui3DMenuUtils: Object1 is null");
				return 1;
			}
			else if(object2 == null)
			{
				Debug.LogError("Gui3DMenuUtils: Object2 is null");
				return -1;	
			}
			
			Gui3DObject obj1 = object1.GetComponent<Gui3DObject>();
			Gui3DObject obj2 = object2.GetComponent<Gui3DObject>();
			
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
			else return LocationCompare(object1, object2);
		}
		
		public static int LocationCompare(GameObject object1, GameObject object2)
		{
			if((object1 == null) && (object2 == null))
			{
				Debug.LogError("Gui3DMenuUtils: Objects are null");
				return 0;
			}
			else if(object1 == null)
			{
				Debug.LogError("Gui3DMenuUtils: Object1 is null");
				return 1;
			}
			else if(object2 == null)
			{
				Debug.LogError("Gui3DMenuUtils: Object2 is null");
				return -1;	
			}
			
			Vector3 object1position = object1.transform.position;
			Vector3 object2position = object2.transform.position;
			
			CompareValue xcompare = Compare(object1position.x, object2position.x);
			CompareValue ycompare = Compare(object1position.y, object2position.y);
			CompareValue zcompare = Compare(object1position.z, object2position.z);
			print ( "Comparing " + object1.name + " and " + object2.name);
			print(ycompare);
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
		
		
	}
}
