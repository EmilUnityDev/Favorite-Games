using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public float timeLimit = 30f;
    private float timeRemaining;
    public Text timerText;
    public GameObject gameOverPanel;
    public GameObject JoystickObj;
    public GameObject player;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        
        timeRemaining = timeLimit;
        gameOverPanel.SetActive(false);
        scoreText.text = "Score: " + score.ToString();
    }
    public int score;
    public Text scoreText;
    private void Update()
    {
        // Убедитесь, что таймер не становится отрицательным
        if (timeRemaining < 0)
        {
            timeRemaining = 0;
        }

        // Рассчитываем минуты и секунды
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            
        }
        else
        {
            GameOver();
        }
    }

    void GameOver()
    {
        gameOverPanel.SetActive(true);
        timerText.gameObject.SetActive(false);
        player.SetActive(false);
        JoystickObj.SetActive(false);
        int coin =  PlayerPrefs.GetInt("coin2",0);
        if(coin < score)
        {
            PlayerPrefs.SetInt("coin2", score);
        }

    }

    public void RestartGame()
    {
        Time.timeScale = 1; // Возобновляем игру
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void AddCoin()
    {
        score += Random.Range(50, 100) + (int)(25 * Random.Range(0.1f, 2.0f));
        scoreText.text ="Score: " + score.ToString();
    }
}
