/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FFStudio
{
	[CreateAssetMenu( fileName = "SharedColorProperty", menuName = "FF/Data/Shared/ColorProperty" )]
	public class SharedColorProperty : ScriptableObject
	{
		public ColorGameEvent changeEvent;
		private Color sharedValue;
		public Color Value
		{
			get
			{
				return sharedValue;
			}
			set
			{
				sharedValue = value;

				changeEvent.eventValue = value;
				changeEvent.Raise();
			}
		}
	}
}
