using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChanceRollerState : BaseEnemyState
{
	private struct Entry
	{
		public IEnemyState State;
		public int Weight;

		public Entry(IEnemyState state, int weight)
		{
			State = state;
			Weight = weight;
		}
	}

	public IEnemyState PickedState { get; private set; }

	private List<Entry> entries;

	public ChanceRollerState(Enemy context) : base(context)
	{
		entries = new List<Entry>();
	}

	public void AddEntry(IEnemyState state, int weight)
	{
		entries.Add(new Entry(state, weight));
	}

	public override bool IsDone()
	{
		return PickedState != null;
	}

	public override void OnEnter()
	{
		PickedState = null;
		int totalWeight = 0;

		foreach (var entry in entries)
		{
			totalWeight += entry.Weight;
		}

		int randomNumber = Random.Range(0, totalWeight);
		int cummulativeWeight = 0;
		var enumerator = entries.GetEnumerator();

		while (PickedState == null && enumerator.MoveNext())
		{
			cummulativeWeight += enumerator.Current.Weight;

			if (cummulativeWeight > randomNumber)
			{
				PickedState = enumerator.Current.State;
			}
		}
	}

	public override void Update()
	{
		
	}

	public override void OnExit()
	{
		PickedState = null;
	}
}
