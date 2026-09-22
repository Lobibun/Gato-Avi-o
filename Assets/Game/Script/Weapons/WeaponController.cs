using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Dados da Arma")]
    public WeaponData weaponData;
    
    protected float currentCooldown;
    protected float currentDamage;
    protected int currentCount;
    protected float currentSpeed;
    protected float currentDuration; 
    
    protected float currentArea = 1f; 

    public int currentLevel = -1; 

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
        currentCooldown -= Time.deltaTime;
        if (currentCooldown < 0f)
        {
            Attack();
        }
    }

    protected virtual void Attack()
    {
        if (currentLevel < 0 || currentLevel >= weaponData.levels.Count) return;

        float baseCd = weaponData.levels[currentLevel].cooldown;
        
        float reduction = (StatusManager.instance != null) ? StatusManager.instance.GetCooldownReductionBonus() : 0f;
        
        currentCooldown = baseCd * (1f - reduction);
    }

    public void LevelUp()
    {
        if (currentLevel < weaponData.levels.Count - 1)
        {
            currentLevel++;
            ApplyStats();
        }
    }

    void ApplyStats()
    {
        var stats = weaponData.levels[currentLevel];
        
        currentDamage = stats.damage;
        currentCount = stats.projectileCount;
        currentSpeed = stats.speed;
        currentDuration = stats.duration; 

        if (stats.area <= 0) 
        {
            currentArea = 1f; 
        }
        else 
        {
            currentArea = stats.area;
        }
        
        Debug.Log($"Arma {weaponData.weaponName} subiu para o Nível {currentLevel + 1}");
    }
}