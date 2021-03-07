/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using FFStudio;
using UnityEngine;

public class ModelViewSpace : MonoBehaviour
{
	#region Fields
	public CurrentLevelData currentLevelData;
	public bool defaultViewSpace;
	public ModelViewer[] viewers;


	#endregion

	#region Unity API
	private void Start()
	{
		if( defaultViewSpace )
			DefaultViewSpace();
		else
			SelectionViewSpace();
	}
	#endregion

	#region API
	#endregion

	#region Implementation
	void SelectionViewSpace()
	{
		for( var i = 0; i < 3; i++ )
		{
			viewers[ i ].CreateModels( i );
		}

		var levelData = currentLevelData.levelData as UISelectionLevelData;
		viewers[ 3 ].CreateModels( levelData.unlockedObject );
	}

	void DefaultViewSpace()
	{
		var levelData = currentLevelData.levelData as LevelData;
		viewers[ 3 ].CreateModels( levelData.unlockedObject );
	}
	#endregion
}