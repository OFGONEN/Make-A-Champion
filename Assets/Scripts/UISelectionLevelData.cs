using UnityEngine;

namespace FFStudio
{
	[CreateAssetMenu( fileName = "SelectionLevelData", menuName = "FF/Data/SelectionLevelData" )]
	public class UISelectionLevelData : LevelData
	{
		public GameObject[] gameObjectToSelect;
	}
}