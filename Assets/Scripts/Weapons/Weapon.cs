using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float range = 100f;
    [SerializeField] private float maxZoomFOV = 30f;
    [SerializeField] private float minZoomFOV = 10f;
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private float reloadTime = 2f;
    [SerializeField] private float recoilForce = 5f;
    [SerializeField] private int maxAmmo = 30;
    [SerializeField] private float nextFireTime = 0f;

    [SerializeField] private float minAimNoiseAmplitude = 0.1f;
    [SerializeField] private float maxAimNoiseAmplitude = 0.3f;
    [SerializeField] private float minAimNoiseFrequency = 0.5f;
    [SerializeField] private float maxAimNoiseFrequency = 1.5f;
    [SerializeField] private float maxScatterRadius = 1f;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletSpawnPoint;

    private float force = 100f;
    private Vector3 offset = Vector3.zero;
    private Vector3 shootingDirection = Vector3.zero;

    [SerializeField] private float npcInformRadius = 10f;

    public float MaxZoomFOV => maxZoomFOV;
    public float MinZoomFOV => minZoomFOV;
    public float Damage => damage;
    public float Range => range;
    public float FireRate => fireRate;
    public float ReloadTime => reloadTime;
    public float RecoilForce => recoilForce;

    public float MinAimNoiseAmplitude => minAimNoiseAmplitude;
    public float MaxAimNoiseAmplitude => maxAimNoiseAmplitude;
    public float MinAimNoiseFrequency => minAimNoiseFrequency;
    public float MaxAimNoiseFrequency => maxAimNoiseFrequency;
    public float MaxScatterRadius => maxScatterRadius;

    public float NPCInformRadius => npcInformRadius;

    public void SetOffset(Vector3 offset)
    {
        this.offset = offset;
    }

    public void SetDirection(Vector3 direction)
    {
        shootingDirection = direction;
        bulletSpawnPoint.transform.forward = shootingDirection.normalized;
    }

    /// <summary>
    /// Attempts to fire the weapon.
    /// This method checks if enough time has passed since the last shot based on the fire rate.
    /// </summary>
    /// <returns>
    /// Returns true if the weapon was successfully fired, false otherwise.
    /// </returns>
    public bool Fire()
    {
        if (Time.fixedTime >= nextFireTime)
        {
            nextFireTime = Time.fixedTime + fireRate;
            Shoot();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Shoots the weapon, applying damage to any hit targets within range.
    /// This method uses a raycast to detect hits and applies damage to the HitBox component of the target.
    /// </summary>
    private void Shoot()
    {
        RaycastHit hit;
        
        int layerMask = LayerMask.GetMask("Body");
        if (Physics.Raycast(bulletSpawnPoint.transform.position + offset, shootingDirection, out hit, range, layerMask))
        {
            IDamageable hitBox = hit.collider.GetComponent<IDamageable>();

            if (hitBox != null)
            {
                hitBox.TakeDamage(damage, hit.point, bulletSpawnPoint.transform.forward, force);
            }
        }
        Instantiate(bullet, bulletSpawnPoint.transform.position + offset, bulletSpawnPoint.rotation);
        InformNearbyNPCsAboutTheShot();
    }

    void InformNearbyNPCsAboutTheShot()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, npcInformRadius);
        foreach (Collider hit in hitColliders)
        {
            if(hit.TryGetComponent<NPCBase>(out NPCBase npc))
            {
                npc.TriggerPanic();
            }
        }
    }
}
