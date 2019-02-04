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

	private void Awake()
	{
		canvasScaler = transform.GetComponentInParent<CanvasScaler>();

		if (canvasScaler == null)
		{
			canvasScaler = transform.GetComponentInChildren<CanvasScaler>();
		}

		gameManager = FindObjectOfType<GameManager>();
	}

	private void OnEnable()
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

		gameManager.OnGameStateChange += OnGameManagerStateChange;
	}

	private void OnDisable()
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

		gameManager.OnGameStateChange -= OnGameManagerStateChange;
	}

	private void OnGameManagerStateChange(GameState lastState, GameState newState)
	{
		canvas.gameObject.SetActive(newState == GameState.Running);

		if (newState == GameState.Running)
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

	private void OnWeaponChange(Weapon weapon)
	{
		if (canvasScaler != null)
		{
			float displacement = canvasScaler.referenceResolution.y * weapon.Dispersion;

			topMarker.anchoredPosition = new Vector2(topMarker.anchoredPosition.x, displacement);
			rightMarker.anchoredPosition = new Vector2(displacement, rightMarker.anchoredPosition.y);
			bottomMarker.anchoredPosition = new Vector2(bottomMarker.anchoredPosition.x, -displacement);
			leftMarker.anchoredPosition = new Vector2(-displacement, leftMarker.anchoredPosition.y);
		}
	}
}
