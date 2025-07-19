using UnityEngine;
using UnityEngine.UI;

public class FruitUnlockPanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Image fruitImage;
    [SerializeField] private Sprite[] fruitSprites;
    [SerializeField] private Animator panelAnimator;
    private bool isShowing = false;

    private void Awake()
    {
        panel.SetActive(false); // Hide panel by default
    }

    public void ShowFruitUnlock(int fruitIndex)
    {
        if (isShowing) return;
        isShowing = true;


        if (fruitIndex < 0 || fruitIndex >= fruitSprites.Length)
        {
            Debug.LogWarning("Invalid fruit index for unlock panel.");
            return;
        }

        fruitImage.sprite = fruitSprites[fruitIndex];
        panel.SetActive(true);
        panelAnimator.Play("FruitUnlockAnim", 0, 0f); // Reset and play from start
    }

    // Called via Animation Event
    public void OnUnlockAnimationEnd()
    {
        isShowing = false;
        panel.SetActive(false);
    }
}
