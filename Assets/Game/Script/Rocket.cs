using UnityEngine;

public class Rocket : MonoBehaviour
{
    [Header("Configuração")]
    public float speed;
    public float damage;
    public float explosionRadius;

    [Header("Ajuste Visual")]
    public float spriteScaleAdjustment;

    [Header("Refencias")]
    public LayerMask enemyLayer;
    public GameObject explosionEffect; 
    private Vector3 direction;

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 1. Se bater na parede, só destrói
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