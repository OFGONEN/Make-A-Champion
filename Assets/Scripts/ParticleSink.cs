/* Created by and for usage of FF Studios (2021). */

using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using DG.Tweening;

public class ParticleSink : MonoBehaviour
{
#region Fields
	[ Header( "Modified Shared Data" ) ]
	public SharedFloatProperty liquidFillPercentage;

	[ Header( "Parameters" ) ]
	[ Range( 0.01f, 1.0f ) ] public float particleContributionToLiquidPercentage = 0.5f;

	//Private Fields
	private new ParticleSystem particleSystem;
	private BoxCollider selectionCollider;
	private TweenCallback lateUpdate;
	private Tween movementTween;

	private Vector3 startPosition;
	private Vector3 startRotation;

	private float hoverValue;
	#endregion

	#region Unity API
	private void Awake()
    {
		particleSystem = GetComponent< ParticleSystem >();
		selectionCollider = GetComponentInChildren<BoxCollider>();

		lateUpdate = ExtensionMethods.EmptyMethod;

		startPosition = transform.position;
		startRotation = transform.rotation.eulerAngles;

		hoverValue = startPosition.y;
	}

    private void OnParticleTrigger()
    {
		List< ParticleSystem.Particle > particles = new List< ParticleSystem.Particle >();

		int numberOfEnteringParticles = particleSystem.GetTriggerParticles( ParticleSystemTriggerEventType.Enter, particles );
		liquidFillPercentage.Value += numberOfEnteringParticles * particleContributionToLiquidPercentage;
    }

	private void LateUpdate()
	{
		lateUpdate();
	}
	#endregion

	#region API
	public void OnSelect()
	{
		FFLogger.Log( name + " Selected!" );

		movementTween = DOTween.To( () => hoverValue, x => hoverValue = x, 0.75f, 0.25f );

		particleSystem.Play();

		lateUpdate += Hover;
		// lateUpdate += SearchTarget;
	}

	public void OnDeselect()
	{
		FFLogger.Log( name + " Deselected!" );

		selectionCollider.enabled = false;

		particleSystem.Stop();
		// particleSystem.

		lateUpdate = ExtensionMethods.EmptyMethod;
		movementTween.Kill();

		hoverValue = startPosition.y;

		transform.DOMove( startPosition, 0.5f ).OnComplete( () => selectionCollider.enabled = true );
		transform.DORotate( startRotation, 0.5f );
	}

	#endregion

	#region Implementation
	void Hover()
	{
		var position = transform.position;
		position.y = hoverValue;
		position.z = startPosition.z;

		transform.position = position;

		//looktargetovertime
	}
	#endregion
}
