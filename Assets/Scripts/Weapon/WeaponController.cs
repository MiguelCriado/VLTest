﻿using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
	public delegate void WeaponChangeDelegate(Weapon newWeapon);

	public event WeaponChangeDelegate OnWeaponChange;

	[Header("Prefabs")]
	[SerializeField] private LineRenderer bulletTrailPrefab;
	[Header("References")]
 	[SerializeField] private List<Weapon> availableWeapons;
	[SerializeField] private Transform weaponLocation;
	[SerializeField] private Light shootLight;
	[Header("Settings")]
	[SerializeField] private KeyCode shootButton;
	[SerializeField] private LayerMask shootMask;
	[SerializeField] private float lightTime;
	[SerializeField] private float weaponAdjustSpeed;

	private Weapon currentWeapon;
	private float lastShootTime;
	private GameManager gameManager;
	private CameraController cameraController;

	private void Awake()
	{
		gameManager = FindObjectOfType<GameManager>();
		cameraController = FindObjectOfType<CameraController>();
	}

	private void Start()
	{
		if (availableWeapons.Count > 0)
		{
			MountWeapon(availableWeapons[0]);
		}
	}

	private void Update()
	{
		if (gameManager.GameState == GameState.Running)
		{
			SwapWeapon();
			UpdateShoot();
			UpdateLight();
		}
	}

	private void LateUpdate()
	{
		if (gameManager.GameState == GameState.Running)
		{
			UpdateWeaponRotation();
		}
	}

	public void Shoot()
	{
		if (currentWeapon != null && (cameraController == null || cameraController.ActiveCamera.AllowShooting))
		{
			WeaponDefinition definition = currentWeapon.Definition; ;
			float aimBound = Camera.main.pixelHeight * definition.Dispersion;

			for (int i = 0; i < definition.ProjectilesPerShot; i++)
			{
				Vector2 shotOrigin = Random.insideUnitCircle * aimBound;
				Vector3 screenPoint = new Vector3(Camera.main.pixelWidth / 2 + shotOrigin.x, Camera.main.scaledPixelHeight / 2 + shotOrigin.y, 0);
				Ray ray = Camera.main.ScreenPointToRay(screenPoint);
				RaycastHit[] hits = Physics.RaycastAll(ray, 100, shootMask);

				LineRenderer line = Instantiate(bulletTrailPrefab);
				line.SetPosition(0, currentWeapon.Muzzle.position);

				if (hits.Length > 0)
				{
					RaycastHit firstHit = hits[0];

					Health health = firstHit.collider.GetComponent<Health>();
					health.Hurt(definition.DamagePerProjectile, firstHit.point, firstHit.normal, gameObject);

					line.SetPosition(1, firstHit.point);
				}
				else
				{
					line.SetPosition(1, ray.origin + ray.direction.normalized * 100);
				}
			}

			shootLight.transform.position = currentWeapon.Muzzle.position;
			shootLight.gameObject.SetActive(true);

			currentWeapon.LastShotTime = Time.time;
			lastShootTime = Time.time;
		}
	}

	private void SwapWeapon()
	{
		if (Input.mouseScrollDelta.y != 0)
		{
			int step = 1;

			if (Input.mouseScrollDelta.y < 0)
			{
				step = -1;
			}

			int currentWeaponIndex = availableWeapons.FindIndex(x => x == currentWeapon);
			int newWeaponIndex = MathUtilities.Modulo(currentWeaponIndex + step, availableWeapons.Count);

			MountWeapon(availableWeapons[newWeaponIndex]);
		}
		else
		{
			Weapon newWeapon = null;

			foreach (var weapon in availableWeapons)
			{
				if (Input.GetKeyDown(weapon.SwitchCode))
				{
					newWeapon = weapon;
				}
			}

			if (newWeapon != null && newWeapon != currentWeapon)
			{
				MountWeapon(newWeapon);
			}
		}
	}

	private void UpdateWeaponRotation()
	{
		if (currentWeapon != null && (cameraController == null || cameraController.ActiveCamera.AllowShooting))
		{
			Vector3 screenPoint = new Vector3(Camera.main.pixelWidth / 2, Camera.main.scaledPixelHeight / 2, 0);
			Ray ray = Camera.main.ScreenPointToRay(screenPoint);
			RaycastHit[] hits = Physics.RaycastAll(ray, 100, shootMask);
			Vector3 target;

			if (hits.Length > 0)
			{
				target = hits[0].point;
			}
			else
			{
				target = ray.origin + ray.direction * 100;
			}

			Quaternion targetRotation = Quaternion.LookRotation(target - currentWeapon.transform.position);
			currentWeapon.transform.rotation = Quaternion.Slerp(currentWeapon.transform.rotation, targetRotation, Time.deltaTime * weaponAdjustSpeed);
		}
	}

	private void UpdateShoot()
	{
		if (Input.GetKey(shootButton) && currentWeapon.IsReloaded)
		{
			Shoot();
		}
	}

	private void UpdateLight()
	{
		if (shootLight.gameObject.activeSelf && Time.time - lastShootTime > lightTime)
		{
			shootLight.gameObject.SetActive(false);
		}
	}

	private void MountWeapon(Weapon weapon)
	{
		if (currentWeapon != null)
		{
			currentWeapon.gameObject.SetActive(false);
		}

		currentWeapon = weapon;
		currentWeapon.gameObject.SetActive(true);
		currentWeapon.transform.SetParent(weaponLocation);
		currentWeapon.transform.localPosition = Vector3.zero;
		currentWeapon.transform.localRotation = Quaternion.identity;

		OnWeaponChange?.Invoke(weapon);
	}
}
