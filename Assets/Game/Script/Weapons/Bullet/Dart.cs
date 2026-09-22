using UnityEngine;

public class Dart : Projectile
{
    private float impactDamage;
    private float poisonDamage;
    private float poisonDuration;

    public void Setup(Vector2 dir, float spd, float iDmg, float pDmg, float pDur)
    {
        speed = spd;
        impactDamage = iDmg;
        poisonDamage = pDmg;
        poisonDuration = pDur;

        SetDirection(dir);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }

        IDamageable damageableObject = other.GetComponent<IDamageable>();
        if (damageableObject != null)
        {
            OnHitEnemy(other.gameObject);
        }
    }

    protected override void OnHitEnemy(GameObject enemyObj)
    {
        IDamageable damageableInfo = enemyObj.GetComponent<IDamageable>();

        DamageSystem.ApplyDamage(enemyObj, impactDamage);
        ApplyPoisonLoginc(enemyObj, damageableInfo);
        Destroy(gameObject);
    }

    void ApplyPoisonLoginc(GameObject enemy, IDamageable damageableInfo)
    {
        PoisonEffect existingEffect = enemy.GetComponent<PoisonEffect>();

        if (existingEffect != null)
        {
            existingEffect.RefreshDuration(poisonDuration, poisonDamage);
        }
        else
        {
            PoisonEffect newEffect = enemy.AddComponent<PoisonEffect>();
            newEffect.Activate(poisonDamage, poisonDuration, damageableInfo);
        }
    }
}