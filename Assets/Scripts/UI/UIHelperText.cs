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

		uiTransform.localScale = Vector3.zero;

		revealTextListener.response = () => TweenScale( Vector3.one / 2f );
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

		GoPopOut();
	}

	#endregion
}