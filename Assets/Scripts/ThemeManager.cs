using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ThemeManager : MonoBehaviour
{
    public static ThemeManager instance;

    [Header("All Themes")]
    public ThemeData[] allThemes;

    [Header("Theme Data")]
    [SerializeField] private ThemeData currentTheme;

    [Header("UI References")]
    [SerializeField] private SpriteRenderer bearSpriteRenderer;
    [SerializeField] private Image mainMenuBackground;
    [SerializeField] private Image gameOverPanelBackground;
    [SerializeField] private SpriteRenderer gameSceneBackgroundRenderer;
    [SerializeField] private Image logo;

    [Header("Theme Particle Effects")]
    [SerializeField] private ParticleSystem summerParticle;
    [SerializeField] private ParticleSystem winterParticle;

    private const string SelectedThemeKey = "SelectedTheme";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        LoadTheme();
    }

    public void ApplyTheme(ThemeData theme)
    {
        currentTheme = theme;

        // Save theme name
        PlayerPrefs.SetString(SelectedThemeKey, theme.themeName);
        PlayerPrefs.Save();

        // Update bear sprite
        if (bearSpriteRenderer != null)
            bearSpriteRenderer.sprite = theme.bearSprite;

        // Update Logo
        if (logo != null)
            logo.sprite = theme.logo;

        // Update backgrounds
        if (mainMenuBackground != null)
            mainMenuBackground.sprite = theme.mainMenuBackground;

        if (gameOverPanelBackground != null)
            gameOverPanelBackground.sprite = theme.gameOverPanelBackground;

        if (gameSceneBackgroundRenderer != null)
            gameSceneBackgroundRenderer.sprite = theme.gameSceneBackground;

        // Update fruit prefabs & UI
        UpdateFruitSprites(theme.fruitSprites);

        // Update fruits already present
        UpdateExistingFruitsInScene(theme.fruitSprites);

        // Activate/deactivate particle effects
        if (theme.themeName == "Summer")
        {
            if (summerParticle != null) summerParticle.gameObject.SetActive(true);
            if (winterParticle != null) winterParticle.gameObject.SetActive(false);
        }
        else if (theme.themeName == "Christmas")
        {
            if (summerParticle != null) summerParticle.gameObject.SetActive(false);
            if (winterParticle != null) winterParticle.gameObject.SetActive(true);
        }
        else
        {
            // Deactivate both if theme is neither
            if (summerParticle != null) summerParticle.gameObject.SetActive(false);
            if (winterParticle != null) winterParticle.gameObject.SetActive(false);
        }
    }

    private void LoadTheme()
    {
        string savedThemeName = PlayerPrefs.GetString(SelectedThemeKey, "");

        if (!string.IsNullOrEmpty(savedThemeName))
        {
            ThemeData savedTheme = allThemes.FirstOrDefault(t => t.themeName == savedThemeName);
            if (savedTheme != null)
            {
                ApplyTheme(savedTheme);
                return;
            }
        }

        // Fallback to default theme
        if (currentTheme != null)
            ApplyTheme(currentTheme);
    }

    private void UpdateExistingFruitsInScene(Sprite[] fruitSprites)
    {
        Fruit[] existingFruits = FindObjectsOfType<Fruit>();

        foreach (Fruit fruit in existingFruits)
        {
            int index = fruit.fruitIndex;
            if (index >= 0 && index < fruitSprites.Length)
            {
                SpriteRenderer sr = fruit.GetComponentInChildren<SpriteRenderer>();
                if (sr != null)
                    sr.sprite = fruitSprites[index];
            }
        }
    }

    private void UpdateFruitSprites(Sprite[] fruitSprites)
    {
        if (fruitSprites == null || fruitSprites.Length == 0) return;

        FruitSelector fruitSelector = FruitSelector.instance;
        if (fruitSelector != null)
        {
            for (int i = 0; i < fruitSelector.Fruits.Length; i++)
            {
                GameObject fruitPrefab = fruitSelector.Fruits[i];
                if (fruitPrefab != null && i < fruitSprites.Length)
                {
                    SpriteRenderer spriteRenderer = fruitPrefab.GetComponentInChildren<SpriteRenderer>();
                    if (spriteRenderer != null)
                        spriteRenderer.sprite = fruitSprites[i];
                }
            }
        }
    }
}
