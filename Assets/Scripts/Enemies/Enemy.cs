using UnityEngine;

[RequireComponent(typeof(Health), typeof(MeshRenderer))]
public abstract class Enemy : MonoBehaviour
{
	public Transform Claimer;
	public Transform Target { get { return target; } }
	public float Size { get { return size; } }

	[Header("Prefabs")]
	[SerializeField] protected Transform claimerPrefab;
	[SerializeField] protected ParticleSystem hitParticles;
	[SerializeField] protected ParticleSystem deathParticles;
	[Header("Settings")]
	[SerializeField] protected float size;
	[SerializeField] protected int damageOnContact;
	[SerializeField] protected float stepTime;
	[SerializeField] protected Color aliveColor;
	[SerializeField] protected Color deadColor;

	protected Transform target;
	protected IEnemyState currentState;
	protected Health health;
	protected MeshRenderer meshRenderer;

	protected virtual void Reset()
	{
		size = 1;
		damageOnContact = 1;
		stepTime = 2f;
		aliveColor = Color.blue;
		deadColor = Color.gray;
		OnValidate();
	}

	protected virtual void OnValidate()
	{
		transform.localScale = Vector3.one * size;
		transform.localPosition = new Vector3(transform.position.x, size / 2, transform.position.z);
	}

	protected virtual void Awake()
	{
		currentState = InitializeStates();
		currentState.OnEnter();

		health = GetComponent<Health>();
		meshRenderer = GetComponent<MeshRenderer>();
	}

	protected virtual void Start()
	{
		RefreshColor();
	}

	protected virtual void OnEnable()
	{
		health.OnHurt += OnHurt;
		health.OnDeath += OnDeath;
	}

	protected virtual void OnDisable()
	{
		health.OnHurt -= OnHurt;
		health.OnDeath -= OnDeath;
	}

	protected void Update()
	{
		currentState.CheckTransitions();
		currentState.Update();
	}

	protected void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject == target.gameObject)
		{
			Health targetHealth = target.GetComponent<Health>();

			if (targetHealth != null)
			{
				targetHealth.Hurt(damageOnContact, collision.GetContact(0).point, collision.GetContact(0).normal);
			}

			health.Hurt(health.CurrentHealth, transform.position, Vector3.up);
		}
	}

	public void Initialize(Transform target)
	{
		this.target = target;
		transform.localPosition = new Vector3(transform.position.x, size / 2, transform.position.z);
		Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, target.position.z);
		transform.LookAt(targetPosition);
	}

	public void ChangeState(IEnemyState state)
	{
		currentState.OnExit();
		currentState = state;
		currentState.OnEnter();
	}

	protected abstract IEnemyState InitializeStates();

	private void OnHurt(int healthChange, int resultingHealth, Vector3 contactPoint, Vector3 normal)
	{
		ParticleSystem particles = Instantiate(hitParticles, contactPoint, Quaternion.identity);
		particles.transform.LookAt(contactPoint + normal);

		var main = particles.main;
		main.startColor = meshRenderer.material.color;
		particles.Play();

		RefreshColor();
	}

	private void OnDeath()
	{
		ParticleSystem particles = Instantiate(deathParticles, transform.position, transform.rotation);
		particles.transform.localScale = transform.localScale;

		var main = particles.main;
		main.startColor = meshRenderer.material.color;
		particles.Play();
	}

	private void RefreshColor()
	{
		Color newColor = Color.Lerp(deadColor, aliveColor, (float)health.CurrentHealth / health.MaxHealth);
		meshRenderer.material.color = newColor;
	}
}
