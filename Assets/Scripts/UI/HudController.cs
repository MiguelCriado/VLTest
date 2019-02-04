using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HudController : MonoBehaviour
{
	[SerializeField] private Slider healthSlider;
	[SerializeField] private TextMeshProUGUI healthText;
	[SerializeField] private Slider reloadSlider;

	private GameObject player;
	private Weapon currentWeapon;

	private void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player");
	}

	private void Start()
	{
		if (player != null)
		{
			Health health = player.GetComponent<Health>();

			if (health != null)
			{
				healthSlider.maxValue = health.MaxHealth;
				healthSlider.value = health.CurrentHealth;
			}
		}
	}

	private void OnEnable()
	{
		if (player != null)
		{
			Health health = player.GetComponent<Health>();

			if (health != null)
			{
				health.OnHurt += OnPlayerHurt;
			}

			WeaponController weapon = player.GetComponentInChildren<WeaponController>();

			if (weapon != null)
			{
				weapon.OnWeaponChange += OnPlayerChangeWeapon;
			}
		}
	}

	private void OnDisable()
	{
		if (player != null)
		{
			Health health = player.GetComponent<Health>();

			if (health != null)
			{
				health.OnHurt -= OnPlayerHurt;
			}

			WeaponController weapon = player.GetComponentInChildren<WeaponController>();

			if (weapon != null)
			{
				weapon.OnWeaponChange += OnPlayerChangeWeapon;
			}
		}
	}

	private void Update()
	{
		RefreshReloadSlider();
	}

	private void OnPlayerHurt(int healthChange, int resultingHealth, Vector3 contactPoint, Vector3 normal, GameObject attacker)
	{
		healthSlider.value = resultingHealth;
		healthText.text = string.Format("{0}/{1}", Mathf.Clamp(resultingHealth, healthSlider.minValue, healthSlider.maxValue), healthSlider.maxValue);
	}

	private void OnPlayerChangeWeapon(Weapon newWeapon)
	{
		currentWeapon = newWeapon;
		RefreshReloadSlider();
	}

	private void RefreshReloadSlider()
	{
		if (currentWeapon != null)
		{
			float reloadStatus = Mathf.Clamp01((Time.time - currentWeapon.LastShotTime) / currentWeapon.Definition.FiringRate);

			if (reloadStatus != reloadSlider.value)
			{
				reloadSlider.value = reloadStatus;
			}
		}
	}
}
