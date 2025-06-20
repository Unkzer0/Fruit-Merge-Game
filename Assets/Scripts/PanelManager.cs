using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public static PanelManager instance;
    public GameOverPanel gameOverPanelScript;

    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject comingSoonPanel;
    [SerializeField] private GameObject comingSoonForMainMenu;

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

    public bool IsAnyPanelOpen()
    {
        return mainMenuPanel.activeSelf ||
               gameOverPanel.activeSelf ||
               settingsPanel.activeSelf ||
               comingSoonPanel.activeSelf ||
               comingSoonForMainMenu.activeSelf;
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
        comingSoonForMainMenu.SetActive(false);
    }

    // Panel Shortcuts
    public void ShowMainMenu() => ShowOnly(mainMenuPanel);
    public void ShowGameOver() => ShowOnly(gameOverPanel);
    public void ShowSettings() => ShowOnly(settingsPanel);
    public void ShowComingSoon() => ShowOnly(comingSoonPanel);
    public void ShowComingSoonFromMainMenu() => ShowOnly(comingSoonForMainMenu);
}
