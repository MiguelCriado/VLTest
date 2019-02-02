public class SimpleEnemy : Enemy
{
	protected override IEnemyState InitializeStates()
	{
		var idle = new IdleState(this);
		var stepForward = new MoveForwardState(this, stepTime);

		idle.AddTransition(() => { return true; }, stepForward);
		stepForward.AddTransition(() => { return stepForward.IsDone(); }, idle);

		return idle;
	}
}
