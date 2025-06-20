using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsPanel : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button musicToggleButton;
    [SerializeField] private Button sfxToggleButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button resumeButton;

    [Header("Music UI")]
    [SerializeField] private Image musicIcon;
    [SerializeField] private Sprite musicOnSprite;
    [SerializeField] private Sprite musicOffSprite;

    [Header("SFX UI")]
    [SerializeField] private Image sfxIcon;
    [SerializeField] private Sprite sfxOnSprite;
    [SerializeField] private Sprite sfxOffSprite;

    private void Start()
    {
        musicToggleButton?.onClick.AddListener(OnMusicToggle);
        sfxToggleButton?.onClick.AddListener(OnSFXToggle);
        restartButton?.onClick.AddListener(RestartGame);
        resumeButton?.onClick.AddListener(() => PanelManager.instance?.ShowOnly(null));

        // Set initial icon states
        UpdateMusicIcon();
        UpdateSFXIcon();
    }

    private void OnMusicToggle()
    {
        Music.instance?.ToggleMusic();
        UpdateMusicIcon();
    }

    private void OnSFXToggle()
    {
        MergeManager.instance?.ToggleSFX();
        FindObjectOfType<FruitDropperController>()?.ToggleSFX();
        UpdateSFXIcon();
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void UpdateMusicIcon()
    {
        if (musicIcon == null || Music.instance == null) return;

        musicIcon.sprite = Music.instance.IsMusicMuted() ? musicOffSprite : musicOnSprite;
    }

    private void UpdateSFXIcon()
    {
        if (sfxIcon == null || MergeManager.instance == null) return;

        sfxIcon.sprite = MergeManager.instance.IsSFXMuted() ? sfxOffSprite : sfxOnSprite;
    }
}
