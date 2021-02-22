using UnityEngine;
using FFStudio;

public class ModelAnimationTest : MonoBehaviour
{
	public EventListenerDelegateResponse animationChangeListener;
	public Animator animator;

	private void OnEnable()
	{
		animationChangeListener.OnEnable();
		animationChangeListener.response = SetAnimator;
	}

	void SetAnimator()
	{
		var _changeEvent = animationChangeListener.gameEvent as IntGameEvent;
		animator.SetInteger( "Selection", _changeEvent.eventValue );
	}
}
