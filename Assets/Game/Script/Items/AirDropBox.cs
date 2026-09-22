using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AirDropBox : MonoBehaviour,  IDamageable
{
   public float speed;
   public float Hp;
   public Vector2 Direction; 
   private FlashEffect flashEffect;
   public LootTable lootTable;
   [Range(0f,1f)] public float dropChance = 1f;

    void Start()
    {
        flashEffect = GetComponent<FlashEffect>();
    }

    public void SetDirection(Vector2 dir)
    {
        Direction = dir.normalized; 
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
           Destroy(gameObject); 
        } 
    }


    public void TakeDamage(float damagefb)
    {
      Hp -= damagefb;
      
      flashEffect.Flash();
      
      if (Hp <= 0)
      {
        death();
      }
    }

     public void death()
    {

        if (Random.value <= dropChance) 
        {
            GameObject drop = lootTable.GetRandomDrop();
            if (drop != null)
            {
               Instantiate(drop, transform.position, Quaternion.identity);
            }
        }
        Destroy(gameObject);
    }



}
