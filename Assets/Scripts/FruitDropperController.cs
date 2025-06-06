using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitDropperController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float dragSpeed = 10f;
    [SerializeField] private float minX = -2.5f;
    [SerializeField] private float maxX = 2.5f;

    [Header("Fruit Drop Settings")]
    [SerializeField] private Transform dropSpawnPoint;
    [SerializeField] private float dropCooldown = 0.5f;
    [SerializeField] private float swipeDropThreshold = 1000f; // Pixels/second
    [SerializeField] private AudioSource dropSound; // Assign a drop sound here

    private Camera mainCam;
    private bool isDragging = false;
    private Vector3 targetPos;
    private bool canDrop = true;

    private Vector2 lastTouchPosition;
    private float lastTouchTime;

    private void Start()
    {
        mainCam = Camera.main;
        targetPos = transform.position;
    }

    private void Update()
    {
        HandleTouchInput();

        if (isDragging)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, dragSpeed * Time.deltaTime);
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                lastTouchPosition = touch.position;
                lastTouchTime = Time.time;
                MoveToPoint(touch.position, instant: true);
                break;

            case TouchPhase.Moved:
            case TouchPhase.Stationary:
                MoveToPoint(touch.position, instant: false);
                break;

            case TouchPhase.Ended:
                float deltaTime = Time.time - lastTouchTime;
                float swipeSpeed = (touch.position - lastTouchPosition).magnitude / deltaTime;

                // Drop only on swipe or release
                if (swipeSpeed > swipeDropThreshold || deltaTime > 0.05f)
                {
                    TryDropFruit();
                }
                break;
        }
    }

    private void MoveToPoint(Vector2 screenPosition, bool instant)
    {
        Vector3 worldPos = mainCam.ScreenToWorldPoint(screenPosition);
        float clampedX = Mathf.Clamp(worldPos.x, minX, maxX);
        Vector3 newTargetPos = new Vector3(clampedX, transform.position.y, transform.position.z);

        if (instant)
        {
            transform.position = newTargetPos;
            isDragging = false;
        }
        else
        {
            targetPos = newTargetPos;
            isDragging = true;
        }
    }

    public void SetCanDrop(bool value)
    {
        canDrop = value;
    }

    private void TryDropFruit()
    {
        if (!canDrop || FruitSelector.instance == null) return;

        Vector3 spawnPos = dropSpawnPoint != null ? dropSpawnPoint.position : transform.position;
        GameObject fruit = Instantiate(FruitSelector.instance.GetFruitToSpawn(), spawnPos, Quaternion.identity);

        canDrop = false;

        // Play drop sound
        if (dropSound != null)
        {
            dropSound.Play();
        }

        Fruit fruitScript = fruit.GetComponent<Fruit>();
        if (fruitScript != null)
        {
            fruitScript.onSettled = () => { StartCoroutine(EnableDropAfterDelay()); };
        }
        else
        {
            StartCoroutine(EnableDropAfterDelay());
        }
    }

    public IEnumerator EnableDropAfterDelay()
    {
        yield return new WaitForSeconds(dropCooldown);
        canDrop = true;
    }
}
