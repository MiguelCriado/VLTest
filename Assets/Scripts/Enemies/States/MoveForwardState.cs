using UnityEngine;

public class MoveForwardState : BaseEnemyState
{
	private float stepTime;
	private float initialTime;
	private Vector3 pivot;
	private Vector3 axis;
	private float rotationSpeed;
	private Quaternion initialRotation;
	private bool isDone;

	public MoveForwardState(Enemy context, float stepTime) : base(context)
	{
		this.stepTime = stepTime;
	}

	public override bool IsDone()
	{
		return isDone;
	}

	public override void OnEnter()
	{
		isDone = false;
		initialTime = Time.time;
		pivot = context.transform.position + (context.transform.forward * context.Size / 2 + Vector3.down * context.Size / 2);
		axis = Vector3.Cross(Vector3.up, context.transform.forward);
		rotationSpeed = 90 / stepTime;

		initialRotation = context.transform.localRotation;
	}

	public override void Update()
	{
		if (isDone == false)
		{
			if (initialTime + stepTime > Time.time)
			{
				context.transform.RotateAround(pivot, axis, rotationSpeed * Time.deltaTime);
			}
			else
			{
				context.transform.localRotation = initialRotation;
				context.transform.localPosition = new Vector3(context.transform.localPosition.x, context.Size / 2, context.transform.localPosition.z);
				isDone = true;
			}
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
