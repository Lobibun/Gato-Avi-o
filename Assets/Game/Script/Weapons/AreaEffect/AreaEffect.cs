using UnityEngine;
using System.Collections.Generic;

public abstract class AreaEffect : MonoBehaviour
{
    protected List<IDamageable> enemiesInside = new List<IDamageable>();
    protected float damageTimer;
    protected float tickRate = 0.2f;

    protected virtual void Update()
    {
        damageTimer -= Time.deltaTime;
        if (damageTimer <= 0)
        {
            ApplyAreaDamage();
            damageTimer = tickRate;
        }
    }

    protected abstract void ApplyAreaDamage();

    protected virtual void OnEnemyEnter(GameObject enemyObj) { }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            IDamageable dmg = other.GetComponent<IDamageable>();
            if (dmg != null && !enemiesInside.Contains(dmg))
            {
                enemiesInside.Add(dmg);
                OnEnemyEnter(other.gameObject);
            }
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            IDamageable dmg = other.GetComponent<IDamageable>();
            if (dmg != null && enemiesInside.Contains(dmg))
            {
                enemiesInside.Remove(dmg);
            }
        }
    }
}