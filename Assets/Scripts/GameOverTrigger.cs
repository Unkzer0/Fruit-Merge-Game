using UnityEngine;

public class GameOverTrigger : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Fruit"))
        {
            int finalScore = ScoreManager.instance.GetCurrentScore();

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

            Destroy(other.gameObject);
        }
    }
}
