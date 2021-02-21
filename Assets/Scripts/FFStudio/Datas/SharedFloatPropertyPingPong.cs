using UnityEngine;
using NaughtyAttributes;
using DG.Tweening;

namespace FFStudio
{
	[CreateAssetMenu( fileName = "SharedFloatPropertyPingPong", menuName = "FF/Data/Shared/FloatPropertyPingPong" )]
	public class SharedFloatPropertyPingPong : ScriptableObject
	{

		#region Fields
		[MinMaxSlider( -1f, 1f )]
		public Vector2 minMaxValues;
		[Tooltip( "Duration movement of from middle to end" )]
		public float changeDuration;
		public Ease changeEase;
		public FloatGameEvent changeEvent;

		private float sharedValue;
		private Sequence pingPongSequnce;
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
					sharedValue = value;

					changeEvent.eventValue = value;
					changeEvent.Raise();
				}
			}
		}

		public void StartPingPong()
		{
			if( pingPongSequnce != null )
				pingPongSequnce.Kill();

			sharedValue = 0;

			var _maxTween = DOTween.To( () => Value, x => Value = x, minMaxValues.y, changeDuration ).SetEase( changeEase );
			var _middleTween = DOTween.To( () => Value, x => Value = x, 0, changeDuration ).SetEase( changeEase );
			var _minTween = DOTween.To( () => Value, x => Value = x, minMaxValues.x, changeDuration * 2f ).SetEase( changeEase );

			pingPongSequnce = DOTween.Sequence();
			pingPongSequnce.Join( _maxTween );
			pingPongSequnce.Append( _minTween );
			pingPongSequnce.Append( _middleTween );
			pingPongSequnce.SetLoops( -1 );
		}

		public void EndPingPong()
		{
			if( pingPongSequnce != null )
				pingPongSequnce.Kill();

			sharedValue = 0;
		}

	}
}
