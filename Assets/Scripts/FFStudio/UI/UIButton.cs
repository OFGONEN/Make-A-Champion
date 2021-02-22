using UnityEngine.UI;
using UnityEngine;
using FFStudio;
using DG.Tweening;

public class UIButton : UIEntity
{
	public Button uiButton;
	public override Tween GoTargetPosition()
	{
		uiButton.interactable = false;
		return uiTransform.DOMove( destinationTransform.position, gameSettings.uiEntityMoveTweenDuration ).OnComplete( MakeButtonInteractable );
	}
	public override Tween GoStartPosition()
	{
		uiButton.interactable = false;
		return uiTransform.DOMove( startPosition, gameSettings.uiEntityMoveTweenDuration ).OnComplete( MakeButtonInteractable );
	}

	public override Tween GoPopOut()
	{
		uiButton.interactable = false;
		return uiTransform.DOScale( Vector3.one, gameSettings.uiEntityScaleTweenDuration ).OnComplete( MakeButtonInteractable );
	}

	public override Tween GoPopIn()
	{
		uiButton.interactable = false;
		return uiTransform.DOScale( Vector3.zero, gameSettings.uiEntityScaleTweenDuration ).OnComplete( MakeButtonInteractable );
	}

	void MakeButtonInteractable()
	{
		uiButton.interactable = true;
	}


}
