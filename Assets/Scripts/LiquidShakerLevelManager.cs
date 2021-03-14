/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using DG.Tweening;

public class LiquidShakerLevelManager : MonoBehaviour
{
	#region Fields
	[Header( "Event Listeners" )]
	public EventListenerDelegateResponse levelRevealedListener;
	public EventListenerDelegateResponse liquidValueChangeListener;

	[Header( "Shared Variables" )]
	public SharedFloatPropertyTweener levelProgress;
	public SharedFloatProperty liquidFillPercentage;
	public CurrentLevelData currentLevel;


	//Private Fields
	private Camera mainCamera;
	#endregion

	#region Unity API
	private void OnEnable()
	{
		levelRevealedListener.OnEnable();
		liquidValueChangeListener.OnEnable();
	}
	private void OnDisable()
	{
		levelRevealedListener.OnDisable();
		liquidValueChangeListener.OnDisable();
	}
	private void Awake()
	{
		mainCamera = Camera.main;

		mainCamera.transform.position = currentLevel.levelData.cameraStartPosition;
		mainCamera.transform.rotation = Quaternion.Euler( currentLevel.levelData.cameraStartRotation );

		levelRevealedListener.response = LevelRevealedResponse;
		liquidValueChangeListener.response = LiquidValueChangeResponse;

	}
	#endregion

	#region API
	#endregion

	#region Implementation
	void LiquidValueChangeResponse()
	{
		if( liquidFillPercentage.Value >= 1f )
		{
			// stop filling.
			// can close the bottle.
		}
		else if( liquidFillPercentage.Value >= 0.5f )
		{
			// stop filling
			// make dust particle selectable.
		}

		var fillPercentageValue = Mathf.Min( liquidFillPercentage.Value, 100f );
		var fillPercentage = fillPercentageValue / 200f;

		levelProgress.Value = fillPercentage;
	}

	void LevelRevealedResponse()
	{
		mainCamera.transform.DOMove( currentLevel.levelData.cameraEndPosition, currentLevel.gameSettings.cameraTravelDuration );
		mainCamera.transform.DORotate( currentLevel.levelData.cameraEndRotation, currentLevel.gameSettings.cameraTravelDuration );
	}
	#endregion
}