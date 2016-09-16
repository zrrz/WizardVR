using UnityEngine;
using System.Collections;

public class FireCharge : SpellCharge {
	public override void StartCharge ()
	{
//		transform.FindChild ("ChargeParticle").gameObject.SetActive (true);
		if(transform.FindChild ("ChargeParticle").GetChild (0).GetComponent<WindZone> ().windMain < 0)
			transform.FindChild ("ChargeParticle").GetChild (0).GetComponent<WindZone> ().windMain *= -1f;

		transform.FindChild ("FlameTrail").GetComponent<ParticleSystem> ().startLifetime *= 2f;
		transform.FindChild ("FlameTrail").FindChild("Distortion").GetComponent<ParticleSystem> ().startLifetime *= 2f;
	}

	public override void UpdateCharge() {
		ParticleUtilities.ScaleParticleSystem(transform.FindChild ("ChargeParticle").gameObject, 1 + (scalingSpeed * Time.deltaTime));
	}

	public override void ReleaseCharge ()
	{
//		transform.FindChild ("ChargeParticle").gameObject.SetActive (false);
		ParticleSystem m_System = transform.FindChild ("ChargeParticle").GetComponent<ParticleSystem>();
		ParticleSystem.Particle[] m_Particles = new ParticleSystem.Particle[m_System.maxParticles];
		// GetParticles is allocation free because we reuse the m_Particles buffer between updates
		int numParticlesAlive = m_System.GetParticles(m_Particles);
		// Change only the particles that are alive
		float radius = 0.4f;
		for (int i = 0; i < numParticlesAlive; i++)
		{
			if(Vector3.Distance(m_System.transform.position, m_Particles[i].position) < radius)
				m_Particles [i].lifetime = 0f;// += Vector3.up * m_Drift;
		}
		// Apply the particle changes to the particle system
		m_System.SetParticles(m_Particles, numParticlesAlive);
		transform.FindChild ("FlameTrail").FindChild("Distortion").GetComponent<ParticleSystem> ().startLifetime /= 2f;
	}

	public override void FizzleCharge ()
	{
		transform.FindChild ("ChargeParticle").GetChild (0).GetComponent<WindZone> ().windMain *= -1f;
	}
}
