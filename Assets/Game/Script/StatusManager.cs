using UnityEngine;

public class StatusManager : MonoBehaviour
{
    public static StatusManager instance;
    
    [Header("Upgrade levels (0-5)")]
    public int damageLevel = 0;
    public int projectileAmountLevel = 0;
    public int projectileSpeedLevel = 0;
    public int areaLevel = 0;
    public int movementSpeedLevel = 0;
    public int healthLevel = 0;
    public int cooldownReductionLevel = 0;
    public int armorLevel = 0;
    public int durationLevel = 0;
    public int recoveryLevel = 0;
    public int magnetLevel = 0;
    public int growthLevel = 0;
    public int greedLevel = 0;
    public int luckLevel = 0;

    void Awake() 
    { 
        if (instance == null) instance = this;
        else Destroy(gameObject); 
    }

    public void ApplyUpgradeEffect(UpgradeType type)
    {
        switch (type)
        {
            case UpgradeType.DamageIncrease:
                damageLevel++;
                break;

            case UpgradeType.ProjectileQuantity:
                projectileAmountLevel++;
                break;

            case UpgradeType.ProjectileSpeed:
                projectileSpeedLevel++;
                break;

            case UpgradeType.ProjectileSize:
                areaLevel++;
                break;

            case UpgradeType.MovingSpeed:
                movementSpeedLevel++;
                break;

            case UpgradeType.HealthIncrease:
                healthLevel++;
                if (Player.instance != null)
                {
                    Player.instance.maxHp += 2f;
                    Player.instance.currentHp += 2f;
                    
                    if(GameControler.instance != null)
                    {
                        GameControler.instance.UpdateHpText(Player.instance.currentHp);
                    }
                }
                break;

            case UpgradeType.CoodownReduction:
                cooldownReductionLevel++;
                break;

            case UpgradeType.Armor:
                armorLevel++;
                break;

            case UpgradeType.Duration: 
                durationLevel++;
                 break;

            case UpgradeType.Recovery: 
                recoveryLevel++;
                break;

            case UpgradeType.Magnetism: 
                magnetLevel++;
                break;

            case UpgradeType.Growth: 
                growthLevel++;
                break;

            case UpgradeType.Greed: 
                greedLevel++;
                break;

            case UpgradeType.Luck: // NOVO
                luckLevel++;
                break;    
        }
    }

    public float GetDamageBonus() => damageLevel * 0.2f;
    public int GetProjectileAmountBonus() => projectileAmountLevel;
    public float GetProjectileSpeedBonus() => projectileSpeedLevel * 0.2f;
    public float GetAreaBonus() => areaLevel * 0.2f;
    public float GetMovementSpeedBonus() => movementSpeedLevel *0.5f;
    public float GetHealthBonus() => healthLevel * 0.5f;
    public float GetCooldownReductionBonus() => cooldownReductionLevel * 0.1f;
    public float GetArmorBonus() => armorLevel * 1.0f;
    public float GetDurationBonus() => durationLevel * 0.3f;
    public float GetRecoveryBonus() => recoveryLevel * 0.5f;
    public float GetMagnetBonus() => magnetLevel * 1.5f;            
    public float GetGrowthBonus() => growthLevel * 0.2f;              
    public float GetGreedBonus() => greedLevel * 1f;             
    public float GetLuckBonus() => luckLevel * 1.0f;
}