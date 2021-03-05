/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using UnityEngine.UI;
using FFStudio;
using TMPro;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
	#region Fields
	[Header( "Event Listeners" )]
	public EventListenerDelegateResponse levelLoadedListener;
	public EventListenerDelegateResponse levelCompletedResponse;
	public EventListenerDelegateResponse levelFailResponse;

	[Header( "Fired Events" )]
	public GameEvent levelRevealed;

	[Header( "UI Elements" )]
	public Image backgroundRenderer;
	public UIText levelText;
	public Button uiLevelCompleted;
	public Button uiItemUnlocked;
	public CurrentLevelData currentLevel;

	#endregion

	#region Unity API
	private void OnEnable()
	{
		levelLoadedListener.OnEnable();
		levelCompletedResponse.OnEnable();
	}
	private void OnDisable()
	{
		levelLoadedListener.OnDisable();
		levelCompletedResponse.OnDisable();
	}

	private void Awake()
	{
		levelLoadedListener.response = LevelLoadedResponse;
		levelCompletedResponse.response = LevelCompleteResponse;
	}

	#endregion

	#region API
	public void ItemUnlockedContinue()
	{
		uiItemUnlocked.interactable = false;

		uiLevelCompleted.gameObject.SetActive( false );

		FFLogger.Log( "Load New Level" );
		// Load new level
	}
	public void LevelCompleteContinue()
	{
		FFLogger.Log( "Level Continue" );

		uiLevelCompleted.interactable = false;
		uiItemUnlocked.interactable = true;

		uiItemUnlocked.transform.DOScale( Vector3.one, 0.5f );
	}
	#endregion

	#region Implementation
	void LevelCompleteResponse()
	{
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
		levelText.textRenderer.text = "Level " + currentLevel.currentLevel;

		var sequence = DOTween.Sequence();
		sequence.Append( backgroundRenderer.DOFade( 0, 1f ) );
		sequence.AppendCallback( levelRevealed.Raise );
		sequence.Append( levelText.GoTargetPosition() );
	}
	#endregion
}