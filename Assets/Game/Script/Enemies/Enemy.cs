using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamageable
{
    [Header("Status")]
    public float hp;
    public GameObject xp;
    public float speed;

    protected Vector2 direction;
    protected FlashEffect flashEffect;

    protected virtual void Start()
    {
        flashEffect = GetComponent<FlashEffect>();
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    public virtual void TakeDamage(float damageAmount)
    {
        hp -= damageAmount;

        if (flashEffect != null)
            flashEffect.Flash();

        if (hp <= 0)
        {
            Death();
        }
    }

    protected virtual void Death()
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
}
