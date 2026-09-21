using UnityEngine;
using System.Collections;

public class Dart : MonoBehaviour
{
    public float speed;
    private Vector2 direction;
    private Rigidbody2D rb;
    private Collider2D col;
    private float impactDamage;
    private float poisonDamage;
    private float poisonDuration;

    public void Setup(Vector2 dir, float spd, float iDmg, float pDmg, float pDur)
    {
        this.direction = dir.normalized;
        this.speed = spd;
        this.impactDamage = iDmg;
        this.poisonDamage = pDmg;
        this.poisonDuration = pDur;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    
    void Update()
    {
         transform.Translate(direction * speed * Time.deltaTime);
    }

     public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
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
            DamageSystem.ApplyDamage(other.gameObject, impactDamage);
            ApplyPoisonLoginc(other.gameObject, damageableObject);
            Destroy(gameObject);
        } 
    }

    void ApplyPoisonLoginc(GameObject enemy, IDamageable damageableInfo)
    {
        PoisonEffect existingEffect = enemy.GetComponent<PoisonEffect>();

        if (existingEffect != null)
        {
            // caso O inimigo JÁ está envenenado.
            existingEffect.RefreshDuration(poisonDuration, poisonDamage);
        }
        else
        {
            // Caso o inimigo NÃO está envenenado.
            PoisonEffect newEffect = enemy.AddComponent<PoisonEffect>();
            newEffect.Activate(poisonDamage, poisonDuration, damageableInfo);
        }
    }
}
