using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuPanel : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Button playButton;
    [SerializeField] private TextMeshProUGUI highScoreText;

    [Header("Audio")]
    [SerializeField] private AudioClip buttonClickSound;

    private void Start()
    {
        if (playButton != null)
        {
            playButton.onClick.AddListener(() =>
            {
                PlayClickSound();
                StartCoroutine(HidePanelAfterDelay(0.45f));
            });
        }

        UpdateHighScoreText();
    }

    private void UpdateHighScoreText()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0); // Or ScoreManager.instance?.GetHighScore()
        if (highScoreText != null)
        {
            highScoreText.text = $"{highScore} HIGHSCORE";
        }
    }

    private IEnumerator HidePanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        PanelManager.instance.ShowOnly(null);

        // Load save after game UI becomes visible
        GameSaveManager.instance.RestoreGame();
    }

    private void PlayClickSound()
    {
        if (buttonClickSound != null && Camera.main != null)
        {
            AudioSource.PlayClipAtPoint(buttonClickSound, Camera.main.transform.position);
        }
    }
}



