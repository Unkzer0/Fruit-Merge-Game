using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]
public class WarningTrigger : MonoBehaviour
{
    [SerializeField] private WarningLineController warningLine;
    [SerializeField] private float warningDelay = 5f;

    private Dictionary<Collider2D, Coroutine> activeWarnings = new Dictionary<Collider2D, Coroutine>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Fruit") && !activeWarnings.ContainsKey(other))
        {
            Coroutine routine = StartCoroutine(DelayedWarning(other));
            activeWarnings.Add(other, routine);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Fruit") && activeWarnings.ContainsKey(other))
        {
            Coroutine routine = activeWarnings[other];
            if (routine != null)
                StopCoroutine(routine);

            activeWarnings.Remove(other);
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
                activeWarnings.Remove(fruit);
                yield break; // Fruit exited early
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        warningLine?.TriggerWarningBlink();
        activeWarnings.Remove(fruit);
    }
}
