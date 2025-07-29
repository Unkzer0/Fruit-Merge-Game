using UnityEngine;
using UnityEngine.UI;

public class ThemeManager : MonoBehaviour
{
    public static ThemeManager instance;

    [Header("Theme Data")]
    [SerializeField] private ThemeData currentTheme;

    [Header("UI References")]
    [SerializeField] private SpriteRenderer bearSpriteRenderer; // Changed from Image to SpriteRenderer
    [SerializeField] private Image mainMenuBackground;
    [SerializeField] private Image gameOverPanelBackground;
    [SerializeField] private SpriteRenderer gameSceneBackgroundRenderer; // Changed from Image to SpriteRenderer
    [SerializeField] private Image logo;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void ApplyTheme(ThemeData theme)
    {
        currentTheme = theme;

        // Update bear sprite
        if (bearSpriteRenderer != null)
            bearSpriteRenderer.sprite = theme.bearSprite;

        //Update Logo
        if(logo != null)
            logo.sprite = theme.logo;

        // Update backgrounds
        if (mainMenuBackground != null)
            mainMenuBackground.sprite = theme.mainMenuBackground;

        if (gameOverPanelBackground != null)
            gameOverPanelBackground.sprite = theme.gameOverPanelBackground;

        if (gameSceneBackgroundRenderer != null)
            gameSceneBackgroundRenderer.sprite = theme.gameSceneBackground;

        // Update fruit sprites
        UpdateFruitSprites(theme.fruitSprites);
    }

    private void UpdateFruitSprites(Sprite[] fruitSprites)
    {
        if (fruitSprites == null || fruitSprites.Length == 0) return;

        // Update fruit prefabs in FruitSelector
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

            // Update current and next fruit sprites
            if (fruitSelector.CurrentFruitIndex < fruitSprites.Length)
                fruitSelector.currentFruitSpriteRenderer.sprite = fruitSprites[fruitSelector.CurrentFruitIndex];

            if (fruitSelector.NextFruitIndex < fruitSprites.Length)
                fruitSelector.nextFruitUIImage.sprite = fruitSprites[fruitSelector.NextFruitIndex];
        }
    }
}