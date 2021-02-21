using FFStudio;
using UnityEngine;
using UnityEngine.UI;

namespace FFStudio
{
	public class UIFillingBar : MonoBehaviour
	{
		#region Fields
		public Image uiFillingImage;
		public EventListenerDelegateResponse fillingAmountChangeListener;
		#endregion

		#region Unity API
		private void OnEnable()
		{
			fillingAmountChangeListener.OnEnable();
		}
		private void Awake()
		{
			fillingAmountChangeListener.response = FillingAmountChange;
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