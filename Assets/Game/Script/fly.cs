using UnityEngine;
using System.Collections;

public class fly : MonoBehaviour, IDamageable
{
     public float hp;
      public float speed = 3f;
    private Vector2 direction;
    public GameObject xp;
    [Header("Movimento Ondulado")]
     public float amplitude = 0.5f;
     public float frequency = 2f;
     private float startY; 
     private FlashEffect flashEffect; 

    void Start()
    {
          SetDirection(Vector2.left);
          startY = transform.position.y;
          flashEffect = GetComponent<FlashEffect>();
    }

    void Update()
    {
       float horizontalMovement = direction.x * speed * Time.deltaTime;
        float verticalOffset = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.Translate(new Vector2(horizontalMovement, 0));
        transform.position = new Vector3(transform.position.x, startY + verticalOffset, transform.position.z);
    }
    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall2"))
        {
           Destroy(gameObject); 
        } 
    
    }
    public void TakeDamage(float damagefb)
    {
      hp-=damagefb;
      flashEffect.Flash();
      if (hp<=0)
      {
        death();
      }
    }

    public void  death()
    {
      GameControler.instance.AddKill();
        Destroy(gameObject);
        Instantiate(xp, transform.position, Quaternion.identity);
    }
}
