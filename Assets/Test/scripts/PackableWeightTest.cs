using UnityEngine;
using DG.Tweening;
using FFStudio;

public class PackableWeightTest : MonoBehaviour
{
	#region Fields
	public GameSettings gameSettings;
	public PackingTarget weightTarget;
	public Transform target;
	public float lookAtSpeed;

	TweenCallback onLateUpdate;
	TweenCallback onDeSelect;

	BoxCollider selectionCollider;
	Vector3 startPosition;
	Vector3 startRotation;
	#endregion

	#region UnityAPI
	private void Awake()
	{
		selectionCollider = GetComponent<BoxCollider>();

		startPosition = transform.position;
		startRotation = transform.rotation.eulerAngles;

		onLateUpdate = ExtensionMethods.EmptyMethod;
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

	#region Implemantation
	void CheckDistance()
	{
		var _distance = Vector3.Distance( transform.position, weightTarget.transform.position );

		if( Mathf.Abs( _distance ) <= gameSettings.packingWeightSearchDistance )
		{
			transform.LookAtOverTime( weightTarget.transform.position, lookAtSpeed );
			weightTarget.Select( Color.green );
			onDeSelect = GoWeightTarget;
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
		transform.DOMove( target.position, 0.5f );
	}
	#endregion
}
