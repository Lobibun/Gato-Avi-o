using UnityEngine;
using System.Collections;

public class PoisonEffect : MonoBehaviour
{
    private float damagePerTick;
    private float duration;
    private float tickInterval = 0.5f; 
    
    private float timeRemaining;
    private IDamageable targetHealth;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    public void Activate(float dmg, float time, IDamageable target)
    {
        this.damagePerTick = dmg;
        this.duration = time;
        this.timeRemaining = time;
        this.targetHealth = target;

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
            spriteRenderer.color = Color.green; 
        }

        StartCoroutine(PoisonRoutine());
    }

    public void RefreshDuration(float newDuration, float newDamage)
    {
        // Reseta o tempo para o máximo
        timeRemaining = newDuration;
        
        if (newDamage > damagePerTick)
        {
            damagePerTick = newDamage;
        }
    }

    IEnumerator PoisonRoutine()
    {
        while (timeRemaining > 0)
        {
            DamageSystem.ApplyDamage(gameObject, damagePerTick, DamageType.Poison);

            yield return new WaitForSeconds(tickInterval);
            timeRemaining -= tickInterval;
        }

        if (spriteRenderer != null) spriteRenderer.color = originalColor;
        Destroy(this);
    }
}
