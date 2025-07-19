using UnityEngine;

public class ComboTextPoolItem : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Play()
    {
        // Do not play animation if any panel is active
        if (PanelManager.instance != null && PanelManager.instance.IsAnyPanelOpen())
            return;

        gameObject.SetActive(true);
        animator.SetTrigger("Pop");
    }

    // Call this at the end of your animation using Animation Event
    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
