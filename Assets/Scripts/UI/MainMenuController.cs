using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
	[SerializeField] private string gameplayScene;

	public void StartGame()
	{
		SceneManager.LoadScene(gameplayScene);
	}

	public void Quit()
	{
		Application.Quit();
	}
}
