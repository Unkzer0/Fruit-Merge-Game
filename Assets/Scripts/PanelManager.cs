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
    [SerializeField] private GameObject boomPowerUpPanel;
    [SerializeField] private GameObject fruitUpgradePowerUpPanel; 
    [SerializeField] private GameObject smallFruitPowerUpPanel; 
    [SerializeField] private GameObject cleanPowerUpPanel;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject themePanel;
    [SerializeField] private GameObject fruitUnlockPanel;


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
               boomPowerUpPanel.activeSelf ||
               smallFruitPowerUpPanel.activeSelf ||
               cleanPowerUpPanel.activeSelf ||
               fruitUpgradePowerUpPanel.activeSelf ||
               settingsOnMainMenuPanel.activeSelf ||
               shopPanel.activeSelf ||
               themePanel.activeSelf ||
               fruitUnlockPanel.activeSelf; //  Add this line
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
        boomPowerUpPanel.SetActive(false);
        smallFruitPowerUpPanel.SetActive(false);
        cleanPowerUpPanel.SetActive(false);
        fruitUpgradePowerUpPanel.SetActive(false);
        settingsOnMainMenuPanel.SetActive(false);
        shopPanel.SetActive(false);
        themePanel.SetActive(false);
    }
    public bool IsGameOverShown => gameOverPanel != null && gameOverPanel.activeSelf;
    // Panel Shortcuts
    public void ShowMainMenu() { PlayClickSound(); ShowOnly(mainMenuPanel); }
    public void ShowGameOver() { ShowOnly(gameOverPanel); }
    public void ShowSettings() { PlayClickSound(); ShowOnly(settingsPanel); }
    public void ShowSettingOnMainMenu() { PlayClickSound(); ShowOnly(settingsOnMainMenuPanel); }
    public void ShowBoompowerUp() { PlayClickSound(); ShowOnly(boomPowerUpPanel); }
    public void ShowSmallFruitpowerUp() { PlayClickSound(); ShowOnly(smallFruitPowerUpPanel); }
    public void ShowClearFruitpowerUp() { PlayClickSound(); ShowOnly(cleanPowerUpPanel); }
    public void ShowFruitUpgradepowerUp() { PlayClickSound(); ShowOnly(fruitUpgradePowerUpPanel); }
    public void ShowShop() { PlayClickSound(); ShowOnly(shopPanel); }
    public void ShowtTheme() { PlayClickSound(); ShowOnly(themePanel); }
}
