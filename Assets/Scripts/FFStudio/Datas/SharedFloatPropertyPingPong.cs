using UnityEngine;
using NaughtyAttributes;
using DG.Tweening;

namespace FFStudio
{
	[CreateAssetMenu( fileName = "SharedFloatPropertyPingPong", menuName = "FF/Data/Shared/FloatPropertyPingPong" )]
	public class SharedFloatPropertyPingPong : ScriptableObject
	{

		[MinMaxSlider( -1f, 1f )]
		public Vector2 minMaxValues;
		public FloatGameEvent changeEvent;
		private float sharedValue;
		private Sequence pingPongSequnce;

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
					sharedValue = value;

					changeEvent.eventValue = value;
					changeEvent.Raise();
				}
			}
		}

		public void StartPingPong( float startValue, float duration, Ease easeFunction )
		{
			if( pingPongSequnce != null )
			{
				pingPongSequnce.Kill();
				pingPongSequnce = null;
			}

			sharedValue = startValue;

			var _maxTween = DOTween.To( () => Value, x => Value = x, minMaxValues.y, duration ).SetEase( easeFunction );
			var _middleTween = DOTween.To( () => Value, x => Value = x, startValue, duration ).SetEase( easeFunction );
			var _minTween = DOTween.To( () => Value, x => Value = x, minMaxValues.x, duration * 2f ).SetEase( easeFunction );


			pingPongSequnce = DOTween.Sequence();
			pingPongSequnce.Join( _maxTween );
			pingPongSequnce.Append( _minTween );
			pingPongSequnce.Append( _middleTween );
			pingPongSequnce.SetLoops( -1 );
		}
	}
}
