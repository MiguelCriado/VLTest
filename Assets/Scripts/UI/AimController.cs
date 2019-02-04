using UnityEngine;
using UnityEngine.UI;

public class AimController : MonoBehaviour
{
	[SerializeField] private Canvas canvas;
	[SerializeField] private RectTransform topMarker;
	[SerializeField] private RectTransform rightMarker;
	[SerializeField] private RectTransform bottomMarker;
	[SerializeField] private RectTransform leftMarker;

	private CanvasScaler canvasScaler;
	private GameManager gameManager;
	private CameraController cameraController;

	private void Awake()
	{
		canvasScaler = transform.GetComponentInParent<CanvasScaler>();

		if (canvasScaler == null)
		{
			canvasScaler = transform.GetComponentInChildren<CanvasScaler>();
		}

		gameManager = FindObjectOfType<GameManager>();
		cameraController = FindObjectOfType<CameraController>();
	}

	private void OnEnable()
	{
		SubscribeEvents();
	}

	private void OnDisable()
	{
		UnsubscribeEvents();
	}

	private void OnGameManagerStateChange(GameState lastState, GameState newState)
	{
		RefreshVisibility();
	}

	private void OnWeaponChange(Weapon weapon)
	{
		if (canvasScaler != null)
		{
			float displacement = canvasScaler.referenceResolution.y * weapon.Definition.Dispersion;

			topMarker.anchoredPosition = new Vector2(topMarker.anchoredPosition.x, displacement);
			rightMarker.anchoredPosition = new Vector2(displacement, rightMarker.anchoredPosition.y);
			bottomMarker.anchoredPosition = new Vector2(bottomMarker.anchoredPosition.x, -displacement);
			leftMarker.anchoredPosition = new Vector2(-displacement, leftMarker.anchoredPosition.y);
		}
	}

	private void OnCameraChange(VirtualCamera camera)
	{
		RefreshVisibility();
	}

	private void RefreshVisibility()
	{
		canvas.gameObject.SetActive(gameManager.GameState == GameState.Running && (cameraController.ActiveCamera == null || cameraController.ActiveCamera.AllowShooting));

		if (gameManager.GameState == GameState.Running)
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
		else
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
	}

	private void SubscribeEvents()
	{
		GameObject player = GameObject.FindGameObjectWithTag("Player");

		if (player != null)
		{
			WeaponController weaponController = player.GetComponent<WeaponController>();

			if (weaponController != null)
			{
				weaponController.OnWeaponChange += OnWeaponChange;
			}
		}

		if (gameManager != null)
		{
			gameManager.OnGameStateChange += OnGameManagerStateChange;
		}

		if (cameraController != null)
		{
			cameraController.OnCameraChange += OnCameraChange;
		}
	}

	private void UnsubscribeEvents()
	{
		GameObject player = GameObject.FindGameObjectWithTag("Player");

		if (player != null)
		{
			WeaponController weaponController = player.GetComponent<WeaponController>();

			if (weaponController != null)
			{
				weaponController.OnWeaponChange -= OnWeaponChange;
			}
		}

		if (gameManager != null)
		{
			gameManager.OnGameStateChange -= OnGameManagerStateChange;
		}

		if (cameraController != null)
		{
			cameraController.OnCameraChange += OnCameraChange;
		}
	}
}
