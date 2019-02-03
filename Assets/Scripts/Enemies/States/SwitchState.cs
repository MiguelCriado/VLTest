public class SwitchState : BaseEnemyState
{
	public bool Value { get; private set; }

	public SwitchState(Enemy context) : base(context)
	{

	}

	public override bool IsDone()
	{
		return true;
	}

	public override void OnEnter()
	{
		Value = !Value;
	}

	public override void OnExit()
	{

	}

	public override void Update()
	{

	}
}
