/* Created by and for usage of FF Studios (2021). */

using FFStudio;
using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;

public class WeightLoopLevelManager : MonoBehaviour
{
	#region Fields
	[Header( "Event Listeners" )]
	public EventListenerDelegateResponse tapInputListener;
	public EventListenerDelegateResponse levelRevealedListener;
	public EventListenerDelegateResponse idleStateEnteredListener;

	private EventListenerDelegateResponse weightTimeChangeListener;


	[Header( "Fired Events" )]
	public StringGameEvent animationTriggerEvent;
	public GameEvent levelCompleteEvent;
	public GameEvent levelFailedEvent;
	public StringGameEvent helperTextChangeEvent;

	[Header( "UI Elements" )]
	public UIPingPongMeter uiPingPongMeter;
	public UIFillingBar uiFillingBar;

	[Header( "Shared Variables" )]
	public SharedBool animationHardVariable;
	public SharedFloatPropertyTweener levelProgress;
	public SharedColorProperty uiFailbarColor;
	public SharedFloatPropertyPingPong pingPongFloat;
	public SharedFloatPropertyTweener weightTime;
	public SharedFloatPropertyFallBackTweener fallBackFloat;

	[Header( "Public Variables" )]
	public Animator animator;
	public CurrentLevelData currentLevel;


	private Camera mainCamera;
	private Tween failTween;
	private float failTime;
	private float failCount;
	private int loopCount;
	#endregion

	#region Unity API
	private void OnEnable()
	{
		tapInputListener.OnEnable();
		levelRevealedListener.OnEnable();
		idleStateEnteredListener.OnEnable();
		weightTimeChangeListener.OnEnable();
	}

	private void OnDisable()
	{
		weightTimeChangeListener.OnDisable();
		tapInputListener.OnDisable();
		idleStateEnteredListener.OnDisable();
		levelRevealedListener.OnDisable();
	}

	private void Awake()
	{
		weightTimeChangeListener = new EventListenerDelegateResponse();
		weightTimeChangeListener.gameEvent = weightTime.changeEvent;

		levelRevealedListener.response = LevelRevealedResponse;
		idleStateEnteredListener.response = ExtensionMethods.EmptyMethod;
		weightTimeChangeListener.response = ExtensionMethods.EmptyMethod;
		tapInputListener.response = ExtensionMethods.EmptyMethod;
	}

	private void Start()
	{
		mainCamera = Camera.main;

		mainCamera.transform.position = currentLevel.levelData.cameraStartPosition;
		mainCamera.transform.rotation = Quaternion.Euler( currentLevel.levelData.cameraStartRotation );

	}
	#endregion

	#region API
	public void LevelFailAnimationEnd()
	{
		levelFailedEvent.Raise();
	}

	public void PrepareGoUp()
	{
		FFLogger.Log( "Prepare Go Up" );
		helperTextChangeEvent.eventValue = "Tap to Lifth the Weight";
		helperTextChangeEvent.Raise();
		tapInputListener.response = GoUpPushTapTiming;
	}

	[Button]
	public void PrepareGoDown()
	{
		FFLogger.Log( "Prepare Go Down" );

		helperTextChangeEvent.eventValue = "Tap to Lower the Weight";
		helperTextChangeEvent.Raise();

		idleStateEnteredListener.response = ExtensionMethods.EmptyMethod;

		loopCount++;

		levelProgress.Value = loopCount / ( float )currentLevel.gameSettings.weightLoopComplete;

		if( loopCount >= currentLevel.gameSettings.weightLoopComplete )
		{
			// level complete
			FFLogger.Log( "Level Complete" );

			uiPingPongMeter.GoStartPosition();
			pingPongFloat.EndPingPong();

			levelCompleteEvent.Raise();
			tapInputListener.response = ExtensionMethods.EmptyMethod;
			weightTimeChangeListener.response = ExtensionMethods.EmptyMethod;

			return;
		}

		tapInputListener.response = GoDown;
		weightTimeChangeListener.response = WeightTimeChangeResponse;
	}
	#endregion

	#region Implementation
	void LevelRevealedResponse()
	{
		mainCamera.transform.DOMove( currentLevel.levelData.cameraEndPosition, currentLevel.gameSettings.cameraTravelDuration );
		mainCamera.transform.DORotate( currentLevel.levelData.cameraEndRotation, currentLevel.gameSettings.cameraTravelDuration );

		uiPingPongMeter.GoTargetPosition().OnComplete( () =>
		{
			tapInputListener.response = GoDown;
			pingPongFloat.StartPingPong();
		} );
	}

