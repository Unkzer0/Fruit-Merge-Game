using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Text scoreText;
    [SerializeField] private RawImage screenshotDisplay;
    [SerializeField] private Button restartButton;

    private RenderTexture capturedScreenshot;

    public void ShowScore(int score)
    {
        if (scoreText != null)
            scoreText.text = $"Score: {score}";
    }

    public void SetScreenshot(RenderTexture rt)
    {
        capturedScreenshot = rt;

        if (screenshotDisplay != null && rt != null)
        {
            screenshotDisplay.texture = capturedScreenshot;
        }
    }

    private void Start()
    {
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
