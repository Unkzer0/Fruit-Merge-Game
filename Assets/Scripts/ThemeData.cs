using UnityEngine;

[CreateAssetMenu(fileName = "NewTheme", menuName = "Theme/ThemeData")]
public class ThemeData : ScriptableObject
{
    public string themeName;
    public Sprite bearSprite;
    public Sprite mainMenuBackground;
    public Sprite gameOverPanelBackground;
    public Sprite gameSceneBackground;
    public Sprite logo;

    public Sprite[] fruitSprites; // Order should match your fruit types
}