	void GoUpTick( float defaultFailTime )
	{
		var _diff = defaultFailTime - failTime;
		uiFailbarColor.Value = Color.Lerp( Color.green, Color.red, _diff / defaultFailTime );
	}

	void GoDown()
	{
		tapInputListener.response = ExtensionMethods.EmptyMethod;
		weightTimeChangeListener.response = WeightTimeChangeResponse;

		animator.SetFloat( "LiftSpeed", 0 );

		var _value = pingPongFloat.Value;
		// pingPongFloat.EndPingPong();

		if( _value >= -0.5f && _value <= 0.5f )
		{
			FFLogger.Log( "Chill Go Down" );
			animationTriggerEvent.eventValue = "Down";
			animationHardVariable.sharedValue = false;

			animationTriggerEvent.Raise();

			// uiPingPongMeter.GoStartPosition();
			// uiFillingBar.GoTargetPosition();
			fallBackFloat.Value = 0f;
			weightTime.Value = 0;
		}
		else if( _value >= -0.75f && _value <= 0.75f )
		{
			FFLogger.Log( "Hard Go Down" );
			animationTriggerEvent.eventValue = "Down";
			animationHardVariable.sharedValue = true;

			animationTriggerEvent.Raise();

			// uiPingPongMeter.GoStartPosition();
			// uiFillingBar.GoTargetPosition();
			fallBackFloat.Value = 0f;
			weightTime.Value = 0;
		}
		else
			LevelFailed();


	}

	void GoUpLoop()
	{
		var defaultFailTime = currentLevel.gameSettings.weightLoopFailTime;
		failTime = currentLevel.gameSettings.weightLoopFailTime;

		// animationTriggerEvent.eventValue = "Loop";
		// animationTriggerEvent.Raise();

		failTween = DOTween.To( () => failTime, x => failTime = x, 0, failTime )
		.OnComplete( LevelFailed )
		.OnUpdate( () => GoUpTick( defaultFailTime ) );

		fallBackFloat.StartTween( 0, 0.2f );

		tapInputListener.response = GoUpPush;
	}

	void GoUpPushTapTiming()
	{
		FFLogger.Log( "Tap Timing" );

		var _value = pingPongFloat.Value;

		if( _value >= -0.5f && _value <= 0.5f )
		{
			//Green
			weightTime.Value += currentLevel.gameSettings.weightLoopGreenValue;
		}
		else if( _value >= -0.75f && _value <= 0.75f )
		{
			//Yellow
			weightTime.Value += currentLevel.gameSettings.weightLoopYellowValue;
		}
		else
		{
			failCount++;
			weightTime.Value -= currentLevel.gameSettings.weightLoopRedValue;

			if( failCount >= 3 )
				LevelFailed();
		}

	}

	void WeightTimeChangeResponse()
	{
		if( weightTime.Value >= 1f )
		{
			FFLogger.Log( "Weight Time reached 1" );
			animator.SetFloat( "LiftSpeed", 1 );
			tapInputListener.response = ExtensionMethods.EmptyMethod;
			weightTimeChangeListener.response = ExtensionMethods.EmptyMethod;
			weightTime.KillTween();

			idleStateEnteredListener.response = PrepareGoDown;
			// PrepareGoDown();
		}
	}
	void GoUpPush()
	{
		fallBackFloat.Value += 0.2f;

		if( fallBackFloat.Value >= 1f )
		{
			failTween.Kill();
			fallBackFloat.CompleteTween();

			//GoUp;
			// animationTriggerEvent.eventValue = "Up";
			// animationTriggerEvent.Raise();

			animator.SetFloat( "LiftSpeed", 1 );

			tapInputListener.response = ExtensionMethods.EmptyMethod;

			if( loopCount < 3 )
			{
				uiPingPongMeter.GoTargetPosition().OnComplete( pingPongFloat.StartPingPong );
				uiFillingBar.GoStartPosition().OnComplete( PrepareGoDown );
				// fallBackFloat.Value = 0;
			}
		}
	}

	void LevelFailed()
	{
		FFLogger.Log( "Level Failed" );

		tapInputListener.response = ExtensionMethods.EmptyMethod;

		animator.SetFloat( "LiftSpeed", 1 );

		animationTriggerEvent.eventValue = "Fail";
		animationTriggerEvent.Raise();

		uiPingPongMeter.GoStartPosition();
		uiFillingBar.GoStartPosition();
	}
	#endregion
}