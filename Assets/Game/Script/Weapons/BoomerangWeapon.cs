using UnityEngine;
using System.Collections.Generic;

public class BoomerangWeapon : WeaponController
{
    [Header("Configurações Específicas")]
    public GameObject boomerangPrefab;
    
    private List<GameObject> activeBoomerangs = new List<GameObject>();

    protected override void Update()
    {
        activeBoomerangs.RemoveAll(b => b == null);

        if (activeBoomerangs.Count > 0)
        {
            currentCooldown = 0f;
            return;
        }
        base.Update();
    }

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
            areaMult += StatusManager.instance.GetAreaBonus();
            speedMult += StatusManager.instance.GetProjectileSpeedBonus();
        }

        float finalDamage = currentDamage * damageMult;
        float finalSpeed = currentSpeed * speedMult;
        
        int totalBoomerangs = currentCount + amountBonus;

        for (int i = 0; i < totalBoomerangs; i++)
        {
            GameObject boomObj = Instantiate(boomerangPrefab, transform.position, Quaternion.identity);
            boomObj.transform.localScale = boomerangPrefab.transform.localScale * (currentArea * areaMult);

            Boomerang script = boomObj.GetComponent<Boomerang>();
            if (script != null)
            {
                Vector2 targetDir = GetRandomEnemyDirection();
                script.Setup(targetDir, finalDamage, finalSpeed, transform);
            }

            activeBoomerangs.Add(boomObj);
        }
    }

    private Vector2 GetRandomEnemyDirection()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length > 0)
        {
            GameObject randomEnemy = enemies[Random.Range(0, enemies.Length)];
            return (randomEnemy.transform.position - transform.position).normalized;
        }
        return Random.insideUnitCircle.normalized;
    }
}
