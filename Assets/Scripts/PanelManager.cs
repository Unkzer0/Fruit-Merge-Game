using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public static PanelManager instance;
    public GameOverPanel gameOverPanelScript;


    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject comingsoonPanel;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        ShowOnly(mainMenuPanel); // Show main menu at start
    }
    public bool IsAnyPanelOpen()
    {
        return (mainMenuPanel.activeSelf ||
                gameOverPanel.activeSelf ||
                settingsPanel.activeSelf) ||
                comingsoonPanel.activeSelf;
    }

    public void ShowOnly(GameObject panelToShow)
    {
        // Hide all panels first
        mainMenuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        settingsPanel.SetActive(false);
        comingsoonPanel.SetActive(false);

        // Show requested panel
        if (panelToShow != null)
        {
            panelToShow.SetActive(true);
        }
    }

    // Helper methods for convenience
    public void ShowMainMenu() => ShowOnly(mainMenuPanel);
    public void ShowGameOver() => ShowOnly(gameOverPanel);
    public void ShowSettings() => ShowOnly(settingsPanel);
    public void ShowComingSoon() => ShowOnly(comingsoonPanel);
}
