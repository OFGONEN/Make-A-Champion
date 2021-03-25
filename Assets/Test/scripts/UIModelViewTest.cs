using UnityEngine;
using FFStudio;

public class UIModelViewTest : MonoBehaviour
{
	public CurrentLevelData currentLevelData;
	public ModelViewer[] viewers;

	private void Start()
	{
		for( var i = 0; i < 3; i++ )
		{
			viewers[ i ].CreateModels( i );
		}

		var levelData = currentLevelData.levelData as UISelectionLevelData;
		viewers[ 3 ].CreateModels( levelData.unlockedObject );
	}
}
