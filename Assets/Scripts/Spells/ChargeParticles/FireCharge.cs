using UnityEngine;
using System.Collections;

public class FireCharge : SpellCharge {

	bool pullFire = false;

	void Start() {
		ParticleSystem.ShapeModule shapeModule = GetComponent<ParticleSystem> ().shape; 
		shapeModule.meshRenderer = transform.parent.parent.GetComponent<MeshRenderer>();
//		GetComponent<ParticleSystem> ()..shape = shapeModule;
	}

	public override void StartCharge ()
	{
//		transform.FindChild ("ChargeParticle").gameObject.SetActive (true);
//		if(transform.FindChild ("ChargeParticle").GetChild (0).GetComponent<WindZone> ().windMain < 0)
//			transform.FindChild ("ChargeParticle").GetChild (0).GetComponent<WindZone> ().windMain *= -1f;
		transform.FindChild ("FlameTrail").GetComponent<ParticleSystem> ().startLifetime *= 2f;
		transform.FindChild ("FlameTrail").FindChild("Distortion").GetComponent<ParticleSystem> ().startLifetime *= 2f;

		pullFire = true;
	}

	public override void UpdateCharge() {
		ParticleUtilities.ScaleParticleSystem(transform.FindChild ("ChargeParticle").gameObject, 1 + (scalingSpeed * Time.deltaTime));
	}

	void Update() {
		if (pullFire) {
			ParticleSystem chargeParticles = transform.FindChild ("ChargeParticle").GetComponent<ParticleSystem>();
			ParticleSystem.Particle[] particles = new ParticleSystem.Particle[chargeParticles.maxParticles];
			int numParticlesAlive = chargeParticles.GetParticles(particles);
			float gravityFactor = 0.1f;
			for (int i = 0; i < numParticlesAlive; i++)
			{
				if (Vector3.Distance(chargeParticles.transform.position, particles[i].position) > 0.1f) {
					Vector3 dir = (chargeParticles.transform.position - particles [i].position).normalized;
					float sqrMagnitude = (chargeParticles.transform.position - particles [i].position).sqrMagnitude;
					Vector3 velocity = dir * gravityFactor / sqrMagnitude;
					particles [i].velocity = velocity;
				}
			}
			// Apply the particle changes to the particle system
			chargeParticles.SetParticles(particles, numParticlesAlive);
		}
	}

	public override void ReleaseCharge ()
	{
		pullFire = false;
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
		transform.FindChild ("FlameTrail").GetComponent<ParticleSystem> ().startLifetime /= 2f;
		transform.FindChild ("FlameTrail").FindChild("Distortion").GetComponent<ParticleSystem> ().startLifetime /= 2f;
	}

	public override void FizzleCharge ()
	{
//		transform.FindChild ("ChargeParticle").GetChild (0).GetComponent<WindZone> ().windMain *= -1f;
	}
}
