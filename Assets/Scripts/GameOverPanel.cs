using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverPanel : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private RawImage screenshotDisplay;
    [SerializeField] private Button restartButton;
    [SerializeField] private AudioClip buttonClickSound;
    [SerializeField] private AudioClip applauseSound;


    [Header("Effects")]
    [SerializeField] private ParticleSystem highScoreEffect; 

    private RenderTexture capturedScreenshot;

   public void ShowScore()
{
    int current = ScoreManager.instance.GetCurrentScore();
    int high = ScoreManager.instance.GetHighScore();

    if (scoreText != null)
        scoreText.text = $"Score: {current}";

    if (highScoreText != null)
    {
        if (current > 0 && current == high)
        {
            highScoreText.text = "New HighScore!!";
            highScoreText.gameObject.SetActive(true);

            if (applauseSound != null && !SoundManager.instance.IsSFXMuted())
            {
                SoundManager.instance.PlayButtonClick(applauseSound);
            }
            if (highScoreEffect != null)
            {
                    highScoreEffect.Play();
            }

        }
        else
        {
            highScoreText.gameObject.SetActive(false);
        }
    }
}


    public void SetScreenshot(RenderTexture rt)
    {
        capturedScreenshot = rt;
        if (screenshotDisplay != null && rt != null)
            screenshotDisplay.texture = capturedScreenshot;
    }

    private void Start()
    {
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(() =>
            {
                PlayClickSound();
                StartCoroutine(RestartAfterSound());
            });
        }
    }

    private void PlayClickSound()
    {
        if (buttonClickSound != null)
        {
            SoundManager.instance?.PlayButtonClick(buttonClickSound);
        }
    }

    private System.Collections.IEnumerator RestartAfterSound()
    {
        yield return new WaitForSeconds(0.3f);
        PlayerPrefs.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
