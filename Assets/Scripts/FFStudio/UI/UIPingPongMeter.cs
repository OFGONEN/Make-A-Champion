using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace FFStudio
{
	public class UIPingPongMeter : UIEntity
	{
		#region Fields
		[Header( "Event Listeners" )]
		public EventListenerDelegateResponse meterPositionChangeListener;
		[Header( "UI Elements" )]
		public Image uiMeterImage;

		private Vector2 meterMinMaxValues;
		private float uiMeterPosY;
		#endregion

		#region Unity API
		private void OnEnable()
		{
			meterPositionChangeListener.OnEnable();
		}
		private void OnDisable()
		{
			meterPositionChangeListener.OnDisable();
		}
		protected override void Awake()
		{
			base.Awake();

			uiMeterPosY = uiMeterImage.rectTransform.anchoredPosition.y;

			meterMinMaxValues.y = uiTransform.sizeDelta.x / 2f - uiMeterImage.rectTransform.sizeDelta.x / 2f;
			meterMinMaxValues.x = -meterMinMaxValues.y;

			meterPositionChangeListener.response = MeterPositionChange;
		}
		#endregion

		#region Implementation
		void MeterPositionChange()
		{
			var _changeEvent = ( meterPositionChangeListener.gameEvent as FloatGameEvent );

			var _position = _changeEvent.eventValue * meterMinMaxValues.y;
			_position = Mathf.Clamp( _position, meterMinMaxValues.x, meterMinMaxValues.y );

			var meterPosition = _position * Vector2.right;
			meterPosition.y = uiMeterPosY;

			uiMeterImage.rectTransform.anchoredPosition = meterPosition;
		}
		#endregion
	}
}