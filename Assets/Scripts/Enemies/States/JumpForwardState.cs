using UnityEngine;

public class JumpForwardState : BaseEnemyState
{
	private float jumpHeight;
	private float jumpTime;
	private Vector3 initialPosition;
	private Vector3 finalPosition;
	private float initialJumpTime;
	private bool isDone;

	public JumpForwardState(Enemy context, float jumpHeight, float jumpTime) : base(context)
	{
		this.jumpHeight = jumpHeight;
		this.jumpTime = jumpTime;
	}

	public override bool IsDone()
	{
		return isDone;
	}

	public override void OnEnter()
	{
		isDone = false;
		initialPosition = context.transform.position;
		finalPosition = context.transform.position + context.transform.forward * context.Size;
		initialJumpTime = Time.time;
	}

	public override void Update()
	{
		float elapsedTime = Time.time - initialJumpTime;

		if (elapsedTime < jumpTime)
		{
			context.transform.position = MathUtilities.Parabola(initialPosition, finalPosition, jumpHeight, elapsedTime / jumpTime);
		}
		else
		{
			context.transform.position = finalPosition;
			isDone = true;
		}
	}

	public override void OnExit()
	{
		if (context.Claimer != null)
		{
			Object.Destroy(context.Claimer.gameObject);
		}
	}
}
