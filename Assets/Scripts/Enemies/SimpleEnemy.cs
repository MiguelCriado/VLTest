public class SimpleEnemy : Enemy
{
	protected override IEnemyState InitializeStates()
	{
		var idle = new IdleState(this);
		var claimForward = new ClaimForwardState(this, claimerPrefab);
		var stepForward = new MoveForwardState(this, stepTime);

		idle.AddTransition(() => { return true; }, claimForward);
		claimForward.AddTransition(() => { return claimForward.IsDone(); }, stepForward);
		stepForward.AddTransition(() => { return stepForward.IsDone(); }, idle);

		return idle;
	}
}
