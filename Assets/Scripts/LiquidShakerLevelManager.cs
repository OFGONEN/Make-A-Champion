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
	public EventListenerDelegateResponse shakerPositionDiffListener;
	public EventListenerDelegateResponse bottleCapClosedListener;

	[Header( "Fired Events" )]
	public GameEvent shakeBottle; // Event that enables shaking the bottle
	public GameEvent levelComplete;
	public GameEvent stopFillingWater;
	public GameEvent stopFillingPowder;
	public GameEvent enableSelectingWater;
	public GameEvent enableSelectingPower;
	public GameEvent closeBottleLid; // Event that enables closing the lid of the bottle

	[Header( "Shared Variables" )]
	public SharedFloatPropertyTweener levelProgress;
	public SharedFloatProperty liquidFillPercentage;
	public CurrentLevelData currentLevel;

	[Header( "Public Variables" )]
	public BoxCollider bottleCapSelectionCollider;
	public Transform bottleCapParent;

	//Private Fields
	private Camera mainCamera;
	#endregion

	#region Unity API
	private void OnEnable()
	{
		levelRevealedListener.OnEnable();
		liquidValueChangeListener.OnEnable();
		shakerPositionDiffListener.OnEnable();
		bottleCapClosedListener.OnEnable();
	}

	private void OnDisable()
	{
		levelRevealedListener.OnDisable();
		liquidValueChangeListener.OnDisable();
		shakerPositionDiffListener.OnDisable();
		bottleCapClosedListener.OnDisable();
	}

	private void Awake()
	{
		mainCamera = Camera.main;

		mainCamera.transform.position = currentLevel.levelData.cameraStartPosition;
		mainCamera.transform.rotation = Quaternion.Euler( currentLevel.levelData.cameraStartRotation );

		levelRevealedListener.response = LevelRevealedResponse;
		liquidValueChangeListener.response = LiquidValueChangeResponse;
		shakerPositionDiffListener.response = ShakerPositionDiffResponse;
		bottleCapClosedListener.response = () => bottleCapSelectionCollider.transform.SetParent( bottleCapParent );
	}
	#endregion

	#region API
	#endregion

	#region Implementation
	void LiquidValueChangeResponse()
	{
		var fillPercentageValue = Mathf.Min( liquidFillPercentage.Value, 100f );
		var fillPercentage = fillPercentageValue / 200f;


		levelProgress.Value = fillPercentage;

		if( levelProgress.Value >= 0.48f )
		{
			FFLogger.Log( "Stop Filling x2" );
			closeBottleLid.Raise(); // change with can close bottle
			bottleCapSelectionCollider.enabled = true;
			stopFillingPowder.Raise();
		}
		else if( levelProgress.Value >= 0.25f )
		{
			FFLogger.Log( "Stop Filling" );
			stopFillingWater.Raise();
			enableSelectingPower.Raise();
		}
	}

	void LevelRevealedResponse()
	{
		mainCamera.transform.DOMove( currentLevel.levelData.cameraEndPosition, currentLevel.gameSettings.cameraTravelDuration );
		mainCamera.transform.DORotate( currentLevel.levelData.cameraEndRotation, currentLevel.gameSettings.cameraTravelDuration );

		enableSelectingWater.Raise();
	}

	void ShakerPositionDiffResponse()
	{
		var changeEvent = shakerPositionDiffListener.gameEvent as FloatGameEvent;
		levelProgress.Value += changeEvent.eventValue * currentLevel.gameSettings.shakerSpeed;

		// When value is reached 1 level complete;
		if( levelProgress.Value >= 1 )
			levelComplete.Raise();
	}
	#endregion
}