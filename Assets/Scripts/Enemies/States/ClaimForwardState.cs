using UnityEngine;

public class ClaimForwardState : BaseEnemyState
{
	private static readonly Collider[] OverlapColliders;

	private Transform claimerPrefab;
	private int layerMask;
	private Vector3 position;
	private Quaternion rotation;
	private bool isDone;

	public ClaimForwardState(Enemy context, Transform claimerPrefab) : base(context)
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
		position = context.transform.position + context.transform.forward * context.Size;
		rotation = context.transform.rotation;
	}

	public override void Update()
	{
		if (Physics.OverlapBoxNonAlloc(position, Vector3.one * context.Size, OverlapColliders, rotation, layerMask) == 0)
		{
			Transform claimer = Object.Instantiate(claimerPrefab, position, rotation);
			claimer.localScale = Vector3.one * context.Size;
			context.Claimer = claimer;
			isDone = true;
		}
	}

	public override void OnExit()
	{
		
	}
}
