using UnityEngine;

public class Shield : MonoBehaviour
{
   [HideInInspector] public float damage;
   [HideInInspector] public float rotationSpeed;
   public float orbitRadius = 2.5f;
   
   private float currentAngle = 0f; 
   private Transform playerTransform;

    public void Setup(float dmg, float duration, float speed, float radius, float angle)
    {
        this.damage = dmg;
        this.rotationSpeed = speed;
        this.orbitRadius = radius;
        this.currentAngle = angle;

        // O escudo se destrói sozinho após o tempo de duração acabar
        Destroy(gameObject, duration);
    }

    void Start()
    {
        // Encontra o player para girar em volta dele
        if (Player.instance != null)
        {
            playerTransform = Player.instance.transform;
        }
        else
        {
            // Fallback caso não ache o singleton
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) playerTransform = p.transform;
            else Destroy(gameObject);
        }
    }

    void Update()
    {
        if (playerTransform == null) return;

        currentAngle += rotationSpeed * Time.deltaTime;
        if (currentAngle >= 360f) currentAngle -= 360f;

        float rad = currentAngle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * orbitRadius;
        
        transform.position = playerTransform.position + offset;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable damageableObject = other.GetComponent<IDamageable>();
        
         if (damageableObject != null)
    { 
        DamageSystem.ApplyDamage(other.gameObject, damage);
    } 
        else if (other.CompareTag("EnemyProjectile"))
        {
            Destroy(other.gameObject);
        }
    }
}



