using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private float rotationSpeed = 1;

	private void Update()
	{
		float horizontalMovement = Input.GetAxis("Mouse X");
		transform.Rotate(new Vector3(0, horizontalMovement * rotationSpeed * Time.deltaTime, 0));
	}
}
