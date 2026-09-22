using UnityEngine;

public class fly : Enemy
{
    [Header("Movimento Ondulado")]
    public float amplitude = 0.5f;
    public float frequency = 2f;
    private float startY;

    protected override void Start()
    {
        base.Start();
        SetDirection(Vector2.left);
        startY = transform.position.y;
    }

    void Update()
    {
        float horizontalMovement = direction.x * speed * Time.deltaTime;
        float verticalOffset = Mathf.Sin(Time.time * frequency) * amplitude;

        transform.Translate(new Vector2(horizontalMovement, 0));
        transform.position = new Vector3(transform.position.x, startY + verticalOffset, transform.position.z);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall2"))
        {
            Destroy(gameObject);
        }
    }
}
