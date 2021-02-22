using UnityEngine;
using FFStudio;

public class PingPongTest : MonoBehaviour
{
	public SharedFloatPropertyPingPong pingPong;

	private void Start()
	{
		pingPong.StartPingPong();
	}
}
