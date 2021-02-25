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
		[Header( "UI Elements" )]
		public Image uiFillingImage;
		#endregion

		#region Unity API
		private void OnEnable()
		{
			fillingAmountChangeListener.OnEnable();
		}
		protected override void Awake()
		{
			base.Awake();
			fillingAmountChangeListener.response = FillingAmountChange;
			uiFillingImage.fillAmount = 0;
		}
		private void OnDisable()
		{
			fillingAmountChangeListener.OnDisable();
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
		#endregion
	}
}