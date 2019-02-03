using UnityEngine;

public class Decay : MonoBehaviour
{
	[SerializeField] private float deathTime;

	private float finalTime;

	private void Start()
	{
		finalTime = Time.time + deathTime;
	}

	private void Update()
	{
		if (Time.time >= finalTime)
		{
			Destroy(gameObject);
		}
	}
}
