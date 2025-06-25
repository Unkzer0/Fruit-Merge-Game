using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
 


    private int currentScore = 0;
    private int highScore = 0;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateScoreUI();
    }

    public void AddScore(int points)
    {
        currentScore += points;

        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save(); // Ensure it is immediately written to disk
        }

        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = currentScore.ToString();

        if (highScoreText != null)
            highScoreText.text = $"{highScore}";
    }

    public int GetCurrentScore() => currentScore;

    public int GetHighScore() => highScore;

    public void ResetScore()
    {
        currentScore = 0;
        UpdateScoreUI();
    }
}
