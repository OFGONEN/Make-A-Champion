/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using DG.Tweening;
using Lean.Touch;

public class LiquidShaker : MonoBehaviour
{
	#region Fields
	[Header( "Event Listeners" )]
	public EventListenerDelegateResponse closeBottleLidListener;
	public EventListenerDelegateResponse shakeBottleListener;
	public EventListenerDelegateResponse levelCompleteListener;

	[Header( "Fired Events" )]
	public FloatGameEvent positionDiffEvent;

	// Public Fields
	public GameObject bottleCap;

	// private fields
	private TweenCallback update;

	private LeanSelectable leanSelectable;
	private Vector3 lastFramePosition;
	private Vector3 startPosition;
	#endregion

	#region Unity API
	private void OnEnable()
	{
		closeBottleLidListener.OnEnable();
		shakeBottleListener.OnEnable();
		levelCompleteListener.OnEnable();
	}

	private void OnDisable()
	{
		closeBottleLidListener.OnDisable();
		shakeBottleListener.OnDisable();
		levelCompleteListener.OnDisable();
	}

	private void Awake()
	{
		leanSelectable = GetComponent<LeanSelectable>();

		leanSelectable.enabled = false;

		lastFramePosition = transform.position;
		startPosition = lastFramePosition;

		update = ExtensionMethods.EmptyMethod;

		closeBottleLidListener.response = () => bottleCap.SetActive( true );
		shakeBottleListener.response = () =>
		{
			leanSelectable.enabled = true;
			bottleCap.SetActive( false );
		};

		levelCompleteListener.response = LevelCompleteResponse;

	}

	private void Start()
	{
		bottleCap.SetActive( false );
	}

	private void Update()
	{
		update();
	}
	#endregion

	#region API
	public void OnSelect()
	{
		FFLogger.Log( "Shaker Selected" );
		update = CheckPositionDiff;
	}

	public void OnDeselect()
	{
		FFLogger.Log( "Shaker Deselected" );
		update = ExtensionMethods.EmptyMethod;

		transform.DOMove( startPosition, 0.5f );
	}
	#endregion

	#region Implementation
	void CheckPositionDiff()
	{
		var currentPosition = transform.position;
		var diff = Vector3.Distance( currentPosition, lastFramePosition );

		lastFramePosition = currentPosition;

		positionDiffEvent.eventValue = diff;
		positionDiffEvent.Raise();
	}

	void LevelCompleteResponse()
	{
		leanSelectable.enabled = false;

		update = ExtensionMethods.EmptyMethod;
		transform.DOMove( startPosition, 0.5f );
	}
	#endregion
}