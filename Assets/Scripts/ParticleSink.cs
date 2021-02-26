/* Created by and for usage of FF Studios (2021). */

using System.Collections.Generic;
using UnityEngine;
using FFStudio;

public class ParticleSink : MonoBehaviour
{
#region Fields
	[ Header( "Modified Shared Data" ) ]
	public SharedFloatProperty liquidFillPercentage;

	[ Header( "Parameters" ) ]
	[ Range( 0.01f, 1.0f ) ] public float particleContributionToLiquidPercentage = 0.5f;

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
		liquidFillPercentage.Value += numberOfEnteringParticles * particleContributionToLiquidPercentage;
    }
#endregion

#region API
#endregion

#region Implementation
#endregion
}
