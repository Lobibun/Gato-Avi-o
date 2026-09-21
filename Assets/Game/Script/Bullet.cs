using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float damage;
    
    private Vector2 direction;
    private Transform target; 
    void Update()
    {
        if (target != null)
        {
            direction = (target.position - transform.position).normalized;
            
           
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    public void Setup(Transform newTarget, Vector2 fallbackDirection)
    {
        target = newTarget;
        direction = fallbackDirection.normalized;
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
            DamageSystem.ApplyDamage(other.gameObject, damage);
            Destroy(gameObject);
        } 
    }
}