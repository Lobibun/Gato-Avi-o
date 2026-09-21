using UnityEngine;

public class Helicoptero : MonoBehaviour, IDamageable
{
    [Header("Status")]
    public float hp = 10f;
    public GameObject xp;
    private FlashEffect flashEffect;
     [Header("Velocidades")]
    public float velocidadeEntrada = 5f; 
    public float velocidadeCamera = 4f;  
    public float velocidadeVertical = 3f;

     [Header("Controle")]
    public bool chegouNoPosto = false;
    private bool indoParaCima = true;

    [Header("Sistema de Tiro")]
    public GameObject projectilePrefab; 
    public Transform firePoint; 
    public float fireRate = 1.5f; 
    public float visionRange = 15f; 
    public LayerMask playerLayer;
    private float nextFireTime;
    public float velocidadeDoTiro = 7f;
    
    void Start()
    {
        flashEffect = GetComponent<FlashEffect>();
    }

    void Update()
    {
        Mover();
        if (chegouNoPosto) 
        {
            CheckAndFire();
        }
    }

void Mover()
    {
        float moveX = 0f;
        float moveY = 0f;

        if (!chegouNoPosto)
        {
            moveX = -velocidadeEntrada; 
            moveY = 0f; 
        }
        else
        {
            moveX = velocidadeCamera; 

            if (indoParaCima)
                moveY = velocidadeVertical;
            else
                moveY = -velocidadeVertical;
        }

        transform.Translate(new Vector2(moveX, moveY) * Time.deltaTime);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PontoDeParada"))
        {
            chegouNoPosto = true;
            indoParaCima = true; 
        }

        if (other.CompareTag("Teto"))
        {
            indoParaCima = false; 
        }

        if (other.CompareTag("Chao"))
        {
            indoParaCima = true; 
        }
    }

    public void TakeDamage(float amount)
    {
        hp -=amount;

        if (flashEffect != null) 
            flashEffect.Flash();

        if (hp <= 0)
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

        if (xp != null)
        {
            Instantiate(xp, transform.position, Quaternion.identity);
        }
        
        Destroy(gameObject);
    }

    void CheckAndFire()
    {
        if (Time.time < nextFireTime) return;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, visionRange, playerLayer);

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
            bulletScript.SetDirection(Vector2.left);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector2.left * visionRange);
    }
}
