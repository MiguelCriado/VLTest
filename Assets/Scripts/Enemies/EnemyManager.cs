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
	private int enemyCount;
	private GameManager gameManager;
	private List<Enemy> aliveEnemies;
	private Enemy bossInstance;
	private GameObject player;

	private void Awake()
	{
		aliveEnemies = new List<Enemy>();
		lastSpawnTime = float.MinValue;
		nextSpawnInterval = 0;
		enemyCount = 0;

		gameManager = FindObjectOfType<GameManager>();
		player = GameObject.FindGameObjectWithTag("Player");
	}

	private void OnEnable()
	{
		gameManager.OnGameStateChange += OnGameStateChange;
	}

	private void OnDisable()
	{
		gameManager.OnGameStateChange -= OnGameStateChange;

	}

	private void Update()
	{
		if (gameManager.GameState == GameState.Running)
		{
			UpdateSpawnCycle();
		}
	}

	private void UpdateSpawnCycle()
	{
		if (aliveEnemies.Count < maxEnemiesAlive && Time.time > lastSpawnTime + nextSpawnInterval)
		{
			Enemy newEnemy = null;

			if (enemyCount >= enemiesBeforeBoss && bossInstance == null)
			{
				bossInstance = SpawnEnemy(boss);

				if (bossInstance != null)
				{
					newEnemy = bossInstance;

					Health bossHealth = bossInstance.GetComponent<Health>();

					if (bossHealth != null)
					{
						bossHealth.OnDeath += (GameObject attacker) => 
						{
							if (attacker != gameObject && attacker != bossInstance.gameObject)
							{
								gameManager.SetState(GameState.Victory);
							}
						};
					}
				}
			}
			else
			{
				newEnemy = SpawnRandomEnemy();
			}

			if (newEnemy != null)
			{
				aliveEnemies.Add(newEnemy);

				Health health = newEnemy.GetComponent<Health>();

				if (health != null)
				{
					health.OnDeath += (GameObject attacker) =>
					{
						Destroy(newEnemy.gameObject);
						aliveEnemies.Remove(newEnemy);
					};
				}

				enemyCount++;
				nextSpawnInterval = Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns);
				lastSpawnTime = Time.time;
			}
		}
	}

	private Enemy SpawnRandomEnemy()
	{
		Enemy result = null;
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

		if (enemyToSpawn != null)
		{
			result = SpawnEnemy(enemyToSpawn);
		}

		return result;
	}

	public Enemy SpawnEnemy(Enemy enemyPrefab)
	{
		Enemy result = null;
		Vector2 spawnPoint = Random.insideUnitCircle.normalized * spawnRadius;
		Vector3 spawnPosition = new Vector3(spawnPoint.x, 0, spawnPoint.y);

		result = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
		result.Initialize(player.transform);

		return result;
	}

	private void OnGameStateChange(GameState lastState, GameState newState)
	{
		if (newState == GameState.Victory)
		{
			while (aliveEnemies.Count > 0)
			{
				Enemy enemy = aliveEnemies[aliveEnemies.Count - 1];
				Health health = enemy.GetComponent<Health>();

				if (health != null)
				{
					health.Hurt(health.CurrentHealth, enemy.transform.position, Vector3.up, gameObject);
				}
			}
		}
	}
}
