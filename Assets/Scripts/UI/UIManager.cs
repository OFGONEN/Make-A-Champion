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

	public CurrentLevelData currentLevel;
	#endregion

	#region Unity API
	private void OnEnable()
	{
		levelLoadedListener.OnEnable();
	}
	private void OnDisable()
	{
		levelLoadedListener.OnDisable();
	}

	private void Awake()
	{
		levelLoadedListener.response = LevelLoadedResponse;
	}

	#endregion

	#region API
	#endregion

	#region Implementation
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