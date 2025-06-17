using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningTrigger : MonoBehaviour
{
    [SerializeField] private WarningLineController warningLine;
    private Coroutine warningCoroutine;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Fruit"))
        {
            // Start delayed warning check
            warningCoroutine = StartCoroutine(StartWarningAfterDelay(other));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Fruit"))
        {
            // Cancel if the fruit exits early
            if (warningCoroutine != null)
            {
                StopCoroutine(warningCoroutine);
                warningCoroutine = null;
            }
        }
    }

    private IEnumerator StartWarningAfterDelay(Collider2D fruit)
    {
        float delay = 5f;
        float elapsed = 0f;

        // Wait only if the same fruit stays in the trigger
        while (elapsed < delay)
        {
            if (!IsFruitStillTouching(fruit))
                yield break; // Fruit exited early, cancel

            elapsed += Time.deltaTime;
            yield return null;
        }

        warningLine?.TriggerWarningBlink();
    }

    private bool IsFruitStillTouching(Collider2D fruit)
    {
        // Check if the fruit is still overlapping
        return GetComponent<Collider2D>().bounds.Intersects(fruit.bounds);
    }
}