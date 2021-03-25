/* Created by and for usage of FF Studios (2021). */

using UnityEngine;

public class BubbleTest : MonoBehaviour
{
#region Fields
	public CleaningGFXSystem cleaningGFXSystem;
#endregion

#region Unity API
    private void Update()
    {
        if( Input.GetKeyDown( KeyCode.Return ) )
			cleaningGFXSystem.CreateBubble( Vector3.zero );
	}
#endregion

#region API
#endregion

#region Implementation
#endregion
}
