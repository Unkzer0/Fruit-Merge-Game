using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Text scoreText;
    [SerializeField] private RawImage screenshotDisplay;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button shareButton;

    private RenderTexture capturedScreenshot;

    public void ShowScore(int score)
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    public void SetScreenshot(RenderTexture rt)
    {
        capturedScreenshot = rt;

        if (screenshotDisplay != null)
        {
            screenshotDisplay.texture = capturedScreenshot;
        }
    }

    private void Start()
    {
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene(0);
            });
        }

        if (shareButton != null)
        {
            shareButton.onClick.AddListener(ShareScreenshot);
        }
    }

    private void ShareScreenshot()
    {
        Debug.Log("Share button clicked. Screenshot sharing logic goes here.");
        // Optional: Implement screenshot saving + native share intent (for mobile)
    }
}
