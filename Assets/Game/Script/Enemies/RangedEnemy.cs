using UnityEngine;

public abstract class RangedEnemy : Enemy
{
    [Header("Sistema de Tiro")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 1.5f;
    public float visionRange = 10f;
    public LayerMask playerLayer;
    public float velocidadeDoTiro = 12f;

    protected float nextFireTime;

    protected void CheckAndFire()
    {
        if (Time.time < nextFireTime) return;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, visionRange, playerLayer);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    protected virtual void Shoot()
    {
        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;

        GameObject bulletObj = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
        EnemyProjectile bulletScript = bulletObj.GetComponent<EnemyProjectile>();

        if (bulletScript != null)
        {
            bulletScript.speed = velocidadeDoTiro;
            bulletScript.SetDirection(direction);
        }
    }
}
