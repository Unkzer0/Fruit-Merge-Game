using UnityEngine;

public class ComboTextPoolItem : MonoBehaviour
{
    private Animator animator;

    [Header("Sound")]
    public AudioClip popSound;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Play()
    {
        // Skip if any panel is open
        if (PanelManager.instance != null && PanelManager.instance.IsAnyPanelOpen())
            return;

        gameObject.SetActive(true);
        animator.SetTrigger("Pop");

        // Play using SoundManager
        if (popSound != null && SoundManager.instance != null)
        {
            SoundManager.instance.PlayButtonClick(popSound);
        }
    }

    // Called by Animation Event
    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
