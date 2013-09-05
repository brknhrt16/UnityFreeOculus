using System;
using UnityEngine;

namespace Gui3D
{
	public interface Gui3DComponent
	{
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
			get;
		}
	}
}

