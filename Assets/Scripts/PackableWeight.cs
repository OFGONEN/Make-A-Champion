/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using DG.Tweening;
using Lean.Touch;

public class PackableWeight : MonoBehaviour
{
	#region Fields
	[Header( "Event Listener" )]
	public EventListenerDelegateResponse failAnimationListener;
	[Header( "Fired Events" )]
	public IntGameEvent weightPacked;

	[Header( " Shared Variables" )]
	public GameSettings gameSettings;
	public WeightTarget weightTarget;
	public SharedReferance targetReferance;

	//Public Fields
	public int weightIndex;
	public float lookAtSpeed;
	[HideInInspector] public BoxCollider selectionCollider;

	//Private Fields
	LeanDragTranslate leanDragTranslate;
	TweenCallback onLateUpdate;
	TweenCallback onDeSelect;

	Vector3 startPosition;
	Vector3 startRotation;
	#endregion

	#region Unity API
	private void OnEnable()
	{
		failAnimationListener.OnEnable();
	}

	private void OnDisable()
	{
		failAnimationListener.OnDisable();
	}
	private void Awake()
	{
		failAnimationListener.response = () =>
		{
			selectionCollider.enabled = false;
		};

		selectionCollider = GetComponent<BoxCollider>();
		leanDragTranslate = GetComponent<LeanDragTranslate>();

		startPosition = transform.position;
		startRotation = transform.rotation.eulerAngles;

		onLateUpdate = ExtensionMethods.EmptyMethod;

		leanDragTranslate.enabled = false;
	}

	private void LateUpdate()
	{
		onLateUpdate();
	}
	#endregion

	#region API
	public void OnSelect()
	{
		leanDragTranslate.enabled = true;
		onLateUpdate = CheckDistance;
	}

	public void OnDeSelect()
	{
		FFLogger.Log( "DeSelect:" + name );
		onLateUpdate = ExtensionMethods.EmptyMethod;
		selectionCollider.enabled = false;
		leanDragTranslate.enabled = false;

		weightTarget.DeSelect();
		onDeSelect();
	}
	#endregion

	#region Implementation
	void CheckDistance()
	{
		var _distance = Vector3.Distance( transform.position, weightTarget.transform.position );

		if( Mathf.Abs( _distance ) <= gameSettings.packingWeightSearchDistance )
		{
			transform.LookAtOverTime( weightTarget.transform.position, lookAtSpeed );
			weightTarget.Select( Color.green );
			onDeSelect = GoWeightTarget;
			selectionCollider.enabled = false;
		}
		else
		{
			transform.LookAtDirectionOverTime( Vector3.up, lookAtSpeed );
			weightTarget.DeSelect();
			onDeSelect = ReturnDefault;
		}

		var _position = transform.position;

		_position.x = startPosition.x;
		transform.position = _position;
	}

	void ReturnDefault()
	{
		transform.DOMove( startPosition, 0.25f ).OnComplete( () => selectionCollider.enabled = true );
		transform.DORotate( startRotation, 0.25f );
	}

	void GoWeightTarget()
	{
		transform.DOMove( weightTarget.transform.position, 0.5f ).OnComplete( GoTarget );
		transform.DORotate( Vector3.zero, 0.5f );
	}

	void GoTarget()
	{
		var target = targetReferance.sharedValue as Transform;
		transform.DOMove( target.position, 0.5f ).OnComplete( () =>
		{
			weightPacked.eventValue = weightIndex;
			weightPacked.Raise();

			transform.SetParent( target.parent );
		} );
	}
	#endregion
}