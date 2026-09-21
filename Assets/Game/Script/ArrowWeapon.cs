using UnityEngine;

public class ArrowWeapon : WeaponController
{
    [Header("Configurações da Arma")]
    public GameObject arrowPrefab;

    protected override void Attack()
    {
        base.Attack(); 

        float damageMult = 1f;
        float areaMult = 1f;
        float speedMult = 1f;
        float durationMult = 1f;

        if (StatusManager.instance != null)
        {
            damageMult += StatusManager.instance.GetDamageBonus();
            areaMult += StatusManager.instance.GetAreaBonus();
            speedMult += StatusManager.instance.GetProjectileSpeedBonus();
            durationMult += StatusManager.instance.GetDurationBonus();
        }

        float finalDamage = currentDamage * damageMult;
        float finalSpeed = currentSpeed * speedMult;
        float finalLifeTime = currentDuration * durationMult;

        Debug.Log($"[TESTE DE STATUS] A Flecha vai durar: {finalLifeTime} segundos! (Tempo Base do Nível: {currentDuration} * Multiplicador do Jogador: {durationMult})");

        if (arrowPrefab != null)
        {
            GameObject arrowObj = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
            
            arrowObj.transform.localScale = arrowPrefab.transform.localScale * (currentArea * areaMult);

            TrailRenderer trail = arrowObj.GetComponent<TrailRenderer>();
            if (trail != null)
            {
                trail.time *= durationMult;
            }

            Arrow script = arrowObj.GetComponent<Arrow>();
            if (script != null)
            {
                Vector2 targetDir = Vector2.right; 
                
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                if (enemies.Length > 0)
                {
                    GameObject randomEnemy = enemies[Random.Range(0, enemies.Length)];
                    targetDir = (randomEnemy.transform.position - transform.position).normalized;
                }
                
                script.Setup(targetDir, finalDamage, finalSpeed, finalLifeTime);
            }
        }
    }
}