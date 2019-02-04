using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class DestroyParticlesOnFinish : MonoBehaviour
{
	private ParticleSystem particles;

	private void Awake()
	{
		particles = GetComponent<ParticleSystem>();
	}

	private void Update()
	{
		if (particles.IsAlive() == false)
		{
			Destroy(gameObject);
		}
	}
}
