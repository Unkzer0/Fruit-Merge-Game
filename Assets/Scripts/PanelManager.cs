using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public static PanelManager instance;
    public GameOverPanel gameOverPanelScript;

    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject settingsOnMainMenuPanel;
    [SerializeField] private GameObject comingSoonPanel;
    [SerializeField] private GameObject comingSoonForMainMenuPanel;
    [SerializeField] private AudioClip buttonClickSound;

    private bool justClosedPanel = false;
    private static bool hasLaunchedOnce = false;

    public bool JustClosedPanel => justClosedPanel;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        if (!hasLaunchedOnce)
        {
            hasLaunchedOnce = true;
            ShowOnly(mainMenuPanel);  // First launch: show main menu
        }
        else
        {
            ShowOnly(null);           // On restart: no panels
        }
    }

    private void LateUpdate()
    {
        justClosedPanel = false; // Reset each frame
    }

    private void PlayClickSound()
    {
        if (buttonClickSound != null)
        {
            SoundManager.instance?.PlayButtonClick(buttonClickSound);
        }
    }

    public bool IsAnyPanelOpen()
    {
        return mainMenuPanel.activeSelf ||
               gameOverPanel.activeSelf ||
               settingsPanel.activeSelf ||
               comingSoonPanel.activeSelf ||
               settingsOnMainMenuPanel.activeSelf ||
               comingSoonForMainMenuPanel.activeSelf;
    }

    public static bool AnyPanelOrJustClosed =>
        instance != null && (instance.IsAnyPanelOpen() || instance.JustClosedPanel);

    public void ShowOnly(GameObject panelToShow)
    {
        HideAllPanels();
        justClosedPanel = true;

        if (panelToShow != null)
            panelToShow.SetActive(true);
    }

    private void HideAllPanels()
    {
        mainMenuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        settingsPanel.SetActive(false);
        comingSoonPanel.SetActive(false);
        settingsOnMainMenuPanel.SetActive(false);
        comingSoonForMainMenuPanel.SetActive(false);
    }
    public bool IsGameOverShown => gameOverPanel != null && gameOverPanel.activeSelf;
    // Panel Shortcuts
    public void ShowMainMenu() { PlayClickSound(); ShowOnly(mainMenuPanel); }
    public void ShowGameOver() { ShowOnly(gameOverPanel); }
    public void ShowSettings() { PlayClickSound(); ShowOnly(settingsPanel); }
    public void ShowSettingOnMainMenu() { PlayClickSound(); ShowOnly(settingsOnMainMenuPanel); }
    public void ShowComingSoon() { PlayClickSound(); ShowOnly(comingSoonPanel); }
    public void ShowComingSoonFromMainMenu() { PlayClickSound(); ShowOnly(comingSoonForMainMenuPanel); }
}
