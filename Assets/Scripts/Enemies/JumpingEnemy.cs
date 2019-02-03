using UnityEngine;

public class JumpingEnemy : Enemy
{
	[SerializeField] private float jumpHeight;
	[SerializeField] private float jumpTime;
	[SerializeField] [Range(0, 1)] private float jumpChance;

	protected override void Reset()
	{
		base.Reset();

		jumpHeight = 1.5f;
		jumpTime = 2f;
		jumpChance = 0.2f;
	}

	protected override IEnemyState InitializeStates()
	{
		var idle = new IdleState(this);
		var claimForward = new ClaimForwardState(this, claimerPrefab);
		var chanceRoller = new ChanceRollerState(this);
		var stepForward = new MoveForwardState(this, stepTime);
		var jumpForward = new JumpForwardState(this, jumpHeight, jumpTime);

		chanceRoller.AddEntry(jumpForward, Mathf.FloorToInt(jumpChance * 10));
		chanceRoller.AddEntry(stepForward, Mathf.FloorToInt((1 - jumpChance) * 10));

		idle.AddTransition(() => { return true; }, claimForward);
		claimForward.AddTransition(() => { return claimForward.IsDone(); }, chanceRoller);
		chanceRoller.AddTransition(() => { return chanceRoller.PickedState == stepForward; }, stepForward);
		chanceRoller.AddTransition(() => { return chanceRoller.PickedState == jumpForward; }, jumpForward);
		stepForward.AddTransition(() => { return stepForward.IsDone(); }, idle);
		jumpForward.AddTransition(() => { return jumpForward.IsDone(); }, idle);

		return idle;
	}
}
