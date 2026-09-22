using UnityEngine;

public class Rocket : Projectile
{
    [Header("Configuração")]
    public float damage;
    public float explosionRadius;

    [Header("Ajuste Visual")]
    public float spriteScaleAdjustment;

    [Header("Referências")]
    public LayerMask enemyLayer;
    public GameObject explosionEffect;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
            return;
        }

        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            Explode();
        }
    }

    void Explode()
    {
        if (explosionEffect != null)
        {
            GameObject visual = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            SpriteRenderer sr = visual.GetComponent<SpriteRenderer>();

            if (sr != null)
            {
                float spriteWorldSize = sr.bounds.size.x;
                float targetSize = explosionRadius * 2f;
                float scaleFactor = targetSize / spriteWorldSize;
                visual.transform.localScale *= scaleFactor;
            }
        }

        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayer);

        foreach (Collider2D enemyCollider in enemiesHit)
        {
            DamageSystem.ApplyDamage(enemyCollider.gameObject, damage);
        }

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.4f);
        Gizmos.DrawSphere(transform.position, explosionRadius);
    }
}