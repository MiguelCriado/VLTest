using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
	[Header("Prefabs")]
	[SerializeField] private List<EnemyEntry> availableEnemies;
	[SerializeField] private Enemy boss;
	[Header("Settings")]
	[SerializeField] private int enemiesBeforeBoss;
	[SerializeField] private int maxEnemiesAlive;
	[SerializeField] private float minTimeBetweenSpawns;
	[SerializeField] private float maxTimeBetweenSpawns;
	[SerializeField] private float spawnRadius;

	private float lastSpawnTime;
	private float nextSpawnInterval;

	private List<Enemy> aliveEnemies;
	private Transform player;

	private void Awake()
	{
		aliveEnemies = new List<Enemy>();
		lastSpawnTime = float.MinValue;
		nextSpawnInterval = 0;
	}

	private void Start()
	{
		GameObject go = GameObject.FindGameObjectWithTag("Player");

		if (go != null)
		{
			player = go.transform;
		}
	}

	private void Update()
	{
		UpdateSpawnCycle();
	}

	private void UpdateSpawnCycle()
	{
		if (aliveEnemies.Count < maxEnemiesAlive && Time.time > lastSpawnTime + nextSpawnInterval)
		{
			SpawnEnemy();
			nextSpawnInterval = Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns);
			lastSpawnTime = Time.time;
		}
	}

	private void SpawnEnemy()
	{
		int totalWeight = 0;

		foreach (var entry in availableEnemies)
		{
			totalWeight += entry.Weight;
		}

		int randomNumber = Random.Range(0, totalWeight);
		int cummulativeWeight = 0;
		Enemy enemyToSpawn = null;
		var enumerator = availableEnemies.GetEnumerator();

		while (enemyToSpawn == null && enumerator.MoveNext())
		{
			cummulativeWeight += enumerator.Current.Weight;

			if (cummulativeWeight > randomNumber)
			{
				enemyToSpawn = enumerator.Current.Enemy;
			}
		}

		Vector2 spawnPoint = Random.insideUnitCircle.normalized * spawnRadius;
		Vector3 spawnPosition = new Vector3(spawnPoint.x, 0, spawnPoint.y);

		Enemy newEnemy = Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
		newEnemy.Initialize(player);
		aliveEnemies.Add(newEnemy);

		Health health = newEnemy.GetComponent<Health>();

		if (health != null)
		{
			health.OnDeath += () =>
			{
				Destroy(newEnemy.gameObject);
				aliveEnemies.Remove(newEnemy);
			};
		}
	}
}
