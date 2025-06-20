using UnityEngine;
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

        int finalScore = ScoreManager.instance?.GetCurrentScore() ?? 0;

        Destroy(other.gameObject);

        if (screenshotCamera != null)
        {
            screenshotCamera.enabled = true;

            RenderTexture rt = ScreenshotUtility.CaptureCustomArea(
                screenshotCamera,
                screenshotWidth,
                screenshotHeight,
                screenshotAntiAliasing
            );

            screenshotCamera.enabled = false;

            if (PanelManager.instance?.gameOverPanelScript != null)
            {
                PanelManager.instance.gameOverPanelScript.SetScreenshot(rt);
            }
        }

        StartCoroutine(ShowGameOverDelayed(finalScore));
    }

    private IEnumerator ShowGameOverDelayed(int score)
    {
        yield return new WaitForSeconds(5f);

        if (PanelManager.instance != null)
        {
            PanelManager.instance.ShowGameOver();
            PanelManager.instance.gameOverPanelScript?.ShowScore(score);
        }
    }
}


