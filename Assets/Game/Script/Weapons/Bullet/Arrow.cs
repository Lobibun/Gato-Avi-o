using UnityEngine;
using System.Collections.Generic;

public class Arrow : Projectile
{
    private float damage;
    private float lifeTime;
    private float tempoTotalQueDurou;

    private List<IDamageable> hitEnemies = new List<IDamageable>();

    private Camera mainCamera;
    private Vector2 spriteSize;
    private float minX, maxX, minY, maxY;

    public void Setup(Vector2 dir, float dmg, float spd, float life)
    {
        damage = dmg;
        speed = spd;
        lifeTime = life;
        tempoTotalQueDurou = life;

        SetDirection(dir);
    }

    void Start()
    {
        mainCamera = Camera.main;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) spriteSize = sr.bounds.extents;
    }

    protected override void Update()
    {
        base.Update();

        UpdateBoundsAndBounce();

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
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, camDistance));

        minX = bottomLeft.x + spriteSize.x;
        maxX = topRight.x - spriteSize.x;
        minY = bottomLeft.y + spriteSize.y;
        maxY = topRight.y - spriteSize.y;

        Vector3 currentPos = transform.position;
        bool didBounce = false;

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

        if (didBounce)
        {
            transform.position = currentPos;
            RotateToFaceDirection();
            hitEnemies.Clear();
        }
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