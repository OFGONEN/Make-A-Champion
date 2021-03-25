/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

public class WeightStack : MonoBehaviour
{
	#region Fields
	[Header( "Event Listeners" )]
	public EventListenerDelegateResponse weightPackedListener;

	//Public Variables
	public PackableWeight[] weightStack;

	//Private Variables
	private int weightIndex;
	#endregion

	#region Unity API
	private void OnEnable()
	{
		weightPackedListener.OnEnable();
	}

	private void OnDisable()
	{

		weightPackedListener.OnDisable();
	}

	private void Start()
	{
		weightPackedListener.response = WeightPackedResponse;

		weightStack[ 0 ].selectionCollider.enabled = true;
	}
	#endregion

	#region API
	#endregion

	#region Implementation
	void WeightPackedResponse()
	{
		var newWeightIndex = Mathf.Clamp( weightIndex + 1, 0, weightStack.Length - 1 );

		FFLogger.Log( "WeightIndex " + weightIndex + " Array: " + weightStack.Length );
		weightStack[ newWeightIndex ].selectionCollider.enabled = true;
		weightStack[ weightIndex ].selectionCollider.enabled = false;

		weightIndex = newWeightIndex;
	}
	#endregion
}