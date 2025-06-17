using UnityEngine;
using UnityEngine.UI;

public class GameOverTrigger : MonoBehaviour
{
    [Header("Screenshot Settings")]
    [SerializeField] private Camera screenshotCamera;
    [SerializeField] private int screenshotWidth;
    [SerializeField] private int screenshotHeight; 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Fruit"))
        {
            int finalScore = ScoreManager.instance.GetCurrentScore();
            Destroy(other.gameObject);

            // Enable screenshot camera just before rendering
            screenshotCamera.enabled = true;
            RenderTexture rt = ScreenshotUtility.CaptureCustomArea(screenshotCamera, screenshotWidth, screenshotHeight);
            screenshotCamera.enabled = false;

            PanelManager.instance?.gameOverPanelScript?.SetScreenshot(rt);

            StartCoroutine(ShowGameOverDelayed(finalScore));
        }
    }

    private System.Collections.IEnumerator ShowGameOverDelayed(int finalScore)
    {
        yield return new WaitForSeconds(5f);

        PanelManager.instance?.ShowGameOver();

        if (PanelManager.instance?.gameOverPanelScript != null)
        {
            PanelManager.instance.gameOverPanelScript.ShowScore(finalScore);
        }
    }
}
