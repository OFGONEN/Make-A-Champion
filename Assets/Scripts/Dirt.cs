/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using FFStudio;
using DG.Tweening;
using NaughtyAttributes;

public class Dirt : MonoBehaviour
{
#region Fields
    [ Header( "Fired Events" ) ]
	public DirtSet standByList, readyToBeWipedList;

    [ Header( "Take Settings From" ) ]
    public GameSettings gameSettings;

    [ Header( "Properties" ), Tooltip( "After this \"health\" is depleted (via cleaning with a cloth), dirt will be destroyed." ) ]
    [ ProgressBar( "Health", 100 ) ]
    public float health;

	private Tween healthDepleteTween;
    private int triggeringBubbleCount;
#endregion

#region Unity API
    private void OnEnable()
    {
		standByList.AddList( this );
	}

    private void Start()
    {
		health                = 100.0f;
		triggeringBubbleCount = 0;
	}

    private void OnTriggerEnter( Collider other )
    {
#if UNITY_EDITOR
        if( other.CompareTag( "Bubble" ) )
            FFLogger.Log( name + "'s OnTriggerEnter: And a Bubble triggered it!" );
        else if( other.CompareTag( "Cloth" ) )
            FFLogger.Log( name + "'s Dirt.OnTriggerEnter: And a Cloth triggered it!" );
        else
            FFLogger.Log( name + "'s Dirt.OnTriggerEnter: is called!" );
#endif

        /* Phase 1: */
        if( other.CompareTag( "Bubble" ) )
		{
			if( triggeringBubbleCount == 0)
			{
                /* Tween in 1-second-long intervals, run the same tween indefinitely (actually, until OnHealthDepleted()). */
				healthDepleteTween = DOTween.To( () => health, x => { health = x; if( health <= 0 ) OnHealthDepleted(); },
												 health - gameSettings.dirtDepletionRate, 1.0f )
											.SetLoops( -1, LoopType.Incremental );
			}

			triggeringBubbleCount++;
		}
		/* Phase 2: */
		else if( other.CompareTag( "Cloth" ) )
        {
			gameObject.SetActive( false );
			readyToBeWipedList.RemoveList( this );
			standByList.AddList( this );
		}
    }

    private void OnTriggerExit( Collider other )
    {
        /* Phase 1: */
		if( other.CompareTag( "Bubble" ) )
		{
			triggeringBubbleCount--;

            if( triggeringBubbleCount == 0 )
			    StopHealthDepletion();
		}
	}
#endregion

#region API
    /* Dirt components are autonomous; Hence, no public API to control them. */
#endregion

#region Implementation
    private void OnHealthDepleted()
    {
        FFLogger.Log( name + " is depleted by bubbles and ready to be wiped!" );
		StopHealthDepletion();
		standByList.RemoveList( this );
		readyToBeWipedList.AddList( this );
	}

    private void StopHealthDepletion()
    {
		if( healthDepleteTween != null )
		{
			healthDepleteTween.Kill();
			healthDepleteTween = null;
		}
    }
#endregion
}
