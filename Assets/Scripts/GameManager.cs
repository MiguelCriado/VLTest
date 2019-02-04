using UnityEngine;

public class GameManager : MonoBehaviour
{
	public delegate void GameStateChange(GameState lastState, GameState newState);

	public GameState GameState { get; private set; }

	public event GameStateChange OnGameStateChange;

	private GameObject player;

	private void Awake()
	{
		GameState = GameState.Idle;
		player = GameObject.FindGameObjectWithTag("Player");
	}

	private void OnEnable()
	{
		if (player != null)
		{
			Health playerHealth = player.GetComponent<Health>();

			if (playerHealth != null)
			{
				playerHealth.OnDeath += OnPlayerDeath;
			}
		}
	}

	private void OnDisable()
	{
		if (player != null)
		{
			Health playerHealth = player.GetComponent<Health>();

			if (playerHealth != null)
			{
				playerHealth.OnDeath -= OnPlayerDeath;
			}
		}
	}

	private void Start()
	{
		SetState(GameState.Running);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (GameState == GameState.Running)
			{
				SetState(GameState.Paused);
			}
			else if (GameState == GameState.Paused)
			{
				SetState(GameState.Running);
			}
		}
	}

	public void SetState(GameState newState)
	{
		if (newState != GameState)
		{
			GameState lastState = GameState;
			GameState = newState;

			if (GameState == GameState.Paused)
			{
				Time.timeScale = 0;
			}
			else if (GameState == GameState.Running)
			{
				Time.timeScale = 1;
			}

			OnGameStateChange?.Invoke(lastState, GameState);
		}
	}

	private void OnPlayerDeath(GameObject attacker)
	{
		SetState(GameState.GameOver);
	}
}
