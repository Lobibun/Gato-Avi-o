using UnityEngine;

public class Bullet : Projectile
{
    public float damage;
    private Transform target;

    public void Setup(Transform newTarget, Vector2 fallbackDirection)
    {
        target = newTarget;
        SetDirection(fallbackDirection);
    }

    protected override void Update()
    {
        if (target != null)
        {
            SetDirection(target.position - transform.position);
        }

        base.Update();
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
        DamageSystem.ApplyDamage(enemyObj, damage);
        Destroy(gameObject);
    }
}