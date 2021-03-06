using UnityEngine;
using NaughtyAttributes;

namespace FFStudio
{
	[CreateAssetMenu( fileName = "LevelData", menuName = "FF/Data/LevelData" )]
	public class LevelData : ScriptableObject
	{
		[Scene()]
		public int sceneIndex;

		public Vector3 cameraStartPosition;
		public Vector3 cameraStartRotation;
		public Vector3 cameraEndPosition;
		public Vector3 cameraEndRotation;
	}
}
