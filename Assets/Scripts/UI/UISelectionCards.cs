/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using UnityEngine.UI;

public class UISelectionCards : UIEntity
{
	#region Fields
	[Header( "Event Listeners" )]
	public EventListenerDelegateResponse levelRevealedListener;

	[Header( "Fired Events" )]
	public IntGameEvent selectionEvent;


	#endregion

	#region Unity API
	private void OnEnable()
	{
		levelRevealedListener.OnEnable();
	}

	private void OnDisable()
	{
		levelRevealedListener.OnDisable();
	}

	protected override void Awake()
	{
		base.Awake();
		levelRevealedListener.response = () => GoTargetPosition();
	}
	#endregion

	#region API
	public void SelectionComplete( int selection )
	{
		var buttons = GetComponentsInChildren<Button>();
		FFLogger.Log( "Disabling Buttons: " + buttons.Length );

		for( var i = 0; i < buttons.Length; i++ )
		{
			buttons[ i ].interactable = false;
		}

		selectionEvent.eventValue = selection;
		selectionEvent.Raise();

		GoStartPosition();
	}
	#endregion

	#region Implementation
	#endregion
}