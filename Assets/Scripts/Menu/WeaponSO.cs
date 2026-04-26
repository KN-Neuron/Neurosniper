using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapons/Weapon")]
public class WeaponSO : ScriptableObject
{
    public string weaponName;
    public Sprite icon;
    public int damage;
    public float fireRate;
    public float range;
    public float meditivness;
    public float attentivness;
}