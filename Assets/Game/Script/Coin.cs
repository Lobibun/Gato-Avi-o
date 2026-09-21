using UnityEngine;

public class Coin : MonoBehaviour
{
    public int CoinAmount = 1;
    public float moveSpeed = 8f; 
    public float baseMagnetRange = 2f;
    private Transform playerTransform;
    private bool isMagnetized = false;

    void Start()
    {
        if (Player.instance != null)
        {
            playerTransform = Player.instance.transform;
        }
        else
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) playerTransform = p.transform;
        }
    }
        void Update()
    {
        if (playerTransform == null) return;

        float distance = Vector3.Distance(transform.position, playerTransform.position);
        float currentMagnetRange = baseMagnetRange;
        
        if (StatusManager.instance != null)
        {
            currentMagnetRange += StatusManager.instance.GetMagnetBonus();
        }

        if (distance < currentMagnetRange)
        {
            isMagnetized = true;
        }

        if (isMagnetized)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);
        }
    }

    public void GainCoin(int amount)
    {
        if (GameControler.instance != null)
        {
            GameControler.instance.AddCoins(amount);
        }
    }
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            GainCoin(CoinAmount);
            Destroy(gameObject);
        }
    }
}
