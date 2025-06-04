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

    private Camera mainCam;
    private bool isDragging = false;
    private Vector3 targetPos;
    private bool canDrop = true;

    private void Start()
    {
        mainCam = Camera.main;
        targetPos = transform.position;
    }

    private void Update()
    {
#if UNITY_EDITOR
        HandleEditorInput();
#else
        HandleTouchInput();
#endif

        if (isDragging)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, dragSpeed * Time.deltaTime);
        }
    }

    private void HandleEditorInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MoveToPoint(Input.mousePosition, instant: true);
            TryDropFruit();
        }
        else if (Input.GetMouseButton(0))
        {
            MoveToPoint(Input.mousePosition, instant: false);
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            MoveToPoint(touch.position, instant: true);
            TryDropFruit();
        }
        else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
        {
            MoveToPoint(touch.position, instant: false);
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

    private void TryDropFruit()
    {
        if (!canDrop || FruitSelector.instance == null) return;

        Vector3 spawnPos = dropSpawnPoint != null ? dropSpawnPoint.position : transform.position;
        GameObject fruit = Instantiate(FruitSelector.instance.GetFruitToSpawn(), spawnPos, Quaternion.identity);

        // Disable further drops until this one settles
        canDrop = false;

        // Attach Fruit.cs script and assign callback
        Fruit fruitScript = fruit.GetComponent<Fruit>();
        if (fruitScript != null)
        {
            fruitScript.onSettled = () => { canDrop = true; };
        }
    }
}
