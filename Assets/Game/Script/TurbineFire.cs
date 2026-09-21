using UnityEngine;
using System.Collections.Generic;

public class TurbineFire : MonoBehaviour
{
    private float contactDamage;
    private float burnDuration;
    private float vulnerabilityMultiplier; // Ex: 1.5
    
    private float damageTimer;
    private float tickRate = 0.2f;

    private List<IDamageable> enemiesInRange = new List<IDamageable>();
    private List<GameObject> enemyObjectsInRange = new List<GameObject>();

    public void Setup(float dmg, float duration, float multiplier)
    {
        this.contactDamage = dmg;
        this.burnDuration = duration;
        this.vulnerabilityMultiplier = multiplier;
    }

    void Update()
    {
        damageTimer -= Time.deltaTime;
        
        if (damageTimer <= 0)
        {
            ApplyFireLogic();
            damageTimer = tickRate;
        }
    }

    void ApplyFireLogic()
    {
       
        for (int i = enemiesInRange.Count - 1; i >= 0; i--)
        {
            IDamageable enemy = enemiesInRange[i];

            if (enemy == null || !((MonoBehaviour)enemy).gameObject.activeSelf)
            {
                enemiesInRange.RemoveAt(i);
                continue;
            }

            GameObject enemyObj = ((MonoBehaviour)enemy).gameObject;

            DamageSystem.ApplyDamage(enemyObj, contactDamage);
            ApplyBurnToEnemy(enemyObj);
        }
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            IDamageable dmg = other.GetComponent<IDamageable>();
            if (dmg != null && !enemiesInRange.Contains(dmg))
            {
                enemiesInRange.Add(dmg);
                enemyObjectsInRange.Add(other.gameObject);
                
                // Aplica a queimadura imediatamente ao entrar
                ApplyBurnToEnemy(other.gameObject);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            IDamageable dmg = other.GetComponent<IDamageable>();
            if (dmg != null && enemiesInRange.Contains(dmg))
            {
                int index = enemiesInRange.IndexOf(dmg);
                enemiesInRange.RemoveAt(index);
                enemyObjectsInRange.RemoveAt(index);
            }
        }
    }
}
