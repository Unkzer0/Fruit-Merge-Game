using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningLineController : MonoBehaviour
{
        private LineRenderer lineRenderer;
        private Coroutine blinkCoroutine;

        private void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
            SetupLine();
        }

        private void SetupLine()
        {
            SetLineColor(new Color(1, 0, 0, 0));
        }

        private void SetLineColor(Color c)
        {
            lineRenderer.startColor = c;
            lineRenderer.endColor = c;
        }

        public void TriggerWarningBlink()
        {
            if (blinkCoroutine != null)
                StopCoroutine(blinkCoroutine);

            blinkCoroutine = StartCoroutine(BlinkLine());
        }

        private IEnumerator BlinkLine()
        {
            Color visible = new Color(1, 0, 0, 1);   // Red
            Color transparent = new Color(1, 0, 0, 0); // Transparent

            for (int i = 0; i < 6; i++)
            {
                SetLineColor(visible);
                yield return new WaitForSeconds(0.2f);
                SetLineColor(transparent);
                yield return new WaitForSeconds(0.2f);
            }

            SetLineColor(transparent);
            blinkCoroutine = null;
        }
    }
