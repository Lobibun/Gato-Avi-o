using UnityEngine;
using System.Collections.Generic;

public class PulsingAura : MonoBehaviour
{
    private float damage;
    private float maxScale;     
    private float growthSpeed;  
    private float holdDuration;

    private bool isGrowing = true;
    private float currentHoldTime = 0f;
    private float damageInterval = 0.2f; // Dano muito rápido (5x por segundo)
    private float damageTimer = 0f;

    private List<IDamageable> enemiesInside = new List<IDamageable>();

    public void Setup(float dmg, float targetScale, float speed, float duration)
    {
        this.damage = dmg;
        this.maxScale = targetScale;
        this.growthSpeed = speed;
        this.holdDuration = duration;

        transform.localScale = Vector3.zero; 
    }

    void Update()
    {
        if (isGrowing)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one * maxScale, growthSpeed * Time.deltaTime);

            if (transform.localScale.x >= maxScale)
            {
                transform.localScale = Vector3.one * maxScale;
                isGrowing = false; 
            }
        }
        else
        {
            currentHoldTime += Time.deltaTime;
            if (currentHoldTime >= holdDuration)
            {
                Destroy(gameObject); 
            }
        }
        ProcessDamage();
    }

    void ProcessDamage()
    {
        damageTimer -= Time.deltaTime;
        
        if (damageTimer <= 0)
        {
            for (int i = enemiesInside.Count - 1; i >= 0; i--)
            {
                IDamageable enemy = enemiesInside[i];

                if (enemy != null && ((MonoBehaviour)enemy).gameObject.activeSelf)
                {
                    GameObject enemyObj = ((MonoBehaviour)enemy).gameObject;
                    DamageSystem.ApplyDamage(enemyObj, damage);
                }
                else
                {
                    enemiesInside.RemoveAt(i); 
                }
            }
            
            damageTimer = damageInterval; 
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            IDamageable dmg = other.GetComponent<IDamageable>();
            if (dmg != null && !enemiesInside.Contains(dmg))
            {
                enemiesInside.Add(dmg);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
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
