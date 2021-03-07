/* Created by and for usage of FF Studios (2021). */

using FFStudio;
using UnityEngine;
using DG.Tweening;

public class WeightLoopLevelManager : MonoBehaviour
{
	#region Fields
	[Header( "Event Listeners" )]
	public EventListenerDelegateResponse tapInputListener;
	public EventListenerDelegateResponse levelRevealedListener;


	[Header( "Fired Events" )]
	public StringGameEvent animationTriggerEvent;
	public GameEvent levelCompleteEvent;
	public GameEvent levelFailedEvent;

	[Header( "UI Elements" )]
	public UIPingPongMeter uiPingPongMeter;
	public UIFillingBar uiFillingBar;

	[Header( "Shared Variables" )]
	public SharedBool animationHardVariable;
	public SharedColorProperty uiFailbarColor;
	public SharedFloatPropertyPingPong pingPongFloat;
	public SharedFloatPropertyFallBackTweener fallBackFloat;

	public CurrentLevelData currentLevel;


	private Tween failTween;
	private float failTime;
	private int loopCount;
	#endregion

	#region Unity API
	private void OnEnable()
	{
		tapInputListener.OnEnable();
		levelRevealedListener.OnEnable();
	}

	private void OnDisable()
	{
		tapInputListener.OnDisable();
		levelRevealedListener.OnDisable();
	}

	private void Awake()
	{
		levelRevealedListener.response = LevelRevealedResponse;
		tapInputListener.response = ExtensionMethods.EmptyMethod;
	}

	private void Start()
	{
		var mainCamera = Camera.main;

		mainCamera.transform.position = currentLevel.levelData.cameraStartPosition;
		mainCamera.transform.rotation = Quaternion.Euler( currentLevel.levelData.cameraEndRotation );

		mainCamera.transform.DOMove( currentLevel.levelData.cameraEndPosition, 0.5f );
		mainCamera.transform.DORotate( currentLevel.levelData.cameraEndRotation, 0.5f );
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
		tapInputListener.response = GoUpLoop;
	}

	public void PrepareGoDown()
	{
		FFLogger.Log( "Prepare Go Down" );

		loopCount++;

		if( loopCount >= currentLevel.gameSettings.weightLoopComplete )
		{
			// level complete
			FFLogger.Log( "Level Complete" );

			uiPingPongMeter.GoStartPosition();
			pingPongFloat.EndPingPong();

			levelCompleteEvent.Raise();
			tapInputListener.response = ExtensionMethods.EmptyMethod;

			return;
		}

		tapInputListener.response = GoDown;
	}
	#endregion

	#region Implementation
	void LevelRevealedResponse()
	{
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

		var _value = pingPongFloat.Value;
		pingPongFloat.EndPingPong();

		if( _value >= -0.5f && _value <= 0.5f )
		{
			FFLogger.Log( "Chill Go Down" );
			animationTriggerEvent.eventValue = "Down";
			animationHardVariable.sharedValue = false;

			animationTriggerEvent.Raise();

			uiPingPongMeter.GoStartPosition();
			uiFillingBar.GoTargetPosition();
		}
		else if( _value >= -0.75f && _value <= 0.75f )
		{
			FFLogger.Log( "Hard Go Down" );
			animationTriggerEvent.eventValue = "Down";
			animationHardVariable.sharedValue = true;

			animationTriggerEvent.Raise();

			uiPingPongMeter.GoStartPosition();
			uiFillingBar.GoTargetPosition();
		}
		else
			LevelFailed();
	}

	void GoUpLoop()
	{
		var defaultFailTime = currentLevel.gameSettings.weightLoopFailTime;
		failTime = currentLevel.gameSettings.weightLoopFailTime;

		animationTriggerEvent.eventValue = "Loop";
		animationTriggerEvent.Raise();

		failTween = DOTween.To( () => failTime, x => failTime = x, 0, failTime )
		.OnComplete( LevelFailed )
		.OnUpdate( () => GoUpTick( defaultFailTime ) );

		fallBackFloat.StartTween( 0, 0.2f );

		tapInputListener.response = GoUpPush;
	}

	void GoUpPush()
	{
		fallBackFloat.Value += 0.2f;

		if( fallBackFloat.Value >= 1f )
		{
			failTween.Kill();
			fallBackFloat.CompleteTween();

			//GoUp;
			animationTriggerEvent.eventValue = "Up";
			animationTriggerEvent.Raise();

			tapInputListener.response = ExtensionMethods.EmptyMethod;

			if( loopCount < 3 )
			{
				uiPingPongMeter.GoTargetPosition().OnComplete( pingPongFloat.StartPingPong );
				uiFillingBar.GoStartPosition();
				fallBackFloat.Value = 0;
			}
		}
	}

	void LevelFailed()
	{
		FFLogger.Log( "Level Failed" );

		tapInputListener.response = ExtensionMethods.EmptyMethod;

		animationTriggerEvent.eventValue = "Fail";
		animationTriggerEvent.Raise();

		uiPingPongMeter.GoStartPosition();
		uiFillingBar.GoStartPosition();
	}
	#endregion
}