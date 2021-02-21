using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIModelViewTest : MonoBehaviour
{
	public ModelViewer[] viewers;

	private void Start()
	{
		for( var i = 0; i < viewers.Length; i++ )
		{
			viewers[ i ].CreateModels( i );
		}
	}
}
