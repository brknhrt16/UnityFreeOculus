using UnityEngine;
using System.Collections;

namespace Gui3D
{
	public class Gui3DCheckBox : Gui3DButton 
	{
		public bool Checked = false;
		
		void Start()
		{
			OnPush += new OnPushEvent(OnCheck);
		}
		
		void OnCheck()
		{
			Checked = !Checked;
		}
	}
}
