using UnityEngine;

public class WeaponModel : MonoBehaviour
{
	public Transform Muzzle { get { return muzzle; } }

	[SerializeField] private Transform muzzle;
}
