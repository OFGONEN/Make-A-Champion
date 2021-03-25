/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using DG.Tweening;

public class UIHelperText : UIText
{
	#region Fields
	public EventListenerDelegateResponse revealTextListener;
	public EventListenerDelegateResponse hideTextListener;
	public EventListenerDelegateResponse changeTextListener;

	private Vector3 startScale;
	#endregion


	#region Unity API

	private void OnEnable()
	{
		revealTextListener.OnEnable();
		hideTextListener.OnEnable();
		changeTextListener.OnEnable();
	}

	private void OnDisable()
	{

		revealTextListener.OnDisable();
		hideTextListener.OnDisable();
		changeTextListener.OnDisable();

	}

	protected override void Awake()
	{
		base.Awake();

		startScale = uiTransform.localScale;

		uiTransform.localScale = Vector3.zero;

		revealTextListener.response = () => TweenScale( startScale );
		hideTextListener.response = () => TweenScale( Vector3.zero );
		changeTextListener.response = ChangeTextResponse;

	}
	#endregion

	#region API


	#endregion

	#region Implementation
	void TweenScale( Vector3 scale )
	{
		uiTransform.DOScale( scale, gameSettings.uiEntityScaleTweenDuration );
	}

	void ChangeTextResponse()
	{
		uiTransform.localScale = Vector3.zero;

		var changeEvet = changeTextListener.gameEvent as StringGameEvent;
		textRenderer.text = changeEvet.eventValue;

		TweenScale( startScale );
	}

	#endregion
}