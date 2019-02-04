using UnityEngine;

[CreateAssetMenu(fileName ="New Weapon", menuName ="VLTest/Weapon")]
public class WeaponDefinition : ScriptableObject
{
	public bool IsAutomatic { get { return isAutomatic; } }
	public float FiringRate { get { return firingRate; } }
	public int ProjectilesPerShot { get { return projectilesPerShot; } }
	public int DamagePerProjectile { get { return damagePerProjectile; } }
	public float Dispersion { get { return dispersion; } }

	[SerializeField] private bool isAutomatic;
	[SerializeField] private float firingRate;
	[SerializeField] private int projectilesPerShot;
	[SerializeField] private int damagePerProjectile;
	[SerializeField] private float dispersion;
}
