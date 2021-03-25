/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using UnityEngine.UI;
using FFStudio;
using TMPro;
using DG.Tweening;
// using ElephantSDK;

public class UIManager : MonoBehaviour
{
	#region Fields
	[Header( "Event Listeners" )]
	public EventListenerDelegateResponse levelLoadedListener;
	public EventListenerDelegateResponse levelCompletedListener;
	public EventListenerDelegateResponse levelFailListener;

	[Header( "Fired Events" )]
	public GameEvent levelRevealed;
	public GameEvent loadNewLevel;
	public GameEvent resetLevel;

	[Header( "UI Elements" )]
	public Image backgroundRenderer;
	public UIText levelText;
	public UIFillingBar uiLevelProgression;
	public Button uiLevelCompleted;
	public Button uiItemUnlocked;
	public Button uiLevelFailed;


	public CurrentLevelData currentLevel;

	#endregion

	#region Unity API
	private void OnEnable()
	{
		levelLoadedListener.OnEnable();
		levelCompletedListener.OnEnable();
		levelFailListener.OnEnable();
	}
	private void OnDisable()
	{
		levelLoadedListener.OnDisable();
		levelCompletedListener.OnDisable();
		levelFailListener.OnDisable();
	}

	private void Awake()
	{
		levelLoadedListener.response = LevelLoadedResponse;
		levelCompletedListener.response = LevelCompleteResponse;
		levelFailListener.response = LevelFailedResponse;
	}

	#endregion

	#region API
	public void ItemUnlockedContinue()
	{
		uiItemUnlocked.interactable = false;
		uiLevelCompleted.gameObject.SetActive( false );

		FFLogger.Log( "Load New Level" );
		loadNewLevel.Raise();
	}

	public void LevelCompleteContinue()
	{
		FFLogger.Log( "Level Continue" );

		uiLevelCompleted.interactable = false;
		uiItemUnlocked.interactable = true;

		uiItemUnlocked.transform.DOScale( Vector3.one, 0.5f );
	}

	public void LevelFailedContinue()
	{
		FFLogger.Log( "Level Failed Continue" );

		uiLevelFailed.interactable = false;

		levelLoadedListener.response += () => uiLevelFailed.transform.DOScale( Vector3.zero, 0.1f );
		resetLevel.Raise();
	}
	#endregion

	#region Implementation
	void LevelCompleteResponse()
	{
		// Elephant.LevelCompleted( currentLevel.currentLevel );

		var sequence = DOTween.Sequence();
		sequence.Append( levelText.GoStartPosition() );
		sequence.Join( uiLevelProgression.GoStartPosition() );

		uiLevelCompleted.gameObject.SetActive( true );
		uiItemUnlocked.gameObject.SetActive( true );

		uiLevelCompleted.transform.localScale = Vector3.zero;
		uiItemUnlocked.transform.localScale = Vector3.zero;

		uiLevelCompleted.interactable = true;
		uiLevelCompleted.transform.DOScale( Vector3.one, 0.5f );
	}

	void LevelLoadedResponse()
	{
		FFLogger.Log( "Level Loaded" );
		// Elephant.LevelStarted( currentLevel.currentLevel );

		levelLoadedListener.response = LevelLoadedResponse;

		levelText.textRenderer.text = "Level " + currentLevel.currentConsecutiveLevel;
		uiItemUnlocked.transform.DOScale( Vector3.zero, 0.5f );

		var sequence = DOTween.Sequence();
		sequence.Append( backgroundRenderer.DOFade( 0, 1f ) );
		sequence.AppendCallback( levelRevealed.Raise );
		sequence.Append( levelText.GoTargetPosition() );
		sequence.Join( uiLevelProgression.GoTargetPosition() );
	}

	void LevelFailedResponse()
	{
		// Elephant.LevelFailed( currentLevel.currentLevel );

		levelText.GoStartPosition();
		uiLevelProgression.GoStartPosition();

		uiLevelFailed.gameObject.SetActive( true );
		uiLevelFailed.interactable = true;

		uiLevelFailed.transform.localScale = Vector3.zero;
		uiLevelFailed.transform.DOScale( Vector3.one, 0.5f );
	}
	#endregion
}