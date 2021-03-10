/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using DG.Tweening;

public class UISelectionLevelManager : MonoBehaviour
{
	#region Fields
	[Header( "Event Listeners" )]
	public EventListenerDelegateResponse levelRevealedListener;
	public EventListenerDelegateResponse selectionCompleteListener;

	public CurrentLevelData currentLevel;
	public SharedFloatPropertyTweener levelProgress;

	private Camera mainCamera;
	#endregion

	#region Unity API
	private void OnEnable()
	{
		levelRevealedListener.OnEnable();
		selectionCompleteListener.OnEnable();
	}

	private void OnDisable()
	{
		levelRevealedListener.OnDisable();
		selectionCompleteListener.OnDisable();
	}

	private void Awake()
	{
		mainCamera = Camera.main;

		levelRevealedListener.response = LevelRevealedResponse;
		selectionCompleteListener.response = SelectionCompleteResponse;

		mainCamera.transform.position = currentLevel.levelData.cameraStartPosition;
		mainCamera.transform.rotation = Quaternion.Euler( currentLevel.levelData.cameraStartRotation );
	}
	#endregion

	#region API
	#endregion

	#region Implementation
	void LevelRevealedResponse()
	{
		// Move Camera
		FFLogger.Log( "Camera Move Position" );

		mainCamera.transform.DOMove( currentLevel.levelData.cameraEndPosition, currentLevel.gameSettings.cameraTravelDuration );
		mainCamera.transform.DORotate( currentLevel.levelData.cameraEndRotation, currentLevel.gameSettings.cameraTravelDuration );
	}

	void SelectionCompleteResponse()
	{
		levelProgress.Value = 1;
	}
	#endregion
}