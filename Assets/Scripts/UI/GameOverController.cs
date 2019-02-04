using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
	[SerializeField] private Canvas canvas;

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
		// TODO  
	}

	private void OnGameStateChange(GameState lastState, GameState newState)
	{
		if (lastState == GameState.GameOver)
		{
			canvas.gameObject.SetActive(false);
		}
		else if (newState == GameState.GameOver)
		{
			canvas.gameObject.SetActive(true);
		}
	}
}
