/* Created by and for usage of FF Studios (2021). */

using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using DG.Tweening;
using Lean.Touch;

public class ParticleSink : MonoBehaviour
{
	#region Fields
	[Header( "Event Listeneres" )]
	public EventListenerDelegateResponse enableSelectionListener;
	public EventListenerDelegateResponse stopFillingListener;

	[ Header( "Modified Shared Data" ) ]
	public SharedFloatProperty liquidFillPercentage;

	[ Header( "Parameters" ) ]
	[ Range( 0.01f, 1.0f ) ] public float particleContributionToLiquidPercentage = 0.5f;

	//Public Fields
	public Vector3 hoverRotation;


	//Private Fields
	private new ParticleSystem particleSystem;
	private LeanSelectable leanSelectable;
	private TweenCallback lateUpdate;
	private TweenCallback onDeSelect;
	private Tween movementTween;

	private Vector3 startPosition;
	private Vector3 startRotation;

	private float hoverValue;
	#endregion

	#region Unity API

	private void OnEnable()
	{
		enableSelectionListener.OnEnable();
		stopFillingListener.OnEnable();
	}

	private void OnDisable()
	{
		enableSelectionListener.OnDisable();
		stopFillingListener.OnDisable();
	}

	private void Awake()
    {
		particleSystem = GetComponent< ParticleSystem >();
		leanSelectable = GetComponent<LeanSelectable>();

		leanSelectable.enabled = false;

		lateUpdate = ExtensionMethods.EmptyMethod;

		startPosition = transform.position;
		startRotation = transform.rotation.eulerAngles;

		hoverValue = startPosition.y;

		enableSelectionListener.response = () =>
		{
			leanSelectable.enabled = true;
			enableSelectionListener.response = ExtensionMethods.EmptyMethod;
		};

		stopFillingListener.response = StopFilling;

		onDeSelect = ReturnToDefault;
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


		movementTween = DOTween.To( () => hoverValue, x => hoverValue = x, startPosition.y + 0.25f, 0.25f );

		transform.DORotate( hoverRotation, 0.5f );

		particleSystem.Play();

		lateUpdate += Hover;
	}

	public void OnDeselect()
	{
		onDeSelect();
	}

	#endregion

	#region Implementation
	void Hover()
	{
		var position = transform.position;
		position.y = hoverValue;
		position.z = startPosition.z;

		transform.position = position;
	}

	void ReturnToDefault()
	{
		FFLogger.Log( name + " Deselected!" );

		leanSelectable.enabled = false;

		particleSystem.Stop();

		lateUpdate = ExtensionMethods.EmptyMethod;
		movementTween.Kill();

		hoverValue = startPosition.y;

		transform.DOMove( startPosition, 0.5f ).OnComplete( () => leanSelectable.enabled = true );
		transform.DORotate( startRotation, 0.5f );

	}

	void StopFilling()
	{
		stopFillingListener.response = ExtensionMethods.EmptyMethod;
		onDeSelect = ExtensionMethods.EmptyMethod;
		leanSelectable.enabled = false;

		particleSystem.Stop();

		lateUpdate = ExtensionMethods.EmptyMethod;
		movementTween.Kill();

		hoverValue = startPosition.y;

		transform.DOMove( startPosition, 0.5f );
		transform.DORotate( startRotation, 0.5f );
	}
	#endregion
}
