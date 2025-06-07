using UnityEngine;

public class GameOverTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Fruit"))
        {
            int finalScore = ScoreManager.instance.GetCurrentScore();
            Sprite screenshot = ScreenshotUtility.CaptureScreenshotAsSprite();

            PanelManager.instance.ShowGameOverPanel(finalScore, screenshot);
            Destroy(other.gameObject);
        }
    }
}
