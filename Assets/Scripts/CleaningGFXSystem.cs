/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using FFStudio;

public class CleaningGFXSystem : MonoBehaviour
{
#region Fields
    [ Header( "Listened Events" ) ]
    public EventListenerDelegateResponse aDirtRemovedFromASet; /* We have just a few lists, we'll check all of them everytime. */

    [ Header( "Fired Events" ) ]
    public GameEvent cleaningPhase1Completed;
	public GameEvent cleaningPhase2Completed;

    [ Header( "Dirt Sets" ) ]
    public DirtSet standByList, readyToBeWipedList;

    [ Header( "State" ) ]
    public CleaningGFXSystemState state = CleaningGFXSystemState.CleaningWithBubbles;

	[ Header( "Bubbles" ) ]
	public GameObject[] bubblePrefabs;
#endregion

#region Unity API
    private void OnEnable()
    {
		aDirtRemovedFromASet.OnEnable();

        FFLogger.Log( name + "'s OnEnable(): Ready." );

		/* This component is executed before Dirt component(s), so we can now safely "reset" runtime sets here. */
		standByList.ClearSet();
		readyToBeWipedList.ClearSet();
	}

    private void OnDisable()
    {
		aDirtRemovedFromASet.OnDisable();

		/* Also do cleanup here (in addition to OnEnable), just in case. */
		standByList.ClearSet();
		readyToBeWipedList.ClearSet();
    }

    private void Awake()
    {
		aDirtRemovedFromASet.response = OnADirtRemoved;
    }
#endregion

#region API
	private void CreateBubble( Vector3 atPosition )
	{
		SimplePool.Spawn( bubblePrefabs[ Random.Range( 0, bubblePrefabs.Length ) ], atPosition, Quaternion.identity );
	}
#endregion

#region Implementation
    private void OnADirtRemoved()
    {
		if( state == CleaningGFXSystemState.CleaningWithBubbles && standByList.itemList.Count == 0 )
		{
            FFLogger.Log( name + ": All dirt have been depleted by bubbles. Switching to phase 2." );
			cleaningPhase1Completed.Raise();
			state = CleaningGFXSystemState.WipingWithCloth;
		}
		else if( state == CleaningGFXSystemState.WipingWithCloth && readyToBeWipedList.itemList.Count == 0 )
		{
			FFLogger.Log( name + ": All dirt have been destroyed by the cloth. Both phases are completed." );
			cleaningPhase2Completed.Raise();
		}
	}
#endregion
}