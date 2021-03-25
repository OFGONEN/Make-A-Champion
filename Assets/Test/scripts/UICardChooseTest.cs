using UnityEngine;
using FFStudio;
using UnityEngine.UI;

public class UICardChooseTest : MonoBehaviour
{
	public IntGameEvent chooseEvent;
	public Button button;

	public void Choose( int index )
	{
		chooseEvent.eventValue = index;
		chooseEvent.Raise();

		button.enabled = false;
	}
}
