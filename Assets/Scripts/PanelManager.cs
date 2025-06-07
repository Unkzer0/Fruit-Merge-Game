using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PanelManager : MonoBehaviour
{
   public static PanelManager instance;

    [Header("Game Over Panel")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Text scoreText;
    [SerializeField] private Image screenshotImage;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button shareButton;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        gameOverPanel.SetActive(false);

        restartButton.onClick.AddListener(RestartGame);
        shareButton.onClick.AddListener(ShareScreenshot);
    }

    public void ShowGameOverPanel(int finalScore, Sprite screenshot)
    {
        gameOverPanel.SetActive(true);
        scoreText.text = "Score: " + finalScore;
        screenshotImage.sprite = screenshot;
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ShareScreenshot()
    {
        // Platform-specific share code (placeholder)
        Debug.Log("Share functionality goes here.");
    }
}
