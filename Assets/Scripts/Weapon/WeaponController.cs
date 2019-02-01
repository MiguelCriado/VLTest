using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
	public delegate void WeaponChangeDelegate(Weapon newWeapon);

	public event WeaponChangeDelegate OnWeaponChange;

	[SerializeField] private List<Weapon> availableWeapons;
	[SerializeField] private Transform weaponLocation;
	[SerializeField] private KeyCode shootButton;
	[SerializeField] private LayerMask shootMask;

	private Weapon currentWeapon;
	private WeaponModel currentModel;
	private float lastShootTime;

	private void Start()
	{
		if (availableWeapons.Count > 0)
		{
			MountWeapon(availableWeapons[0]);
		}
	}

	private void Update()
	{
		SwapWeapon();

		if (Input.GetKeyDown(shootButton))
		{
			Shoot();
		}
	}

	public void Shoot()
	{
		if (currentWeapon != null)
		{
			float aimBound = Camera.main.pixelHeight * currentWeapon.Dispersion;

			for (int i = 0; i < currentWeapon.ProjectilesPerShot; i++)
			{
				Vector2 shotOrigin = Random.insideUnitCircle * aimBound;
				Vector3 screenPoint = new Vector3(Camera.main.pixelWidth / 2 + shotOrigin.x, Camera.main.scaledPixelHeight / 2 + shotOrigin.y, 0);
				Ray ray = Camera.main.ScreenPointToRay(screenPoint);
				RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, shootMask);

				Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 0.5f);

				if (hits.Length > 0)
				{
					RaycastHit firstHit = hits[0];

					Health health = firstHit.collider.GetComponent<Health>();
					health.Hurt(currentWeapon.DamagePerProjectile);
				}
			}
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
	}

	private void MountWeapon(Weapon weapon)
	{
		if (currentModel != null)
		{
			Destroy(currentModel.gameObject);
		}

		currentWeapon = weapon;
		currentModel = Instantiate(weapon.ModelPrefab, weaponLocation);

		OnWeaponChange?.Invoke(weapon);
	}
}
