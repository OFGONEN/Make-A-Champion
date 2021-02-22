using UnityEngine;
using FFStudio;

public class ModelViewer : MonoBehaviour
{
	#region Fields
	public CurrentLevelData currentLevelData;
	#endregion

	#region UnityAPI
	void Update()
	{
		transform.Rotate( currentLevelData.gameSettings.uiModelViewSpinAxis, currentLevelData.gameSettings.uiModelViewSpinSpeed * Time.deltaTime );
	}
	#endregion

	#region API
	public void CreateModels( int modelIndex )
	{
		var _selectionLevelData = currentLevelData.levelData as UISelectionLevelData;
		var _instance = GameObject.Instantiate( _selectionLevelData.selectionObjects[ modelIndex ].selectionObject, transform );
		_instance.transform.localPosition = _selectionLevelData.selectionObjects[ modelIndex ].position;
		_instance.transform.localScale = _selectionLevelData.selectionObjects[ modelIndex ].scale;
	}

	public void CreateModels( UISelectionLevelData.SelectionObjectData selectionObjectData )
	{
		var _selectionLevelData = currentLevelData.levelData as UISelectionLevelData;
		var _instance = GameObject.Instantiate( selectionObjectData.selectionObject, transform );
		_instance.transform.localPosition = selectionObjectData.position;
		_instance.transform.localScale = selectionObjectData.scale;
	}
	#endregion
}
