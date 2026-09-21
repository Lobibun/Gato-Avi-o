using UnityEngine;
using System.Collections.Generic;

public class Arrow : MonoBehaviour
{
    private float damage;
    private float speed;
    private float lifeTime;
    private Vector2 direction;

    private float tempoTotalQueDurou;

    private List<IDamageable> hitEnemies = new List<IDamageable>();

    private Camera mainCamera;
    private Vector2 spriteSize;
    private float minX, maxX, minY, maxY;

    public void Setup(Vector2 dir, float dmg, float spd, float life)
    {
        this.direction = dir.normalized;
        this.damage = dmg;
        this.speed = spd;
        this.lifeTime = life;
        this.tempoTotalQueDurou = life;
        
        UpdateRotation();
    }

    void Start()
    {
        mainCamera = Camera.main;
        
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) spriteSize = sr.bounds.extents;
    }

    void Update()
    {
        // Move a seta
        transform.position += (Vector3)direction * speed * Time.deltaTime;
        
        // Verifica se saiu da tela e faz o ricochete
        UpdateBoundsAndBounce();

        // Destrói após o tempo de vida acabar
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Debug.Log($"[FIM DA SETA] A flecha durou exatamente {tempoTotalQueDurou} segundos e foi destruída!");
            Destroy(gameObject);
        }
    }

    void UpdateBoundsAndBounce()
    {
        if (mainCamera == null) return;

        float camDistance = Mathf.Abs(mainCamera.transform.position.z - transform.position.z);
        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, camDistance));
        Vector3 topRight   = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, camDistance));

        minX = bottomLeft.x + spriteSize.x;
        maxX = topRight.x - spriteSize.x;
        minY = bottomLeft.y + spriteSize.y;
        maxY = topRight.y - spriteSize.y;

        Vector3 currentPos = transform.position;
        bool didBounce = false;

        // Bateu nas laterais (Esquerda/Direita)
        if (currentPos.x <= minX)
        {
            currentPos.x = minX;
            direction.x *= -1;
            didBounce = true;
        }
        else if (currentPos.x >= maxX)
        {
            currentPos.x = maxX;
            direction.x *= -1;
            didBounce = true;
        }

        // Bateu em cima/embaixo
        if (currentPos.y <= minY)
        {
            currentPos.y = minY;
            direction.y *= -1;
            didBounce = true;
        }
        else if (currentPos.y >= maxY)
        {
            currentPos.y = maxY;
            direction.y *= -1;
            didBounce = true;
        }

        // Se quicou, aplica a posição, gira e "esquece" os inimigos que já bateu
        if (didBounce)
        {
            transform.position = currentPos;
            UpdateRotation();
            hitEnemies.Clear(); 
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            IDamageable dmgObj = other.GetComponent<IDamageable>();
            
            // Se tem vida e ainda NÃO tomou dano nesta passada da seta
            if (dmgObj != null && !hitEnemies.Contains(dmgObj))
            {
                DamageSystem.ApplyDamage(other.gameObject, damage);
                
                // Lembra do inimigo para não dar dano repetido no mesmo frame
                hitEnemies.Add(dmgObj);
            }
        }
    }

    void UpdateRotation()
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }
}
