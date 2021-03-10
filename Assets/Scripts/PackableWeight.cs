/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using DG.Tweening;

public class PackableWeight : MonoBehaviour
{
	#region Fields
	[Header( "Fired Events" )]
	public GameEvent weightPacked;

	[Header( " Shared Variables" )]
	public GameSettings gameSettings;
	public PackingTarget weightTarget;
	public SharedReferance targetReferance;

	//Public Fields
	public float lookAtSpeed;
	[HideInInspector] public BoxCollider selectionCollider;

	//Private Fields
	TweenCallback onLateUpdate;
	TweenCallback onDeSelect;

	Vector3 startPosition;
	Vector3 startRotation;
	#endregion

	#region Unity API
	private void Awake()
	{
		selectionCollider = GetComponent<BoxCollider>();

		startPosition = transform.position;
		startRotation = transform.rotation.eulerAngles;

		onLateUpdate = ExtensionMethods.EmptyMethod;

	}

	private void Start()
	{
		WeightPackingLevelManager.weightCount++;
	}

	private void LateUpdate()
	{
		onLateUpdate();
	}
	#endregion

	#region API
	public void OnSelect()
	{
		onLateUpdate = CheckDistance;
	}

	public void OnDeSelect()
	{
		FFLogger.Log( "DeSelect:" + name );
		onLateUpdate = ExtensionMethods.EmptyMethod;
		selectionCollider.enabled = false;

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

		_position.x = 0;
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
		transform.DOMove( target.position, 0.5f ).OnComplete( weightPacked.Raise );
	}
	#endregion
}