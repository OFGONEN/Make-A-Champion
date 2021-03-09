/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

public class WeightTargets : MonoBehaviour
{
	#region Fields
	[Header( "Event Listeners" )]
	public EventListenerDelegateResponse weightPackedListener;

	[Header( "Shared Variables" )]
	public SharedReferance currentWeightTarget;

	//Public Fields
	public Transform[] weightTargets;

	//Private Fields
	private int weightTargetIndex;
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

	private void Awake()
	{
		weightPackedListener.response = WeightPackedResponse;

		currentWeightTarget.sharedValue = weightTargets[ 0 ];
	}
	#endregion

	#region API
	#endregion

	#region Implementation
	void WeightPackedResponse()
	{
		weightTargetIndex++;

		weightTargetIndex = Mathf.Clamp( weightTargetIndex, 0, weightTargets.Length - 1 );
		currentWeightTarget.sharedValue = weightTargets[ weightTargetIndex ];
	}
	#endregion
}