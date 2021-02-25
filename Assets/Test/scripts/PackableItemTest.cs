using UnityEngine;
using FFStudio;
using DG.Tweening;

public class PackableItemTest : MonoBehaviour
{
	#region Fields
	public SharedReferance targetReferance;
	public PackingTargetSet targetSet;
	public GameSettings gameSettings;

	BoxCollider selectionCollider;
	PackingTarget targetPackingTarget;
	PackingTarget packingTarget;
	float hoverValue;
	TweenCallback lateUpdate;
	TweenCallback onDeSelect;
	Tween movementTween;

	Vector3 startPosition;
	Vector3 startRotation;
	#endregion

	#region UnityAPI
	private void Awake()
	{
		selectionCollider = GetComponent<BoxCollider>();
		lateUpdate = ExtensionMethods.EmptyMethod;

		startPosition = transform.position;
		startRotation = transform.rotation.eulerAngles;

		hoverValue = startPosition.y / 2f;

		onDeSelect = ReturnDefault;
	}
	private void Start()
	{

		targetPackingTarget = targetReferance.sharedValue as PackingTarget;
	}
	private void LateUpdate()
	{
		lateUpdate();
	}
	#endregion

	#region API
	public void OnSelect()
	{
		FFLogger.Log( "Select:" + name );
		movementTween = DOTween.To( () => hoverValue, x => hoverValue = x, 1f, 0.25f );

		transform.DORotate( targetPackingTarget.transform.rotation.eulerAngles, 0.5f );

		lateUpdate += Hover;
		lateUpdate += SearchTarget;
	}
	public void OnDeSelect()
	{
		FFLogger.Log( "DeSelect:" + name );

		lateUpdate = ExtensionMethods.EmptyMethod;
		movementTween.Kill();
		packingTarget.DeSelect();
		onDeSelect();

		hoverValue = startPosition.y / 2f;
	}
	#endregion

	#region Implementation
	void Hover()
	{
		var position = transform.position;
		position.y = hoverValue;

		transform.position = position;
	}
	void SearchTarget()
	{
		float _closestDistance = 1000f;
		int _closestID = 0;

		foreach( var pair in targetSet.itemDictionary )
		{
			var _target = pair.Value;

			var _distance = Vector3.Distance( transform.position, _target.transform.position );

			if( Mathf.Abs( _distance ) <= _closestDistance )
			{
				if( packingTarget != null )
					packingTarget.DeSelect();

				_closestID = _target.GetInstanceID();
				packingTarget = _target;
				_closestDistance = _distance;
			}
		}

		onDeSelect = ReturnDefault;

		if( Mathf.Abs( _closestDistance ) <= gameSettings.packingItemSearchDistance )
		{

			if( _closestID == targetPackingTarget.GetInstanceID() )
			{
				packingTarget.Select( Color.green );
				onDeSelect = GoTarget;
			}
			else
				packingTarget.Select( Color.red );
		}
	}

	void ReturnDefault()
	{
		transform.DOMove( startPosition, 0.5f );
		transform.DORotate( startRotation, 0.5f );
	}
	void GoTarget()
	{
		transform.DOMove( targetPackingTarget.transform.position, 0.5f );
		transform.DORotate( targetPackingTarget.transform.rotation.eulerAngles, 0.5f );
		selectionCollider.enabled = false;
	}
	#endregion
}
