using UnityEngine;
using NaughtyAttributes;

namespace FFStudio
{
	[CreateAssetMenu( fileName = "LevelData", menuName = "FF/Data/LevelData" )]
    public class LevelData : ScriptableObject
    {
		[Scene()]
		public int sceneIndex;
    }
}
