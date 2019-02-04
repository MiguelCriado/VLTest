using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HudController : MonoBehaviour
{
	[SerializeField] private Slider healthSlider;
	[SerializeField] private TextMeshProUGUI healthText;

	private GameObject player;

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
		}
	}

	private void OnPlayerHurt(int healthChange, int resultingHealth, Vector3 contactPoint, Vector3 normal, GameObject attacker)
	{
		healthSlider.value = resultingHealth;
		healthText.text = string.Format("{0}/{1}", resultingHealth, healthSlider.maxValue);
	}
}
