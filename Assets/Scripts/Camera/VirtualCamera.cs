using UnityEngine;

public class VirtualCamera : MonoBehaviour
{
	public Transform CameraMount { get { return cameraMount; } }
	public LayerMask CullingMask { get { return cullingMask; } }
	public bool AllowShooting { get { return allowShooting; } }

	[Header("References")]
	[SerializeField] private Transform rotationJoint;
	[SerializeField] private Transform cameraMount;
	[SerializeField] private Transform target;
	[Header("Settings")]
	[SerializeField] private Vector2 rotationFactor;
	[SerializeField] private LayerMask cullingMask;
	[SerializeField] private bool allowShooting;

	private void LateUpdate()
	{
		if (target != null)
		{
			transform.position = target.position;
			transform.rotation = target.rotation;
		}
	}

	public void Rotate(Vector2 rotation)
	{
		rotationJoint.Rotate(new Vector3(rotation.y * rotationFactor.x, rotation.x * rotationFactor.y));
	}
}
