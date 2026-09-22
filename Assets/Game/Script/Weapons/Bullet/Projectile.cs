using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public float speed;
    protected Vector2 direction;

    public virtual void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        RotateToFaceDirection();
    }

    protected virtual void Update()
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    protected void RotateToFaceDirection()
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    protected virtual void OnHitEnemy(GameObject enemyObj)
    {
    }
}