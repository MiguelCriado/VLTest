using UnityEngine;

public class ZigZagEnemy : Enemy
{
	[SerializeField] private int forwardSteps;
	[SerializeField] private int rightSteps;
	[SerializeField] private int leftSteps;

	protected override IEnemyState InitializeStates()
	{
		var idle = new IdleState(this);
		var claimForward = new ClaimForwardState(this, claimerPrefab);
		var moveForward = new MoveForwardState(this, stepTime);
		var forwardCounter = new CounterState(this, forwardSteps);
		var switchState = new SwitchState(this);
		var claimRight = new ClaimRightState(this, claimerPrefab);
		var moveRight = new MoveRightState(this, stepTime);
		var rightCounter = new CounterState(this, rightSteps);
		var claimLeft = new ClaimLeftState(this, claimerPrefab);
		var moveLeft = new MoveLeftState(this, stepTime);
		var leftCounter = new CounterState(this, leftSteps);


		idle.AddTransition(() => { return true; }, claimForward);

		claimForward.AddTransition(() => { return claimForward.IsDone(); }, moveForward);
		moveForward.AddTransition(() => { return moveForward.IsDone(); }, forwardCounter);
		forwardCounter.AddTransition(() => { return forwardCounter.IsAmountReached(); }, switchState);
		forwardCounter.AddTransition(() => { return forwardCounter.IsAmountReached() == false; }, claimForward);

		switchState.AddTransition(() => { return switchState.Value == true; }, claimRight);
		switchState.AddTransition(() => { return switchState.Value == false; }, claimLeft);

		claimRight.AddTransition(() => { return claimRight.IsDone(); }, moveRight);
		moveRight.AddTransition(() => { return moveRight.IsDone(); }, rightCounter);
		rightCounter.AddTransition(() => { return rightCounter.IsAmountReached(); }, idle);
		rightCounter.AddTransition(() => { return rightCounter.IsAmountReached() == false; }, claimRight);

		claimLeft.AddTransition(() => { return claimLeft.IsDone(); }, moveLeft);
		moveLeft.AddTransition(() => { return moveLeft.IsDone(); }, leftCounter);
		leftCounter.AddTransition(() => { return leftCounter.IsAmountReached(); }, idle);
		leftCounter.AddTransition(() => { return leftCounter.IsAmountReached() == false; }, claimLeft);

		return idle;
	}
}
