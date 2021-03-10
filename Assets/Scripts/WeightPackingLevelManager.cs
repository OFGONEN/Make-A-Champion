/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using DG.Tweening;

public class WeightPackingLevelManager : MonoBehaviour
{
	#region Fields
	[Header( "Event Listeners" )]
	public EventListenerDelegateResponse levelRevealedListener;
	public EventListenerDelegateResponse weightPackedListener;

	[Header( "Fired Items" )]
	public GameEvent levelComplete;

	[Header( "Shared Variables" )]
	public CurrentLevelData currentLevel;
	public SharedFloatPropertyTweener levelProgress;


	//Public Fields
	public static int weightCount;

	// Private Fields
	private Camera mainCamera;
	private int packedWeightCount;
	#endregion

	#region Unity API
	private void OnEnable()
	{
		levelRevealedListener.OnEnable();
		weightPackedListener.OnEnable();
	}
	private void OnDisable()
	{
		levelRevealedListener.OnDisable();
		weightPackedListener.OnDisable();
	}

	private void Awake()
	{
		levelRevealedListener.response = LevelRevealedResponse;
		weightPackedListener.response = WeightPackedResponse;

		weightCount = 0;

		mainCamera = Camera.main;

		mainCamera.transform.position = currentLevel.levelData.cameraStartPosition;
		mainCamera.transform.rotation = Quaternion.Euler( currentLevel.levelData.cameraStartRotation );
	}
	#endregion

	#region API
	#endregion

	#region Implementation

	void WeightPackedResponse()
	{
		packedWeightCount++;

		levelProgress.Value = packedWeightCount / ( float )weightCount;

		if( packedWeightCount >= weightCount )
		{
			FFLogger.Log( "Level Complete" );
			levelComplete.Raise();
		}
	}

	void LevelRevealedResponse()
	{
		mainCamera.transform.DOMove( currentLevel.levelData.cameraEndPosition, currentLevel.gameSettings.cameraTravelDuration );
		mainCamera.transform.DORotate( currentLevel.levelData.cameraEndRotation, currentLevel.gameSettings.cameraTravelDuration );
	}
	#endregion
}