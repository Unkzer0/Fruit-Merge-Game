using GoogleMobileAds.Api;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private RawImage screenshotDisplay;
    [SerializeField] private Button restartButton;
    [SerializeField] private AudioClip buttonClickSound;
    [SerializeField] private AudioClip applauseSound;

    [SerializeField] private BannerAd bannerAd;
    [SerializeField] private RewardAd rewardAd;
    [SerializeField] private Interstitial_Ad interstitialAd;


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
        if (rewardAd != null)
        {
            rewardAd.DestroyRewardedAd();
        }
        if (interstitialAd != null)
        {
            interstitialAd.HandleAdClosed();
        }
        if (bannerAd != null)
        {
            bannerAd.DestroyAd();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
