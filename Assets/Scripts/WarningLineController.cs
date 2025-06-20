using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class WarningLineController : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Coroutine blinkCoroutine;

    [Header("Blink Settings")]
    [SerializeField] private Color warningColor = new Color(1f, 0f, 0f, 1f);      // Red
    [SerializeField] private Color transparentColor = new Color(1f, 0f, 0f, 0f);  // Invisible
    [SerializeField] private int blinkCount = 6;
    [SerializeField] private float blinkInterval = 0.2f;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        SetLineColor(transparentColor);
    }

    private void SetLineColor(Color color)
    {
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
    }

    public void TriggerWarningBlink()
    {
        if (blinkCoroutine != null)
            StopCoroutine(blinkCoroutine);

        blinkCoroutine = StartCoroutine(BlinkLine());
    }

    private IEnumerator BlinkLine()
    {
        for (int i = 0; i < blinkCount; i++)
        {
            SetLineColor(warningColor);
            yield return new WaitForSeconds(blinkInterval);
            SetLineColor(transparentColor);
            yield return new WaitForSeconds(blinkInterval);
        }

        SetLineColor(transparentColor);
        blinkCoroutine = null;
    }
}
