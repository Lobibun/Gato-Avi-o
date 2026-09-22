using UnityEngine;

public class RocketLauncher : WeaponController
{
    [Header("Configuração do Rocket")]
    public GameObject rocketPrefab;
    public LayerMask enemyLayer;
    public float baseExplosionRadius = 2f;
    
    [Header("Formação de Disparo")]
    [Tooltip("Distância entre os mísseis")]
    public float spacing = 0.2f; 

    protected override void Attack()
    {
        base.Attack();
        
        float amountBonus = (StatusManager.instance != null) ? StatusManager.instance.GetProjectileAmountBonus() : 0;
        int rocketCount = currentCount + (int)amountBonus;
        
        for (int i = 0; i < rocketCount; i++)
        {
            // Lógica de Parede Compacta
            float offset = (i - (rocketCount - 1) / 2f) * spacing;
            
            // Posição de saída calculada
            Vector3 spawnPosition = transform.position + (transform.up * offset);

            // Rotação: Mantém a rotação da arma (todos olham pra frente)
            Quaternion finalRotation = transform.rotation;

            GameObject rocketobj = Instantiate(rocketPrefab, spawnPosition, finalRotation);
            Rocket rocketScript = rocketobj.GetComponent<Rocket>();

            if (rocketScript != null)
            {
                // --- APLICANDO STATUS ---
                float dmgMult = 1f;
                float spdMult = 1f;
                float areaMult = 1f;

                if (StatusManager.instance != null)
                {
                    dmgMult += StatusManager.instance.GetDamageBonus();
                    spdMult += StatusManager.instance.GetProjectileSpeedBonus();
                    areaMult += StatusManager.instance.GetAreaBonus();
                }

                rocketScript.damage = currentDamage * dmgMult;
                rocketScript.speed = currentSpeed * spdMult;
                rocketScript.explosionRadius = baseExplosionRadius * areaMult;

                // Aplica o tamanho (Respeitando o currentArea do pai)
                rocketobj.transform.localScale = rocketPrefab.transform.localScale * currentArea * areaMult;
                
                rocketScript.enemyLayer = enemyLayer;
                rocketScript.SetDirection(transform.right);
            }
        }
    }
}