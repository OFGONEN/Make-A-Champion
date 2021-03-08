/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using DG.Tweening;
using NaughtyAttributes;

public class BagPackingLevelManager : MonoBehaviour
{
	#region Fields
	[Header( "Event Listeners" )]
	public EventListenerDelegateResponse levelRevealedListener;
	public EventListenerDelegateResponse itemAcquiredListener;

	[Header( "Fired Items" )]
	public GameEvent levelComplete;

	[Header( "Shared Variables" )]
	public CurrentLevelData currentLevel;
	public SharedFloatPropertyTweener levelProgress;

	//Private Fields
	private Camera mainCamera;
	private int packedItem;

	// Static Fields
	public static int packableItemCount;
	#endregion

	#region Unity API
	private void OnEnable()
	{
		levelRevealedListener.OnEnable();
		itemAcquiredListener.OnEnable();
	}

	private void OnDisable()
	{
		levelRevealedListener.OnDisable();
		itemAcquiredListener.OnDisable();
	}

	private void Awake()
	{
		levelRevealedListener.response = LevelRevealedResponse;
		itemAcquiredListener.response = ItemAquiredResponse;

		packableItemCount = 0;

		mainCamera = Camera.main;

		mainCamera.transform.position = currentLevel.levelData.cameraStartPosition;
		mainCamera.transform.rotation = Quaternion.Euler( currentLevel.levelData.cameraStartRotation );
	}
	#endregion

	#region API
	#endregion

	#region Implementation
	void ItemAquiredResponse()
	{
		packedItem++;

		levelProgress.Value = packedItem / ( float )packableItemCount;

		if( packedItem >= packableItemCount )
		{
			FFLogger.Log( "Level Complete" );
			levelComplete.Raise();
		}
	}

	void LevelRevealedResponse()
	{
		mainCamera.transform.DOMove( currentLevel.levelData.cameraEndPosition, 0.5f );
		mainCamera.transform.DORotate( currentLevel.levelData.cameraEndRotation, 0.5f );
	}
	#endregion
}