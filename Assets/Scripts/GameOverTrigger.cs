using UnityEngine;

public class GameOverTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Fruit"))
        {
            int finalScore = ScoreManager.instance.GetCurrentScore();
            Destroy(other.gameObject); // Immediately destroy the fruit

            // Start coroutine to show Game Over panel after 5 seconds
            StartCoroutine(ShowGameOverDelayed(finalScore));
        }
    }

    private System.Collections.IEnumerator ShowGameOverDelayed(int finalScore)
    {
        yield return new WaitForSeconds(5f);

        if (PanelManager.instance != null)
        {
            PanelManager.instance.ShowGameOver();

            if (PanelManager.instance.gameOverPanelScript != null)
            {
                PanelManager.instance.gameOverPanelScript.ShowScore(finalScore);
            }
            else
            {
                Debug.LogError("GameOverPanel script is not assigned in PanelManager.");
            }
        }
        else
        {
            Debug.LogError("PanelManager.instance is null.");
        }
    }
}
