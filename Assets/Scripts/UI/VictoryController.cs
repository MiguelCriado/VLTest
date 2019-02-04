using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryController : MonoBehaviour
{
	[SerializeField] private Canvas canvas;
	[SerializeField] private string menuScene;

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

	public void Replay()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void GoToMenu()
	{
		SceneManager.LoadScene(menuScene);
	}

	private void OnGameStateChange(GameState lastState, GameState newState)
	{
		if (lastState == GameState.Victory)
		{
			canvas.gameObject.SetActive(false);
		}
		else if (newState == GameState.Victory)
		{
			canvas.gameObject.SetActive(true);
		}
	}
}
