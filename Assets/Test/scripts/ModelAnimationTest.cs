using UnityEngine;
using FFStudio;
using TMPro;

public class ModelAnimationTest : MonoBehaviour
{
	public EventListenerDelegateResponse animationChangeListener;
	public Animator animator;
	public TextMeshProUGUI moodText;
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
