using UnityEngine;

public class AutomaticWeapon : WeaponController
{
    public GameObject bulletPrefab;
    
    [Header("Configuração de Disparo (Parede)")]
    [Tooltip("Distância física entre uma bala e outra (em metros)")]
    public float spacing = 0.5f; 

    protected override void Attack()
    {
        base.Attack(); 

        int amountBonus = 0;
        float damageMult = 1f;
        float speedMult = 1f;
        float areaMult = 1f;
        
        if (StatusManager.instance != null)
        {
            amountBonus = StatusManager.instance.GetProjectileAmountBonus();
            damageMult += StatusManager.instance.GetDamageBonus(); 
            speedMult += StatusManager.instance.GetProjectileSpeedBonus();
            areaMult += StatusManager.instance.GetAreaBonus();
        }

        int totalBullets = currentCount + amountBonus;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        
        Transform closestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject enemy in enemies)
        {
            Vector3 directionToEnemy = enemy.transform.position - currentPosition;
            float dSqrToTarget = directionToEnemy.sqrMagnitude;
            
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                closestTarget = enemy.transform;
            }
        }

        for (int i = 0; i < totalBullets; i++)
        {
            float offset = (i - (totalBullets - 1) / 2f) * spacing;
            Vector3 spawnPosition = transform.position + (transform.up * offset);
            Quaternion finalRotation = transform.rotation;
            
            GameObject bullet = Instantiate(bulletPrefab, spawnPosition, finalRotation);
            Bullet bulletScript = bullet.GetComponent<Bullet>();

            if (bulletScript != null)
            {
                bulletScript.damage = currentDamage * damageMult;
                bulletScript.speed = currentSpeed * speedMult;
                bullet.transform.localScale = bulletPrefab.transform.localScale * currentArea * areaMult;
                bulletScript.Setup(closestTarget, transform.right);
            }
        }
    }
}