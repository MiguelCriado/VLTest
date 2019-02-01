using UnityEngine;
using UnityEngine.UI;

public class AimController : MonoBehaviour
{
	[SerializeField] private RectTransform topMarker;
	[SerializeField] private RectTransform rightMarker;
	[SerializeField] private RectTransform bottomMarker;
	[SerializeField] private RectTransform leftMarker;

	private CanvasScaler canvasScaler;

	private void Start()
	{
		canvasScaler = transform.GetComponentInParent<CanvasScaler>();
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
