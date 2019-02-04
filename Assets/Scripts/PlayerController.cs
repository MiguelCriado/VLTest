using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private float rotationSpeed = 1;

	private GameManager gameManager;

	private void Awake()
	{
		gameManager = FindObjectOfType<GameManager>();
	}

	private void Update()
	{
		if (gameManager == null || gameManager.GameState == GameState.Running)
		{
			float horizontalMovement = Input.GetAxis("Mouse X");
			transform.Rotate(new Vector3(0, horizontalMovement * rotationSpeed * Time.deltaTime, 0));
		}
	}
}
