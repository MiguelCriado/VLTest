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
	[SerializeField] private Vector2 minRotation;
	[SerializeField] private Vector2 maxRotation;
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
		Vector2 rotationToApply = new Vector3(rotation.y * rotationFactor.x, rotation.x * rotationFactor.y);
		Vector2 currentRotation = rotationJoint.localRotation.eulerAngles;

		if (currentRotation.x > 180)
		{
			currentRotation.x = (currentRotation.x - 360);
		}

		if (currentRotation.y > 180)
		{
			currentRotation.y = (currentRotation.y - 360);
		}

		Vector2 finalRotation = new Vector2
		{
			x = Mathf.Clamp(currentRotation.x + rotationToApply.x, minRotation.x, maxRotation.x),
			y = Mathf.Clamp(currentRotation.y + rotationToApply.y, minRotation.y, maxRotation.y)
		};

		Vector2 curatedRotation = finalRotation - currentRotation;
		rotationJoint.Rotate(curatedRotation);
	}
}
