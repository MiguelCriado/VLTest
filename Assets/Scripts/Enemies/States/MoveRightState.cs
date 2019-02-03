using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRightState : BaseEnemyState
{
	private float stepTime;
	private float initialTime;
	private Vector3 pivot;
	private float edgeRotationSpeed;
	private float upRotationSpeed;
	private bool isDone;

	public MoveRightState(Enemy context, float stepTime) : base(context)
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

		Vector3 edgeCenter = context.transform.position + (context.transform.forward * context.Size / 2) - (context.transform.up * context.Size / 2);
		Vector3 rightVertex = edgeCenter + (context.transform.right * context.Size / 2);
		Vector3 leftVertex = edgeCenter - (context.transform.right * context.Size / 2);

		Vector3 rightVector = rightVertex - context.Target.position;
		Vector3 leftVector = leftVertex - context.Target.position;

		pivot = rightVertex;
		float angle = Vector3.Angle(rightVector, leftVector);

		upRotationSpeed = angle / stepTime;
		edgeRotationSpeed = 90 / stepTime;
	}

	public override void Update()
	{
		if (isDone == false)
		{
			if (initialTime + stepTime > Time.time)
			{
				context.transform.RotateAround(pivot, -context.transform.forward, edgeRotationSpeed * Time.deltaTime);
				context.transform.RotateAround(pivot, -context.transform.up, upRotationSpeed * Time.deltaTime);
			}
			else
			{
				context.transform.localRotation = Quaternion.identity;
				context.transform.localPosition = new Vector3(context.transform.localPosition.x, context.Size / 2, context.transform.localPosition.z);
				Vector3 targetPosition = new Vector3(context.Target.position.x, context.transform.position.y, context.Target.position.z);
				context.transform.LookAt(targetPosition);
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
