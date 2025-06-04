using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitDropperController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float minX = -2.5f; // Left boundary
    [SerializeField] private float maxX = 2.5f;  // Right boundary

    private void Update()
    {
        HandleTouchInput();
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);

            // Clamp the x-position to stay within bounds
            float clampedX = Mathf.Clamp(touchPos.x, minX, maxX);

            // Move dropper only along X axis
            Vector3 newPos = new Vector3(clampedX, transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, newPos, moveSpeed * Time.deltaTime);
        }
    }
}
