using UnityEngine;
using System.Collections.Generic;

public class Boomerang : MonoBehaviour
{
    private float damage;
    private float speed;
    private Transform player;
    private Vector2 direction;

    private bool isReturning = false;
    
    [Header("Visual")]
    public float rotationSpeed = 720f; 

    private List<IDamageable> hitEnemies = new List<IDamageable>();

    // Variáveis para calcular a borda da tela (A Parede)
    private Camera mainCamera;
    private Vector2 spriteSize;
    private float minX, maxX, minY, maxY;

    public void Setup(Vector2 dir, float dmg, float spd, Transform playerTransform)
    {
        this.direction = dir.normalized;
        this.damage = dmg;
        this.speed = spd;
        this.player = playerTransform; 
    }

    void Start()
    {
        mainCamera = Camera.main;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) spriteSize = sr.bounds.extents;
    }

    void Update()
    {
        // Gira o visual
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        if (!isReturning)
        {
            // FASE 1: INDO ATÉ A PAREDE (BORDA DA TELA)
            transform.position += (Vector3)direction * speed * Time.deltaTime;
            
            // Se bateu na borda da tela, volta!
            if (CheckOutOfBounds())
            {
                ReturnToPlayer();
            }
        }
        else
        {
            // FASE 2: VOLTANDO (Efeito Ímã)
            if (player != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

                if (Vector3.Distance(transform.position, player.position) < 0.5f)
                {
                    Destroy(gameObject); // Pegou o bumerangue de volta
                }
            }
            else
            {
                Destroy(gameObject); 
            }
        }
    }

    bool CheckOutOfBounds()
    {
        if (mainCamera == null) return false;

        // Calcula exatamente onde termina a visão da câmera
        float camDistance = Mathf.Abs(mainCamera.transform.position.z - transform.position.z);
        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, camDistance));
        Vector3 topRight   = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, camDistance));

        minX = bottomLeft.x + spriteSize.x;
        maxX = topRight.x - spriteSize.x;
        minY = bottomLeft.y + spriteSize.y;
        maxY = topRight.y - spriteSize.y;

        Vector3 currentPos = transform.position;

        // Se a posição dele passou de qualquer limite da tela, retorna TRUE (Bateu na parede)
        if (currentPos.x <= minX || currentPos.x >= maxX || currentPos.y <= minY || currentPos.y >= maxY)
        {
            return true;
        }

        return false;
    }

    void ReturnToPlayer()
    {
        isReturning = true;
        hitEnemies.Clear(); // Limpa a lista para dar dano de novo na volta!
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            IDamageable dmgObj = other.GetComponent<IDamageable>();
            
            if (dmgObj != null && !hitEnemies.Contains(dmgObj))
            {
                DamageSystem.ApplyDamage(other.gameObject, damage);
                hitEnemies.Add(dmgObj);
            }
        }
    }
}
