using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class WarningTrigger : MonoBehaviour
{
    [SerializeField] private WarningLineController warningLine;
    [SerializeField] private float warningDelay = 5f;

    private Coroutine warningCoroutine;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Fruit"))
        {
            // Start delayed warning if not already running
            if (warningCoroutine == null)
                warningCoroutine = StartCoroutine(DelayedWarning(other));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Fruit"))
        {
            // Cancel ongoing coroutine if the fruit exits early
            if (warningCoroutine != null)
            {
                StopCoroutine(warningCoroutine);
                warningCoroutine = null;
            }
        }
    }

    private IEnumerator DelayedWarning(Collider2D fruit)
    {
        float elapsed = 0f;
        Collider2D trigger = GetComponent<Collider2D>();

        while (elapsed < warningDelay)
        {
            if (!trigger.bounds.Intersects(fruit.bounds))
            {
                warningCoroutine = null;
                yield break; // Fruit exited early
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        warningLine?.TriggerWarningBlink();
        warningCoroutine = null;
    }
}
