using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class FruitDropperController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float dragSpeed = 10f;
    [SerializeField] private float minX = -2.5f;
    [SerializeField] private float maxX = 2.5f;

    [Header("Drop Settings")]
    [SerializeField] private Transform dropSpawnPoint;
    [SerializeField] private float dropCooldown = 0.5f;
    [SerializeField] private float swipeDropThreshold = 100f;
    [SerializeField] private AudioSource dropSound;

    private Camera mainCam;
    private Vector3 targetPos;
    private bool canDrop = true;
    private bool isDragging = false;
    private bool sfxMuted = false;
    private bool isInputDisabled = false; 

    private Vector2 lastTouchPos;
    private float lastTouchTime;

    private void Awake()
    {
        sfxMuted = PlayerPrefs.GetInt("SFXMuted", 0) == 1;
        mainCam = Camera.main;
        targetPos = transform.position;
    }

    private void Update()
    {
        if (PanelManager.AnyPanelOrJustClosed || isInputDisabled) return;

        HandleTouch();

        if (isDragging)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, dragSpeed * Time.deltaTime);
        }
    }

    private void HandleTouch()
    {
        if (isInputDisabled || Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began && IsPointerOverUI(touch.fingerId)) return;

        switch (touch.phase)
        {
            case TouchPhase.Began:
                lastTouchPos = touch.position;
                lastTouchTime = Time.time;
                FruitSelector.instance?.NotifyTouch();
                MoveTo(touch.position, true);
                break;

            case TouchPhase.Moved:
            case TouchPhase.Stationary:
                FruitSelector.instance?.NotifyTouch();
                MoveTo(touch.position, false);
                break;

            case TouchPhase.Ended:
                float delta = (touch.position - lastTouchPos).magnitude;
                float speed = delta / (Time.time - lastTouchTime);

                if (speed > swipeDropThreshold || delta < 50f)
                {
                    TryDrop();
                }
                break;
        }
    }

    private void MoveTo(Vector2 screenPos, bool instant)
    {
        if (mainCam == null) return;

        Vector3 screenPoint = new Vector3(screenPos.x, screenPos.y, mainCam.WorldToScreenPoint(transform.position).z);
        Vector3 worldPos = mainCam.ScreenToWorldPoint(screenPoint);
        float clampedX = Mathf.Clamp(worldPos.x, minX, maxX);
        Vector3 newPos = new Vector3(clampedX, transform.position.y, transform.position.z);

        if (instant)
        {
            transform.position = newPos;
            isDragging = false;
        }
        else
        {
            targetPos = newPos;
            isDragging = true;
        }
    }

    private bool IsPointerOverUI(int fingerId)
    {
#if UNITY_EDITOR
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
#else
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(fingerId);
#endif
    }

    private void TryDrop()
    {
        if (!canDrop || FruitSelector.instance == null || isInputDisabled) return;

        Vector3 spawnPos = dropSpawnPoint ? dropSpawnPoint.position : transform.position;
        Instantiate(FruitSelector.instance.GetFruitToSpawn(), spawnPos, Quaternion.identity);

        canDrop = false;

        if (!sfxMuted && dropSound != null)
            dropSound.Play();

        StartCoroutine(EnableDropAfterDelay());
    }

    public IEnumerator EnableDropAfterDelay()
    {
        yield return new WaitForSeconds(dropCooldown);
        canDrop = true;
    }

    public void ToggleSFX()
    {
        sfxMuted = !sfxMuted;
        PlayerPrefs.SetInt("SFXMuted", sfxMuted ? 1 : 0);
    }

    public bool IsSFXMuted() => sfxMuted;
    public void SetCanDrop(bool value) => canDrop = value;

    public void DisableInput()
    {
        isInputDisabled = true;
    }
}
