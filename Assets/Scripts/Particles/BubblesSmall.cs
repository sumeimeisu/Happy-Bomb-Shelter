using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubblesSmall : MonoBehaviour 
{
	ParticleSystem ps;
	ParticleSystem.Particle[] particles;

	void Start () 
	{
		ps = GetComponent<ParticleSystem>();

		particles = new ParticleSystem.Particle[ps.main.maxParticles];
	}
	
	void Update () 
	{
		int numParticles = ps.GetParticles(particles);

		for (int i = 0; i < numParticles; i++)
		{
			if (particles[i].position.y > 0) particles[i].remainingLifetime = 0;
		}

		ps.SetParticles(particles, numParticles);
	}
}
