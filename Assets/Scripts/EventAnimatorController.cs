/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using System;

public class EventAnimatorController : MonoBehaviour
{
	#region Fields
	[Header( "Event Listeners" )]
	public EventListenerDelegateResponse uiChooseListener;
	public EventListenerDelegateResponse triggerEventListener;
	public EventListenerDelegateResponse loopTimeChangeListener;

	[Header( "Fired Events" )]
	public GameEvent levelFinished;

	public GameObject[] unlockableItems;
	public SharedBool animationHardVariable;

	private Animator animator;
	private bool levelComplete;
	#endregion

	#region Unity API
	private void OnEnable()
	{
		uiChooseListener.OnEnable();
		triggerEventListener.OnEnable();
		loopTimeChangeListener.OnEnable();
	}
	private void OnDisable()
	{
		uiChooseListener.OnDisable();
		triggerEventListener.OnDisable();
		loopTimeChangeListener.OnDisable();
	}
	private void Awake()
	{
		animator = GetComponent<Animator>();

		uiChooseListener.response = UIChooseResponse;
		triggerEventListener.response = TriggerEventResponse;
		loopTimeChangeListener.response = LoopTimeChangeResponse;
	}
	#endregion

	#region API
	public void ActivateItem( int index )
	{
		unlockableItems[ index ].SetActive( true );
	}
	#endregion

	#region Implementation
	void UIChooseResponse()
	{
		var changeEvent = uiChooseListener.gameEvent as IntGameEvent;
		animator.SetInteger( "Selection", changeEvent.eventValue );
	}

	void TriggerEventResponse()
	{
		animator.SetBool( "Hard", animationHardVariable.sharedValue );

		var changeEvent = triggerEventListener.gameEvent as StringGameEvent;
		animator.SetTrigger( changeEvent.eventValue );
	}

	void LoopTimeChangeResponse()
	{
		var changeEvent = loopTimeChangeListener.gameEvent as FloatGameEvent;
		animator.SetFloat( "Lift", changeEvent.eventValue );
	}

	void SelectionFinished()
	{
		FFLogger.Log( "Level Finished" );

		if( levelComplete ) return;

		levelComplete = true;
		levelFinished.Raise();
	}
	#endregion
}