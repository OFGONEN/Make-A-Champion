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
	public GameObject bubblePrefab;
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

#if UNITY_EDITOR
	// TODO: Remove when testing is done.
	private void Update()
	{
		if( Input.GetKeyDown( KeyCode.Return ) )
			SimplePool.Spawn( bubblePrefab, Vector3.zero, Quaternion.identity );
	}
#endif

#endregion

#region API
    /* CleaningGFXSystem component is autonomous; Hence, no public API to control it. */
#endregion

#region Implementation
    private void OnADirtRemoved()
    {
		if( state == CleaningGFXSystemState.CleaningWithBubbles && standByList.itemList.Count == 0 )
		{
			cleaningPhase1Completed.Raise();
			state = CleaningGFXSystemState.WipingWithCloth;
            FFLogger.Log( name + ": All dirt have been depleted by bubbles. Switching to phase 2." );
		}
		else if( state == CleaningGFXSystemState.WipingWithCloth && readyToBeWipedList.itemList.Count == 0 )
		{
			cleaningPhase2Completed.Raise();
			FFLogger.Log( name + ": All dirt have been destroyed by the cloth. System work finished." );
		}
	}
#endregion
}