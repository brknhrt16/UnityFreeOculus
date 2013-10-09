using UnityEngine;
using System.Collections;

namespace Gui3D
{
	public class Gui3DCheckBox : Gui3DButton 
	{
		public bool Checked = false;
		public bool Toggle = true;
		
		public delegate void OnCheckEvent();
		public event OnCheckEvent OnCheck;
		
		public delegate void OnUnCheckEvent();
		public event OnUnCheckEvent OnUnCheck;
		
		void Start()
		{
			OnPush += new OnPushEvent(OnClick);
		}
		
		void OnClick()
		{
			if(Toggle)
			{
				if(Checked)
				{
					UnCheck();
				}
				else 
				{
					Check();
				}
			}
		}
		
		public void Check()
		{
			Checked = true;
			if(OnCheck != null)
			{
				OnCheck();
			}
		}
		
		public void UnCheck()
		{
			Checked = false;
			if(OnUnCheck != null)
			{
				OnUnCheck();
			}
		}
	}
}
