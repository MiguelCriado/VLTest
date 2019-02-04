using UnityEngine;

public class Weapon : MonoBehaviour
{
	public Transform Muzzle { get { return muzzle; } }
	public WeaponDefinition Definition { get { return definition; } }
	public bool IsReloaded { get { return Time.time - LastShotTime >= definition.FiringRate; } }
	public float LastShotTime { get; set; }

	[SerializeField] private Transform muzzle;
	[SerializeField] private WeaponDefinition definition;

	private void Awake()
	{
		LastShotTime = float.MinValue;
	}
}
