using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
	public float Size { get { return size; } }

	[SerializeField] protected float size;
	[SerializeField] protected float stepTime;

	protected IEnemyState currentState;

	protected virtual void Reset()
	{
		size = 1;
		stepTime = 2f;
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
	}

	protected void Update()
	{
		currentState.CheckTransitions();
		currentState.Update();
	}

	public void ChangeState(IEnemyState state)
	{
		currentState.OnExit();
		currentState = state;
		currentState.OnEnter();
	}

	protected abstract IEnemyState InitializeStates();

}
