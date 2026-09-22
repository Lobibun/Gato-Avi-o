using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControler : MonoBehaviour
{
    [Header("Hp")]
    public Text HpText;
    private Player player; 

    [Header("UI do Placar")]
    public Text scoreText; 
    private int killScore = 0;
    public Text CoinText;
    private int coinScore = 0;

    [Header("Cronômetro")]
    public Text timerText; 
    private float elapsedTime = 0f;

    public static GameControler instance;
    public GameObject gameOver;

    void Start()
    {
        UpdateScoreText();
        UpdateCoinText();
        instance = this;
        
        player = FindObjectOfType<Player>();
        if (player != null)
        {
            UpdateHpText(player.currentHp);
        }
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        UpdateTimerText();
    }

    public void UpdateHpText(float currentHp)
    {
        if (HpText != null)
        {
            HpText.text = currentHp.ToString("0.#"); 
        }
    }

    public void AddCoins(int amount)
    {
        float multiplier = 1f; 

        if (StatusManager.instance != null)
        {
            multiplier += StatusManager.instance.GetGreedBonus();
        }

        int finalAmount = (int)(amount * multiplier); 
        coinScore += finalAmount;
        UpdateCoinText();

        if (Player.instance != null)
        {
            FloatingText.Create(
                "+" + finalAmount.ToString(),
                Player.instance.transform.position,
                Color.yellow
            );
        }
    }

    public void ShowGameOver()
    {
        gameOver.SetActive(true);
    }

    public void RestartGame(string lvlName)
    {
        SceneManager.LoadScene(lvlName);
    }

    public void AddKill()
    {
        killScore++;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = killScore.ToString();
        }
    }

    private void UpdateCoinText()
    {
        if (CoinText != null)
        {
            CoinText.text = coinScore.ToString();
        }
    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);

        if (timerText != null)
        {
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}