using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClaimLeftState : BaseEnemyState
{
	private static readonly Collider[] OverlapColliders;

	private Transform claimerPrefab;
	private int layerMask;
	private Transform claimer;
	private bool isDone;

	public ClaimLeftState(Enemy context, Transform claimerPrefab) : base(context)
	{
		this.claimerPrefab = claimerPrefab;
		layerMask = 1 << LayerMask.NameToLayer("Enemy");
		layerMask |= 1 << LayerMask.NameToLayer("Claimer");
	}

	public override bool IsDone()
	{
		return isDone;
	}

	public override void OnEnter()
	{
		isDone = false;

		Vector3 edgeCenter = context.transform.position + (context.transform.forward * context.Size / 2) - (context.transform.up * context.Size / 2);
		Vector3 rightVertex = edgeCenter + (context.transform.right * context.Size / 2);
		Vector3 leftVertex = edgeCenter - (context.transform.right * context.Size / 2);

		Vector3 rightVector = rightVertex - context.Target.position;
		Vector3 leftVector = leftVertex - context.Target.position;

		float angle = Vector3.Angle(rightVector, leftVector);

		claimer = Object.Instantiate(claimerPrefab, context.transform.position, context.transform.rotation);
		claimer.localScale = Vector3.one * context.Size;
		claimer.RotateAround(context.Target.position, -Vector3.up, angle);
		claimer.gameObject.SetActive(false);
	}

	public override void Update()
	{
		if (Physics.OverlapBoxNonAlloc(claimer.position, Vector3.one * context.Size, OverlapColliders, claimer.rotation, layerMask) == 0)
		{
			context.Claimer = claimer;
			claimer.gameObject.SetActive(true);
			isDone = true;
		}
	}

	public override void OnExit()
	{

	}
}
