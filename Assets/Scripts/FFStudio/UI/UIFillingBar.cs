using FFStudio;
using UnityEngine;
using UnityEngine.UI;

namespace FFStudio
{
	public class UIFillingBar : UIEntity
	{
		#region Fields
		[Header( "Event Listeners" )]
		public EventListenerDelegateResponse fillingAmountChangeListener;
		public EventListenerDelegateResponse fillingBarColorChangeListener;
		[Header( "UI Elements" )]
		public Image uiFillingImage;
		public bool canChangeColor;
		#endregion

		#region Unity API
		private void OnEnable()
		{
			FFLogger.Log( "Filling Bar", gameObject );
			fillingAmountChangeListener.OnEnable();
			fillingBarColorChangeListener.OnEnable();
		}

		private void OnDisable()
		{
			fillingAmountChangeListener.OnDisable();
			fillingBarColorChangeListener.OnDisable();
		}

		protected override void Awake()
		{
			base.Awake();

			fillingAmountChangeListener.response = FillingAmountChange;

			if( canChangeColor )
				fillingBarColorChangeListener.response = FillingBarColorChange;
			else
				fillingBarColorChangeListener.response = ExtensionMethods.EmptyMethod;

			uiFillingImage.fillAmount = 0;
		}
		#endregion

		#region API
		#endregion

		#region Implementation
		void FillingAmountChange()
		{
			var _changeEvent = ( fillingAmountChangeListener.gameEvent as FloatGameEvent );
			uiFillingImage.fillAmount = _changeEvent.eventValue;
		}
		void FillingBarColorChange()
		{
			var changeEvent = ( fillingBarColorChangeListener.gameEvent as ColorGameEvent );
			uiFillingImage.color = changeEvent.eventValue;
		}
		#endregion
	}
}