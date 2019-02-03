public class CounterState : BaseEnemyState
{
	private int amount;
	private int count;

	public CounterState(Enemy context, int amount) : base(context)
	{
		this.amount = amount;
		count = 0;
	}

	public bool IsAmountReached()
	{
		bool result = false;

		if (count >= amount)
		{
			result = true;
		}

		return result;
	}

	public override bool IsDone()
	{
		return true;
	}

	public override void OnEnter()
	{
		count++;
	}

	public override void Update()
	{

	}

	public override void OnExit()
	{
		if (count >= amount)
		{
			count = 0;
		}
	}
}
