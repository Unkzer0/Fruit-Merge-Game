using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

    private bool sfxMuted = false;
    private Camera mainCam;
    private bool isDragging = false;
    private Vector3 targetPos;
    private bool canDrop = true;

    private Vector2 lastTouchPosition;
    private float lastTouchTime;

    private void Awake()
    {
        sfxMuted = PlayerPrefs.GetInt("SFXMuted", 0) == 1;
    }

    private void Start()
    {
        mainCam = Camera.main;
        targetPos = transform.position;
    }

    private void Update()
    {
        // Block all input if any panel is open or just closed
        if (PanelManager.AnyPanelOrJustClosed)
            return;

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

        // Ignore touches that began over UI (like the Play button)
        if (touch.phase == TouchPhase.Began && IsTouchOverUI(touch))
            return;

        switch (touch.phase)
        {
            case TouchPhase.Began:
                lastTouchPosition = touch.position;
                lastTouchTime = Time.time;
                FruitSelector.instance?.NotifyTouch();
                MoveToPoint(touch.position, instant: true);
                break;

            case TouchPhase.Moved:
            case TouchPhase.Stationary:
                FruitSelector.instance?.NotifyTouch();
                MoveToPoint(touch.position, instant: false);
                break;

            case TouchPhase.Ended:
                float swipeSpeed = (touch.position - lastTouchPosition).magnitude / (Time.time - lastTouchTime);

                // Drop if fast swipe OR small movement (tap)
                if (swipeSpeed > swipeDropThreshold || (touch.position - lastTouchPosition).magnitude < 50f)
                {
                    TryDropFruit();
                }
                break;
        }
    }

    private bool IsTouchOverUI(Touch touch)
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(touch.fingerId);
    }

    private void MoveToPoint(Vector2 screenPosition, bool instant)
    {
        Vector3 screenPoint = new Vector3(screenPosition.x, screenPosition.y, mainCam.WorldToScreenPoint(transform.position).z);
        Vector3 worldPos = mainCam.ScreenToWorldPoint(screenPoint);
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

    public void ToggleSFX()
    {
        sfxMuted = !sfxMuted;
        PlayerPrefs.SetInt("SFXMuted", sfxMuted ? 1 : 0);
    }

    public bool IsSFXMuted() => sfxMuted;

    private void TryDropFruit()
    {
        if (!canDrop || FruitSelector.instance == null) return;

        Vector3 spawnPos = dropSpawnPoint != null ? dropSpawnPoint.position : transform.position;
        GameObject fruit = Instantiate(FruitSelector.instance.GetFruitToSpawn(), spawnPos, Quaternion.identity);

        canDrop = false;

        if (!sfxMuted && dropSound != null)
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

