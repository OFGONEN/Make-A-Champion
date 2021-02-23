using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FFStudio;
using DG.Tweening;

public class ModelLoopAnimationTest : MonoBehaviour
{
	public Animator animator;
	public TextMeshProUGUI moodText;
	public Image barColor;

	public EventListenerDelegateResponse tapInputListener;
	public SharedFloatPropertyPingPong pingPongFloat;
	public SharedFloatPropertyFallBackTweener fallBackFloat;

	public float failTime;
	public Tween failTween;

	private void OnEnable()
	{
		tapInputListener.OnEnable();
	}

	private void Awake()
	{
		tapInputListener.response = GoDown;
	}

	private void OnDisable()
	{
		tapInputListener.OnDisable();
	}

	private void Start()
	{
		pingPongFloat.StartPingPong();
	}

	void GoDown()
	{
		var _value = pingPongFloat.Value;
		pingPongFloat.EndPingPong();

		if( _value >= -0.5f && _value <= 0.5f )
		{
			FFLogger.Log( "Green Go Down" );
			animator.SetTrigger( "GoDown" );
			animator.SetTrigger( "GreenMood" );
		}
		else if( _value >= -0.75f && _value <= 0.75f )
		{
			FFLogger.Log( "Yello Go Down" );
			animator.SetTrigger( "GoDown" );
			animator.SetTrigger( "YellowMood" );
		}
		else
		{
			FFLogger.Log( "Level Failed" );
		}

		tapInputListener.response = GoUpLoop;
	}

	void GoUpLoop()
	{
		var _defaultFailTime = failTime;
		animator.SetBool( "TryToGoUp", true );
		failTween = DOTween.To( () => failTime, x => failTime = x, 0, failTime )
		.OnComplete( LevelFailed )
		.OnUpdate( () => UpdateFailColor( _defaultFailTime ) );

		fallBackFloat.StartTween( 0, 0.2f );

		tapInputListener.response = TryGoUp;
	}
	void TryGoUp()
	{
		fallBackFloat.Value += 0.2f;

		if( fallBackFloat.Value >= 1f )
		{
			failTween.Kill();
			fallBackFloat.CompleteTween();

			//GoUp;
			animator.SetBool( "GoUp", true );
			tapInputListener.response = ExtensionMethods.EmptyMethod;
		}
	}
	void LevelFailed()
	{
		FFLogger.Log( "Level Failed" );
		tapInputListener.response = ExtensionMethods.EmptyMethod;
		animator.enabled = false;
		failTween.Kill();
		fallBackFloat.CompleteTween();
	}

	void UpdateFailColor( float defaultFailTime )
	{
		var _diff = defaultFailTime - failTime;
		barColor.color = Color.Lerp( Color.green, Color.red, _diff / defaultFailTime );
	}
}
