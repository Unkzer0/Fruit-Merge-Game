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

        // Capture Screenshot if camera is assigned
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

            PanelManager.instance?.gameOverPanelScript?.SetScreenshot(rt);
        }

        StartCoroutine(ShowGameOverDelayed());
    }

    private IEnumerator ShowGameOverDelayed()
    {
        yield return new WaitForSeconds(5f);

        if (PanelManager.instance != null)
        {
            PanelManager.instance.ShowGameOver();
            PanelManager.instance.gameOverPanelScript?.ShowScore();
        }
    }
}


