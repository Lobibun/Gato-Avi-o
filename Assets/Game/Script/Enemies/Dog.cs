using UnityEngine;

public class Dog : RangedEnemy
{
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
        CheckAndFire();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, direction * visionRange);
    }
}
