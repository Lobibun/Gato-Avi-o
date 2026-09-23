using UnityEngine;
using System.Collections.Generic;

public class Boomerang : Projectile
{
    private float damage;
    private Transform player;
    private bool isReturning = false;

    [Header("Visual")]
    public float rotationSpeed = 720f;

    private List<IDamageable> hitEnemies = new List<IDamageable>();


    private Vector2 spriteSize;
    private float minX, maxX, minY, maxY;

    public void Setup(Vector2 dir, float dmg, float spd, Transform playerTransform)
    {
        damage = dmg;
        speed = spd;
        player = playerTransform;

        SetDirection(dir);
    }

    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) spriteSize = sr.bounds.extents;
    }

    protected override void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        if (!isReturning)
        {
            base.Update();

            if (CheckOutOfBounds())
            {
                ReturnToPlayer();
            }
        }
        else
        {
            if (player != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

                if (Vector3.Distance(transform.position, player.position) < 0.5f)
                {
                    Destroy(gameObject);
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
        if (CameraBounds.instance == null) return false;

        minX = CameraBounds.instance.MinX + spriteSize.x;
        maxX = CameraBounds.instance.MaxX - spriteSize.x;
        minY = CameraBounds.instance.MinY + spriteSize.y;
        maxY = CameraBounds.instance.MaxY - spriteSize.y;

        Vector3 currentPos = transform.position;

        if (currentPos.x <= minX || currentPos.x >= maxX || currentPos.y <= minY || currentPos.y >= maxY)
        {
            return true;
        }

        return false;
    }

    void ReturnToPlayer()
    {
        isReturning = true;
        hitEnemies.Clear();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            IDamageable dmgObj = other.GetComponent<IDamageable>();

            if (dmgObj != null && !hitEnemies.Contains(dmgObj))
            {
                OnHitEnemy(other.gameObject);
                hitEnemies.Add(dmgObj);
            }
        }
    }

    protected override void OnHitEnemy(GameObject enemyObj)
    {
        DamageSystem.ApplyDamage(enemyObj, damage);
    }
}