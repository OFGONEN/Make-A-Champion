using UnityEngine;
using NaughtyAttributes;
using DG.Tweening;

namespace FFStudio
{
	[CreateAssetMenu( fileName = "SharedFloatPropertyFallBackTweener", menuName = "FF/Data/Shared/SharedFloatPropertyFallBackTweener" )]
	public class SharedFloatPropertyFallBackTweener : ScriptableObject
	{
		#region Fields
		public FloatGameEvent changeEvent;
		public float defaultValue;
		public float changeDuration;
		[Tooltip( "Amount of value to lost in one second" )]
		public float fallBackSpeed;
		public Ease changeEase;

		private float sharedValue;
		private Tween valueChangeTween;
		#endregion

		public float Value
		{
			get
			{
				return sharedValue;
			}
			set
			{
				if( !Mathf.Approximately( sharedValue, value ) )
				{
					if( valueChangeTween != null )
						valueChangeTween.Kill();

					valueChangeTween = DOTween.To( () => sharedValue, x => sharedValue = x, value, changeDuration )
					.SetEase( changeEase )
					.OnUpdate( OnChangeUpdate )
					.OnComplete( FallBackToDefault );
				}
			}
		}

		#region API
		public void StartTween( float startValue, float targetValue )
		{
			sharedValue = startValue;
			Value = targetValue;
		}

		public void CompleteTween( float endValue )
		{
			sharedValue = endValue;

			if( valueChangeTween != null )
				valueChangeTween.Kill();
		}
		public void CompleteTween()
		{
			if( valueChangeTween != null )
				valueChangeTween.Kill();

		}
		#endregion

		#region Implementation
		void FallBackToDefault()
		{
			valueChangeTween = DOTween.To( () => sharedValue, x => sharedValue = x, defaultValue, sharedValue / fallBackSpeed )
			.SetEase( changeEase )
			.OnUpdate( OnChangeUpdate )
			.OnComplete( () => valueChangeTween = null );
		}

		void OnChangeUpdate()
		{
			changeEvent.eventValue = sharedValue;
			changeEvent.Raise();
		}
		#endregion

	}
}
