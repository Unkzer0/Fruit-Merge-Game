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
        gameObject.SetActive(true);
        animator.SetTrigger("Pop");
    }

    // Call this at the end of your animation using Animation Event
    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
