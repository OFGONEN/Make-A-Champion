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

	[Header( "Fired Events" )]
	public StringGameEvent animationTriggerEvent;
	public GameEvent correctWeightPacked;


	[Header( "Shared Variables" )]
	public SharedReferance currentWeightTarget;

	//Public Fields
	public Transform[] weightTargets;
	public GameObject initialTarget;

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
		var changeEvet = weightPackedListener.gameEvent as IntGameEvent;

		if( changeEvet.eventValue != weightTargetIndex )
		{
			animationTriggerEvent.eventValue = "Fail";
			animationTriggerEvent.Raise();
		}
		else
			correctWeightPacked.Raise();

		weightTargets[ weightTargetIndex ].gameObject.SetActive( false ); // Close the silhouette
		weightTargetIndex++;

		if( weightTargetIndex == weightTargets.Length )
			initialTarget.SetActive( false );

		weightTargetIndex = Mathf.Clamp( weightTargetIndex, 0, weightTargets.Length - 1 );
		currentWeightTarget.sharedValue = weightTargets[ weightTargetIndex ];
	}
	#endregion
}