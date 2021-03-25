/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using FFStudio;
using DG.Tweening;

public class Bubble : MonoBehaviour
{
#region Fields
    public Rigidbody theRigidbody;

    [ Header( "Take Settings From" ) ]
    public GameSettings gameSettings;
#endregion

#region Unity API
    private void OnEnable()
    {
		var randomForceMagnitude = Random.Range( gameSettings.bubbleSpreadForceMultiplier.x,
                                                 gameSettings.bubbleSpreadForceMultiplier.y );
		theRigidbody.AddForce( Random.onUnitSphere * randomForceMagnitude, ForceMode.Acceleration );
	}
    
    private void OnTriggerEnter( Collider other )
    {
        if( other.CompareTag( "Cloth" ) )
			StartDestructionTween();
	}
#endregion

#region API
    /* Autonomous; No public API. */
#endregion

#region Implementation
    private void StartDestructionTween()
    {
		transform.DOScale( Vector3.zero, gameSettings.bubbleDestroyTweenDuration )
				 .OnComplete( () => SimplePool.Despawn( gameObject ) );
	}
#endregion
}
