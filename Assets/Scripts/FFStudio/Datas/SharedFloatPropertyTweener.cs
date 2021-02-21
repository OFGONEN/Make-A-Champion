using UnityEngine;
using NaughtyAttributes;
using DG.Tweening;

namespace FFStudio
{
	[CreateAssetMenu( fileName = "SharedFloatPropertyTweener", menuName = "FF/Data/Shared/FloatPropertyTweener" )]
	public class SharedFloatPropertyTweener : ScriptableObject
	{
		#region Fields
		public FloatGameEvent changeEvent;
		public float changeDuration;
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
					.OnComplete( () => valueChangeTween = null );
				}
			}
		}

		#region API
		public void KillTween()
		{
			if( valueChangeTween != null )
				valueChangeTween.Kill();
		}
		#endregion

		#region Implementation
		void OnChangeUpdate()
		{
			changeEvent.eventValue = sharedValue;
			changeEvent.Raise();
		}
		#endregion
	}
}