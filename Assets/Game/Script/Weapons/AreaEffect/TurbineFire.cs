using UnityEngine;

public class TurbineFire : AreaEffect
{
    private float contactDamage;
    private float burnDuration;
    private float vulnerabilityMultiplier;

    public void Setup(float dmg, float duration, float multiplier)
    {
        contactDamage = dmg;
        burnDuration = duration;
        vulnerabilityMultiplier = multiplier;
    }

    protected override void ApplyAreaDamage()
    {
        for (int i = enemiesInside.Count - 1; i >= 0; i--)
        {
            IDamageable enemy = enemiesInside[i];

            if (enemy == null || !((MonoBehaviour)enemy).gameObject.activeSelf)
            {
                enemiesInside.RemoveAt(i);
                continue;
            }

            GameObject enemyObj = ((MonoBehaviour)enemy).gameObject;
            DamageSystem.ApplyDamage(enemyObj, contactDamage);
            ApplyBurnToEnemy(enemyObj);
        }
    }

    protected override void OnEnemyEnter(GameObject enemyObj)
    {
        ApplyBurnToEnemy(enemyObj);
    }

    void ApplyBurnToEnemy(GameObject enemy)
    {
        BurnEffect existingEffect = enemy.GetComponent<BurnEffect>();

        if (existingEffect != null)
        {
            existingEffect.RefreshDuration(burnDuration, vulnerabilityMultiplier);
        }
        else
        {
            BurnEffect newEffect = enemy.AddComponent<BurnEffect>();
            newEffect.Activate(burnDuration, vulnerabilityMultiplier);
        }
    }
}