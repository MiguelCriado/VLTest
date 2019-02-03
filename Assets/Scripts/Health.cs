using UnityEngine;

public class Health : MonoBehaviour
{
	public delegate void HurtDelegate(int healthChange, int resultingHealth, Vector3 contactPoint, Vector3 normal);
	public delegate void HealDelegate(int healthChange, int resultingHealth);
	public delegate void DeathDelegate();

	public int MaxHealth { get { return maxHealth; } }
	public int CurrentHealth { get { return currentHealth; } }

	public event HurtDelegate OnHurt;
	public event HealDelegate OnHeal;
	public event DeathDelegate OnDeath;

	[SerializeField] private int maxHealth;
	[SerializeField] private int currentHealth;

	public void Start()
	{
		currentHealth = maxHealth;
	}

	public void Hurt(int damage, Vector3 contactPoint, Vector3 normal)
	{
		currentHealth -= damage;

		OnHurt?.Invoke(-damage, currentHealth, contactPoint, normal);

		if (currentHealth <= 0)
		{
			OnDeath?.Invoke();
		}
	}

	public void Heal(int amount)
	{
		int effectiveAmount = maxHealth - (currentHealth + amount) + amount;
		currentHealth += effectiveAmount;

		OnHeal?.Invoke(amount, effectiveAmount);
	}
}
