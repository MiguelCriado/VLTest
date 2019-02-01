using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private List<CinemachineVirtualCamera> mainCameraList;
	[SerializeField] private CinemachineVirtualCamera auxiliarCamera;
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
		CinemachineComposer composer = mainCameraList[currentCameraIndex].GetCinemachineComponent<CinemachineComposer>();
		
		if (composer != null)
		{
			float verticalRotation = Input.GetAxis("Mouse Y");
			composer.m_TrackedObjectOffset.y += verticalRotation * rotationSpeed * Time.deltaTime;
		}
	}

	private void ActivateCamera(int cameraIndex)
	{
		for (int i = 0; i < mainCameraList.Count; i++)
		{
			if (i == cameraIndex)
			{
				mainCameraList[i].Priority = 1;
				currentCameraIndex = i;
			}
			else
			{
				mainCameraList[i].Priority = 0;
			}
		}

		if (cameraIndex == -1)
		{
			auxiliarCamera.Priority = 1;
		}
	}
}
