using UnityEngine;

public class PauseController : MonoBehaviour
{
	[SerializeField] private Transform canvas;

	private GameManager gameManager;

	private void Awake()
	{
		gameManager = FindObjectOfType<GameManager>();
	}

	private void OnEnable()
	{
		gameManager.OnGameStateChange += OnGameStateChange;
	}

	private void OnDisable()
	{
		gameManager.OnGameStateChange -= OnGameStateChange;
	}

	public void Resume()
	{
		gameManager.SetState(GameState.Running);
	}

	public void GoToMenu()
	{
		// TODO  
	}

	private void OnGameStateChange(GameState lastState, GameState newState)
	{
		if (lastState == GameState.Paused)
		{
			canvas.gameObject.SetActive(false);
		}
		else if (newState == GameState.Paused)
		{
			canvas.gameObject.SetActive(true);
		}
	}
}
