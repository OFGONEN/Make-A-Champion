using System;
using UnityEngine;

namespace FFStudio
{
	[CreateAssetMenu( fileName = "SelectionLevelData", menuName = "FF/Data/SelectionLevelData" )]
	public class UISelectionLevelData : LevelData
	{
		public SelectionObjectData[] selectionObjects;

		[Serializable]
		public struct SelectionObjectData
		{
			public GameObject selectionObject;
			public Vector3 position;
			public Vector3 scale;
		}
	}
}