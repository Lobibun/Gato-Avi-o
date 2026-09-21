using UnityEngine;
using System.Collections;

public class Dog : MonoBehaviour, IDamageable
{
    [Header("Status")]
    public float Hp;
    public float Speed;      
    public Vector2 Direction; 
    public GameObject Xp;     
    private FlashEffect flashEffect; 

    [Header("Sistema de Tiro")]
    public GameObject projectilePrefab; 
    public Transform firePoint; 
    public float fireRate = 1.5f; 
    public float visionRange = 10f; 
    public LayerMask playerLayer;
    public float velocidadeDoTiro = 12f;

    private float nextFireTime;

    void Start()
    {
        flashEffect = GetComponent<FlashEffect>();
    }

     void Update()
    {
         transform.Translate(Direction * Speed * Time.deltaTime);
         CheckAndFire();
    }

     public void SetDirection(Vector2 dir)
    {
        Direction = dir.normalized; 
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
           Destroy(gameObject); 
        } 
    }

     public void TakeDamage(float damagefb)
    {
      Hp -= damagefb;
      
      if(flashEffect != null) 
          flashEffect.Flash();
      
      if (Hp <= 0)
      {
        death();
      }
    }

    public void death()
    {
        if (GameControler.instance != null)
        {
            GameControler.instance.AddKill();
        }

        if (Xp != null) 
        {
            Instantiate(Xp, transform.position, Quaternion.identity);
        }
        
        Destroy(gameObject);
    }

    void CheckAndFire()
    {
        if (Time.time < nextFireTime) return;

    
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Direction, visionRange, playerLayer);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player"))
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

   void Shoot()
    {
        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;
        
        GameObject bulletObj = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
        EnemyProjectile bulletScript = bulletObj.GetComponent<EnemyProjectile>();
        
        if (bulletScript != null)
        {
            bulletScript.speed = velocidadeDoTiro;
            bulletScript.SetDirection(Direction);
        }
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
       Gizmos.DrawRay(transform.position, Direction * visionRange);
    }
}