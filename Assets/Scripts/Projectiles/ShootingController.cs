using UnityEngine;

public class ShootingController : MonoBehaviour
{
    [Header("Projectile Types")]
    public GameObject normalProjectilePrefab;
    public GameObject ricochetProjectilePrefab;
    
    [Header("Shooting Settings")]
    public float fireRate = 0.5f;
    public float spawnOffset = 1f;
    
    private float _lastFireTime = -10f;

    private void Update()
    {
        // Normal fire
        if (Input.GetMouseButtonDown(0) && Time.time >= _lastFireTime + fireRate)
        {
            FireProjectile(normalProjectilePrefab);
        }
        
        // Ricochet fire (right click)
        if (Input.GetMouseButtonDown(1) && Time.time >= _lastFireTime + fireRate)
        {
            FireProjectile(ricochetProjectilePrefab);
        }
    }

    private void FireProjectile(GameObject projectilePrefab)
    {
        Vector3 spawnPosition = transform.position + (transform.forward * spawnOffset) + Vector3.up;
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, transform.rotation);
        _lastFireTime = Time.time;
    }
}