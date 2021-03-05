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

	[Header( "Fired Events" )]
	public GameEvent levelFinished;

	private Animator animator;
	private bool levelComplete;
	#endregion

	#region Unity API
	private void OnEnable()
	{
		uiChooseListener.OnEnable();
	}
	private void OnDisable()
	{
		uiChooseListener.OnDisable();
	}
	private void Awake()
	{
		animator = GetComponent<Animator>();

		uiChooseListener.response = UIChooseResponse;
	}
	#endregion

	#region API
	#endregion

	#region Implementation
	void UIChooseResponse()
	{
		var changeEvent = uiChooseListener.gameEvent as IntGameEvent;
		animator.SetInteger( "Selection", changeEvent.eventValue );
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