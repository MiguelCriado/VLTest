using UnityEngine;

public class ClaimForwardState : BaseEnemyState
{
	private static readonly Collider[] OverlapColliders = new Collider[20];

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
		int hitCount = Physics.OverlapBoxNonAlloc(position, Vector3.one * context.Size, OverlapColliders, rotation, layerMask);

		if (hitCount == 0 || CheckOtherObject(OverlapColliders, hitCount) == false)
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

	private bool CheckOtherObject(Collider[] colliders, int hitCount)
	{
		bool result = false;
		int i = 0;

		while (result == false && i < hitCount)
		{
			if (colliders[i].gameObject != context.gameObject)
			{
				result = true;
			}

			i++;
		}

		return result;
	}
}
