using UnityEngine;

public class Health : MonoBehaviour
{
	public delegate void HealthDelegate(int healthChange, int resultingHealth);
	public delegate void DeathDelegate();

	public int MaxHealth { get { return maxHealth; } }
	public int CurrentHealth { get { return currentHealth; } }

	public event HealthDelegate OnHurt;
	public event HealthDelegate OnHeal;
	public event DeathDelegate OnDeath;

	[SerializeField] private int maxHealth;
	[SerializeField] private int currentHealth;

	public void Start()
	{
		currentHealth = maxHealth;
	}

	public void Hurt(int damage)
	{
		currentHealth -= damage;

		OnHurt?.Invoke(-damage, currentHealth);

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
