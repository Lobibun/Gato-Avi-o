using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour
{
    public static Player instance;
    public float speed;
    private Camera mainCamera;
    private float minX, maxX, minY, maxY;

    private Vector2 playerSpriteSize;
    private FlashEffect flashEffect;
    public GameObject bulletPrefab; 
    private bool isDead = false;
    [Header("Vida")]
    public float maxHp = 5;
    public float currentHp;
    [Header("XP e Nível")]
     public int currentLevel = 1;
     public float currentXp = 0;
     public float xpToNextLevel = 30;
     public Slider xpSlider;
     public Text levelText;

     [SerializeField] public Collider2D bodyCollider;


    void Start()
    {
        StartCoroutine(RegenerationRoutine());
        flashEffect = GetComponent<FlashEffect>();
        instance = this;
        mainCamera = Camera.main;
        playerSpriteSize = GetComponent<SpriteRenderer>().bounds.extents;
         currentHp = maxHp;
             UpdateLevelText(); 
        UpdateXpBar();
         if (GameControler.instance != null)
        {
            GameControler.instance.UpdateHpText(currentHp);
        }
         
    }

    void Update()
{
   
    Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
    float currentSpeed = speed; 
    if (StatusManager.instance != null)
    {
        
        currentSpeed += StatusManager.instance.GetMovementSpeedBonus();
    }
    transform.position += movement * currentSpeed * Time.deltaTime;

    UpdateBounds();
    ClampPlayerPosition();
}

    void UpdateBounds()
    {
        float camDistance = Mathf.Abs(mainCamera.transform.position.z - transform.position.z);

        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, camDistance));
        Vector3 topRight   = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, camDistance));

        minX = bottomLeft.x + playerSpriteSize.x;
        maxX = topRight.x - playerSpriteSize.x;

        minY = bottomLeft.y + playerSpriteSize.y;
        maxY = topRight.y - playerSpriteSize.y;
    }

    void ClampPlayerPosition()
    {
        Vector3 currentPosition = transform.position;

        float clampedX = Mathf.Clamp(currentPosition.x, minX, maxX);
        float clampedY = Mathf.Clamp(currentPosition.y, minY, maxY);

        transform.position = new Vector3(clampedX, clampedY, currentPosition.z);
    }

     void OnTriggerEnter2D(Collider2D other)
  {
        if (bodyCollider == null) return;
        
         if (!other.IsTouching(bodyCollider)) return;

        EnemyDamage enemy = other.GetComponent<EnemyDamage>();


        if (enemy == null) return;
        
            TakeDamage(enemy.damageAmount);

             if (enemy .destroyOnImpact)
            {
                Destroy(other.gameObject);
            }
           
        
    }

    void TakeDamage(float damage)
    {
       if (isDead) return;

       float defense = 0;
       if (StatusManager.instance != null)
       {
           defense = StatusManager.instance.GetArmorBonus();
       }

       float finalDamage = Mathf.Max(1f, damage - defense);
       Debug.Log($"Dano Original: {damage} | Defesa: {defense} | Dano Final: {finalDamage}");
       flashEffect.Flash(); 
       currentHp -= finalDamage; 
       
       GameControler.instance.UpdateHpText(currentHp); 
       
       if (currentHp <= 0)
       {
           KillPlayer();
       }
    }
    
     public void KillPlayer()
     {
        GameControler.instance.ShowGameOver();
        Destroy(gameObject);
     }

     public void GainXp(float amount)
   {
    float multiplier = 1f; 

   if (StatusManager.instance != null) multiplier += StatusManager.instance.GetGrowthBonus();
    
    currentXp += amount * multiplier;

    while (currentXp >= xpToNextLevel)
    {
        LevelUp();
    }

     UpdateXpBar(); 
   }

private void LevelUp()
{
    currentLevel++;
    currentXp -= xpToNextLevel; 
    xpToNextLevel *= 1.2f;
    UpdateLevelText(); 

    UpdateXpBar(); 
    UpgradeManager.instance.TriggerLevelUp();
}

public void UpdateXpBar()
{
    if (xpSlider != null)
    {
        xpSlider.maxValue = xpToNextLevel;
        xpSlider.value = currentXp;
    }
}
public void UpdateLevelText()
{
    if (levelText != null)
    {
        levelText.text = "Nvl: " + currentLevel;
    }
}


IEnumerator RegenerationRoutine()
    {
        while (!isDead)
        {
            float bonusCura = StatusManager.instance.GetRecoveryBonus();
            if (currentHp < maxHp && bonusCura > 0)
            {
                currentHp = Mathf.Min(currentHp + bonusCura, maxHp);
                if (GameControler.instance != null)
                {
                    GameControler.instance.UpdateHpText(currentHp);
                }
            }
         yield return new WaitForSeconds(1f);
        }
    }



}
