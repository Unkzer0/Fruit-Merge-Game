using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameOverTrigger : MonoBehaviour
{
    [Header("Screenshot Settings")]
    [SerializeField] private Camera screenshotCamera;
    [SerializeField] private int screenshotWidth = 1080;
    [SerializeField] private int screenshotHeight = 1920;
    [SerializeField] private int screenshotAntiAliasing = 2;

    [Header("References")]
    [SerializeField] private FruitDropperController fruitDropperController;

    [Header("Game Over Timing")]
    [SerializeField] private float gameOverDelay = 3f;

    private Dictionary<Collider2D, Coroutine> activeTimers = new Dictionary<Collider2D, Coroutine>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Fruit")) return;

        if (!activeTimers.ContainsKey(other))
        {
            Coroutine timer = StartCoroutine(DelayedGameOver(other));
            activeTimers.Add(other, timer);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (activeTimers.TryGetValue(other, out Coroutine timer))
        {
            if (timer != null)
            {
                StopCoroutine(timer);
            }

            activeTimers.Remove(other);
        }
    }

    private IEnumerator DelayedGameOver(Collider2D fruit)
    {
        float elapsed = 0f;
        Collider2D trigger = GetComponent<Collider2D>();

        while (elapsed < gameOverDelay)
        {
            if (!trigger.bounds.Intersects(fruit.bounds))
            {
                activeTimers.Remove(fruit);
                yield break;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        activeTimers.Clear();

        // Disable dropper input
        fruitDropperController?.DisableInput();

        // Freeze fruit (instead of destroying it)
        Rigidbody2D rb = fruit.GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = false;

        // Capture screenshot
        if (screenshotCamera != null)
        {
            screenshotCamera.enabled = true;

            RenderTexture rt = ScreenshotUtility.CaptureCustomArea(
                screenshotCamera,
                screenshotWidth,
                screenshotHeight,
                screenshotAntiAliasing
            );

            screenshotCamera.enabled = false;
            PanelManager.instance?.gameOverPanelScript?.SetScreenshot(rt);
        }

        // Wait before showing Game Over
        yield return new WaitForSeconds(0.5f);

        PanelManager.instance?.ShowGameOver();
        PanelManager.instance?.gameOverPanelScript?.ShowScore();
        GameSaveManager.instance?.DeleteSave();
    }
}
