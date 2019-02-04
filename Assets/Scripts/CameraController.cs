using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private Camera camera;
	[SerializeField] private List<VirtualCamera> mainCameraList;
	[SerializeField] private VirtualCamera auxiliarCamera;
	[Header("Settings")]
	[SerializeField] private KeyCode auxiliarCameraButton;
	[SerializeField] private KeyCode swapCameraButton;
	[SerializeField] private float rotationSpeed = 1;

	private int currentCameraIndex;
	private bool usingAuxiliarCamera;

	private void Awake()
	{
		currentCameraIndex = 0;
	}

	private void Start()
	{
		ActivateCamera(0);
	}

	private void Update()
	{
		ToggleAuxiliarCamera();
		SwapCamera();
		RotateCamera();
	}

	private void ToggleAuxiliarCamera()
	{
		if (Input.GetKeyDown(auxiliarCameraButton))
		{
			usingAuxiliarCamera = true;
			ActivateCamera(-1);
		}
		else if (Input.GetKeyUp(auxiliarCameraButton))
		{
			usingAuxiliarCamera = false;
			ActivateCamera(currentCameraIndex);
		}
	}

	private void SwapCamera()
	{
		if (usingAuxiliarCamera == false && Input.GetKeyDown(swapCameraButton))
		{
			int newCameraIndex = MathUtilities.Modulo(currentCameraIndex + 1, mainCameraList.Count);
			ActivateCamera(newCameraIndex);
		}
	}

	private void RotateCamera()
	{
		Vector2 rotation = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

		foreach (var camera in mainCameraList)
		{
			camera.Rotate(rotation * Time.deltaTime);
		}
	}

	private void ActivateCamera(int cameraIndex)
	{
		VirtualCamera activeCamera = null;

		for (int i = 0; i < mainCameraList.Count; i++)
		{
			if (i == cameraIndex)
			{
				activeCamera = mainCameraList[i];
				currentCameraIndex = i;
			}
		}

		if (cameraIndex == -1)
		{
			activeCamera = auxiliarCamera;
		}

		camera.transform.SetParent(activeCamera.CameraMount);
		camera.transform.localPosition = Vector3.zero;
		camera.transform.localRotation = Quaternion.identity;
		camera.cullingMask = activeCamera.CullingMask;
	}
}
