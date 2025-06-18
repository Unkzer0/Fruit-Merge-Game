using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class GameOverTrigger : MonoBehaviour
{
 
    [Header("Screenshot Settings")]
    [SerializeField] private Camera screenshotCamera;
    [SerializeField] private int screenshotWidth = 1080;
    [SerializeField] private int screenshotHeight = 1920;
    [SerializeField] private int screenshotAntiAliasing = 2;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Fruit")) return;

        int finalScore = ScoreManager.instance.GetCurrentScore();
        Destroy(other.gameObject);

        // Ensure camera is disabled normally
        screenshotCamera.enabled = true;

        // Capture clean render
        RenderTexture rt = ScreenshotUtility.CaptureCustomArea(
            screenshotCamera,
            screenshotWidth,
            screenshotHeight,
            screenshotAntiAliasing
        );

        screenshotCamera.enabled = false;

        // Pass to GameOver panel
        if (PanelManager.instance?.gameOverPanelScript != null)
        {
            PanelManager.instance.gameOverPanelScript.SetScreenshot(rt);
        }

        StartCoroutine(ShowGameOverDelayed(finalScore));
    }

    private IEnumerator ShowGameOverDelayed(int finalScore)
    {
        yield return new WaitForSeconds(5f);

        if (PanelManager.instance != null)
        {
            PanelManager.instance.ShowGameOver();
            PanelManager.instance.gameOverPanelScript?.ShowScore(finalScore);
        }
    }
}


