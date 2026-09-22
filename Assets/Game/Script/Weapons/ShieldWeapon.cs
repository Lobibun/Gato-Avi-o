using UnityEngine;

public class ShieldWeapon : WeaponController
{
    [Header("Configurações Específicas")]
    public GameObject shieldPrefab;
    public float baseRadius = 2.5f;
    private GameObject currentActiveShield;

    protected override void Update()
    {
        if (currentActiveShield != null)
        {
            return; 
        }
        base.Update();
    }

    protected override void Attack()
    {
        base.Attack();
        
        float damageMult = 1f;
        float speedMult = 1f;
        float areaMult = 1f;
        float durationMult = 1f;

        if (StatusManager.instance != null)
        {
            damageMult += StatusManager.instance.GetDamageBonus(); 
            areaMult += StatusManager.instance.GetAreaBonus();
            speedMult += StatusManager.instance.GetProjectileSpeedBonus();
            durationMult += StatusManager.instance.GetDurationBonus();
        }

        int totalShields = 2; 
        float finalDuration = currentDuration * durationMult;
        float finalSpeed = currentSpeed * speedMult;
        float finalDamage = currentDamage * damageMult;

        float angleStep = 360f / totalShields;

        for (int i = 0; i < totalShields; i++)
        {
            GameObject shieldObj = Instantiate(shieldPrefab, transform.position, Quaternion.identity);
            currentActiveShield = shieldObj;
            
            // Aplica Tamanho
            shieldObj.transform.localScale = shieldPrefab.transform.localScale * (currentArea * areaMult);

            // Configura o Script
            Shield script = shieldObj.GetComponent<Shield>();
            if (script != null)
            {
                float currentAngle = i * angleStep;
                script.Setup(finalDamage, finalDuration, finalSpeed, baseRadius, currentAngle);
            }
        }
    }
}