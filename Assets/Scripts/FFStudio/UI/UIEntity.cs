using UnityEngine;
using DG.Tweening;

namespace FFStudio
{
	public class UIEntity : MonoBehaviour
	{
		public GameSettings gameSettings;
		[HideInInspector] public RectTransform uiTransform;
		public RectTransform destinationTransform;
		[HideInInspector] public Vector3 startPosition;
		#region Unity API
		protected virtual void Awake()
		{
			uiTransform = GetComponent< RectTransform >();
		}

		public virtual void Start()
		{
			startPosition = uiTransform.position;
		}
		#endregion
		public virtual Tween GoTargetPosition()
		{
			return uiTransform.DOMove( destinationTransform.position, gameSettings.uiEntityMoveTweenDuration );
		}
		public virtual Tween GoStartPosition()
		{
			return uiTransform.DOMove( startPosition, gameSettings.uiEntityMoveTweenDuration );
		}
		public virtual Tween GoPopOut()
		{
			return uiTransform.DOScale( Vector3.one, gameSettings.uiEntityScaleTweenDuration );
		}
		public virtual Tween GoPopIn()
		{
			return uiTransform.DOScale( Vector3.zero, gameSettings.uiEntityScaleTweenDuration );
		}
	}
}