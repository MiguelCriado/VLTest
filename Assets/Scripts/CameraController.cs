using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public delegate void CameraChangeDelegate(VirtualCamera camera);

	public VirtualCamera ActiveCamera { get; private set; }

	public event CameraChangeDelegate OnCameraChange = delegate { };

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
	private GameManager gameManager;

	private void Awake()
	{
		currentCameraIndex = 0;
		gameManager = FindObjectOfType<GameManager>();
	}

	private void Start()
	{
		ActivateCamera(0);
	}

	private void Update()
	{
		if (gameManager == null || gameManager.GameState == GameState.Running)
		{
			ToggleAuxiliarCamera();
			SwapCamera();
			RotateCamera();
		}
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
		ActiveCamera = null;

		for (int i = 0; i < mainCameraList.Count; i++)
		{
			if (i == cameraIndex)
			{
				ActiveCamera = mainCameraList[i];
				currentCameraIndex = i;
			}
		}

		if (cameraIndex == -1)
		{
			ActiveCamera = auxiliarCamera;
		}

		camera.transform.SetParent(ActiveCamera.CameraMount);
		camera.transform.localPosition = Vector3.zero;
		camera.transform.localRotation = Quaternion.identity;
		camera.cullingMask = ActiveCamera.CullingMask;

		OnCameraChange?.Invoke(ActiveCamera);
	}
}
