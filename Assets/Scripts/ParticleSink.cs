/* Created by and for usage of FF Studios (2021). */

using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using NaughtyAttributes;

public class ParticleSink : MonoBehaviour
{
#region Fields
	[ Header( "Modified Shared Data" ) ]
	public SharedFloatProperty mixLiquidPercentage;

	[ Header( "Take Settings From" ), Expandable ]
	public GameSettings gameSettings;

    private new ParticleSystem particleSystem;
#endregion

#region Unity API
    private void Start()
    {
		particleSystem = GetComponent< ParticleSystem >();
	}

    private void OnParticleTrigger()
    {
		List< ParticleSystem.Particle > particles = new List< ParticleSystem.Particle >();

		int numberOfEnteringParticles = particleSystem.GetTriggerParticles( ParticleSystemTriggerEventType.Enter, particles );

		for( int i = 0; i < numberOfEnteringParticles; i++ )
		{
			ParticleSystem.Particle particle = particles[ i ];
#if UNITY_EDITOR
			particle.startColor = new Color32( 255, 0, 0, 255 ); /* FOR VISUALIZATION */
#endif
			particles[ i ] = particle;
			mixLiquidPercentage.Value += gameSettings.particleContributionToLiquidPercentage;
		}
        
		particleSystem.SetTriggerParticles( ParticleSystemTriggerEventType.Enter, particles );
    }
#endregion

#region API
#endregion

#region Implementation
#endregion
}
