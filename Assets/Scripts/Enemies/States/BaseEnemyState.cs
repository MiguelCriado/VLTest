using System;
using System.Collections.Generic;

public abstract class BaseEnemyState : IEnemyState
{
	protected Enemy context;
	protected Dictionary<Func<bool>, IEnemyState> transitions;

	public BaseEnemyState(Enemy context)
	{
		transitions = new Dictionary<Func<bool>, IEnemyState>();
		this.context = context;
	}

	public void SetContext(Enemy context)
	{
		this.context = context;
	}

	public void AddTransition(Func<bool> condition, IEnemyState newState)
	{
		transitions[condition] = newState;
	}

	public void CheckTransitions()
	{
		IEnemyState newState = null;
		var enumerator = transitions.GetEnumerator();

		while (newState == null && enumerator.MoveNext())
		{
			if (enumerator.Current.Key.Invoke())
			{
				newState = enumerator.Current.Value;
			}
		}

		if (newState != null)
		{
			context.ChangeState(newState);
		}
	}

	public abstract bool IsDone();

	public abstract void OnEnter();

	public abstract void Update();

	public abstract void OnExit();
}
