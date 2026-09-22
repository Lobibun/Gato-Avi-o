using UnityEngine;

public class ForceFieldWeapon : WeaponController
{
    [Header("Configurações do Campo")]
    public GameObject fieldPrefab;
    
    private GameObject currentActiveField; 

    protected override void Update()
    {
        if (currentActiveField != null) 
        {
            return; 
        }

        base.Update();
    }

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
        float finalMaxScale = currentArea * areaMult; 
        float finalGrowthSpeed = currentSpeed * speedMult; 
        
        float finalDuration = currentDuration * durationMult;

        currentActiveField = Instantiate(fieldPrefab, transform.position, Quaternion.identity, transform);
        
        PulsingAura script = currentActiveField.GetComponent<PulsingAura>();
        
        if (script != null)
        {
            script.Setup(finalDamage, finalMaxScale, finalGrowthSpeed, finalDuration);
        }
    }
}