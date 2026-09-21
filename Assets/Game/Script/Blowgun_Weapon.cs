using UnityEngine;

public class Blowgun_Weapon : WeaponController
{
    [Header("Configurações do Dardo")]
    public GameObject DartPrefab;

    [Header("Divisão do Dano (Baseado no WeaponData)")]
    [Tooltip("Quanto % do dano do WeaponData vira Impacto? (0.5 = 50%)")]
    public float impactRatio = 0.5f; 

    [Tooltip("Quanto % do dano do WeaponData vira Veneno? (1.0 = 100%)")]
    public float poisonRatio = 1.0f;

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
        float durationMult = 1f;
        
        if (StatusManager.instance != null)
        {
            amountBonus = StatusManager.instance.GetProjectileAmountBonus();
            damageMult += StatusManager.instance.GetDamageBonus(); 
            speedMult += StatusManager.instance.GetProjectileSpeedBonus();
            areaMult += StatusManager.instance.GetAreaBonus();
            durationMult += StatusManager.instance.GetDurationBonus();
        }

        float finalImpactDamage = (currentDamage * impactRatio) * damageMult;
        float finalPoisonDamage = (currentDamage * poisonRatio) * damageMult; 
        float finalPoisonDuration = currentDuration * durationMult;
        float finalSpeed = currentSpeed * speedMult;
        
        int totalBullets = currentCount + amountBonus;

        for (int i = 0; i < totalBullets; i++)
        {
            float offset = (i - (totalBullets - 1) / 2f) * spacing;
            Vector3 spawnPosition = transform.position + (transform.up * offset);
            Quaternion finalRotation = transform.rotation;
            GameObject dartObj = Instantiate(DartPrefab, spawnPosition, finalRotation);
            Dart dartScript = dartObj.GetComponent<Dart>();

            if (dartScript != null)
            {
                dartScript.Setup(
                    transform.right,      
                    finalSpeed,            
                    finalImpactDamage,    
                    finalPoisonDamage,    
                    finalPoisonDuration  
                );
            }
        }
    }
}