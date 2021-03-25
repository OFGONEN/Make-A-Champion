using UnityEngine;
using NaughtyAttributes;
using System;

namespace FFStudio
{
	[CreateAssetMenu( fileName = "LevelData", menuName = "FF/Data/LevelData" )]
	public class LevelData : ScriptableObject
	{
		[Scene()]
		public int sceneIndex;

		public SelectionObjectData unlockedObject;

		public Vector3 cameraStartPosition;
		public Vector3 cameraStartRotation;
		public Vector3 cameraEndPosition;
		public Vector3 cameraEndRotation;

		[Serializable]
		public struct SelectionObjectData
		{
			public GameObject selectionObject;
			public Vector3 position;
			public Vector3 scale;
		}
	}
}
