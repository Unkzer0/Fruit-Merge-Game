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
    [SerializeField] private GameObject comingsoonformainmenu;

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
            ShowOnly(mainMenuPanel); // Show only on first game launch
        }
        else
        {
            ShowOnly(null); // No panel on restart
        }
    }



    private void LateUpdate()
    {
        justClosedPanel = false; // Reset after each frame
    }

    public bool IsAnyPanelOpen()
    {
        return mainMenuPanel.activeSelf ||
               gameOverPanel.activeSelf ||
               settingsPanel.activeSelf ||
               comingsoonPanel.activeSelf||
               comingsoonformainmenu.activeSelf;
    }

    public static bool AnyPanelOrJustClosed =>
        instance != null && (instance.IsAnyPanelOpen() || instance.JustClosedPanel);

    public void ShowOnly(GameObject panelToShow)
    {
        // Hide all panels
        mainMenuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        settingsPanel.SetActive(false);
        comingsoonPanel.SetActive(false);
        comingsoonformainmenu.SetActive(false);

        justClosedPanel = true; // Delay input for 1 frame

        if (panelToShow != null)
            panelToShow.SetActive(true);
    }

    // Helpers
    public void ShowMainMenu() => ShowOnly(mainMenuPanel);
    public void ShowGameOver() => ShowOnly(gameOverPanel);
    public void ShowSettings() => ShowOnly(settingsPanel);
    public void ShowComingSoon() => ShowOnly(comingsoonPanel);
    public void showComingSoonformainmenu() => ShowOnly(comingsoonformainmenu);
}

