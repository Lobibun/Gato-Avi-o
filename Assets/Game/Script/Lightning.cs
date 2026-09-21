using UnityEngine;

public class Lightning : MonoBehaviour
{
    public float damage;
    public float duration = 0.5f;

    void Start()
    {
        Destroy(gameObject, duration); 
    }

    void OnTriggerEnter2D(Collider2D other)
    {
    IDamageable damageableObject = other.GetComponent<IDamageable>();

     if (damageableObject != null)
    { 
     DamageSystem.ApplyDamage(other.gameObject, damage);
    } 

    }
}
