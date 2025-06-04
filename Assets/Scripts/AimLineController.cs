using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimLineController : MonoBehaviour
{
    [SerializeField] private Transform dropper;    // Reference to fruit dropper
    [SerializeField] private float bottomY = -4f;  // Y position where line ends

    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (dropper == null) return;

        // Update the start and end points of the line
        Vector3 start = new Vector3(dropper.position.x, dropper.position.y, 0f);
        Vector3 end = new Vector3(dropper.position.x, bottomY, 0f);

        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }
}
