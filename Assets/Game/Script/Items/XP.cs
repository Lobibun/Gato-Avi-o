using UnityEngine;

public class XP : MonoBehaviour
{
    public int xpAmount = 10;
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

        void OnTriggerEnter2D(Collider2D col)
        {
        if (col.CompareTag("Player"))
        {
            col.GetComponent<Player>().GainXp(xpAmount);
            Destroy(gameObject);
        }
        }
}
