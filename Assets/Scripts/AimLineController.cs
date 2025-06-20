using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class AimLineController : MonoBehaviour
{
    [SerializeField] private Transform dropper;   // Reference to the fruit dropper
    [SerializeField] private float bottomY = -4f; // Y position where line ends

    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void LateUpdate()
    {
        if (!dropper) return;

        Vector3 dropperPos = dropper.position;
        lineRenderer.SetPosition(0, new Vector3(dropperPos.x, dropperPos.y, 0f));
        lineRenderer.SetPosition(1, new Vector3(dropperPos.x, bottomY, 0f));
    }
}

