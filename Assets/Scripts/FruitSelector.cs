using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FruitSelector : MonoBehaviour
{
    public static FruitSelector instance;
    private float lastTouchTime;

    [Header("Fruit Prefabs")]
    public GameObject[] Fruits;

    [Header("Fruit Sprites")]
    [SerializeField] private Sprite[] fruitSprites;

    [Header("Current & Next Fruit Display")]
    [SerializeField] private SpriteRenderer currentFruitSpriteRenderer;
    [SerializeField] private SpriteRenderer nextFruitSpriteRenderer;
    [SerializeField] private Transform displayFollowTransform;
    [SerializeField] private Vector3 offset = new Vector3(0, 1f, 0);
    [SerializeField] private float bobbingAmplitude = 0.1f;
    [SerializeField] private float bobbingSpeed = 2f;

    [Header("Fruit Settings")]
    public int HighestStartingIndex = 3;
    [SerializeField] private Vector3[] nextFruitScales; // New: custom scale per fruit

    public GameObject CurrentFruit { get; private set; }
    public int CurrentFruitIndex { get; private set; }

    public GameObject NextFruit { get; private set; }
    public int NextFruitIndex { get; private set; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        InitFruitQueue();
        lastTouchTime = Time.time;
    }

    private void Update()
    {
        if (displayFollowTransform != null && currentFruitSpriteRenderer != null)
        {
            bool isIdle = Time.time - lastTouchTime > 10f;

            Vector3 position = displayFollowTransform.position + offset;

            if (isIdle)
            {
                Vector3 bobbing = new Vector3(0, Mathf.Sin(Time.time * bobbingSpeed) * bobbingAmplitude, 0);
                position += bobbing;
            }

            currentFruitSpriteRenderer.transform.position = position;
        }
    }

    public void NotifyTouch()
    {
        lastTouchTime = Time.time;
    }

    private void InitFruitQueue()
    {
        CurrentFruitIndex = GetRandomIndex();
        CurrentFruit = Fruits[CurrentFruitIndex];

        NextFruitIndex = GetRandomIndex();
        NextFruit = Fruits[NextFruitIndex];

        UpdateUI();
    }

    public GameObject GetFruitToSpawn()
    {
        GameObject fruit = CurrentFruit;

        CurrentFruitIndex = NextFruitIndex;
        CurrentFruit = Fruits[CurrentFruitIndex];

        NextFruitIndex = GetRandomIndex();
        NextFruit = Fruits[NextFruitIndex];

        UpdateUI();
        return fruit;
    }

    private void UpdateUI()
    {
        if (currentFruitSpriteRenderer != null && fruitSprites.Length > CurrentFruitIndex)
        {
            currentFruitSpriteRenderer.sprite = fruitSprites[CurrentFruitIndex];
        }

        if (nextFruitSpriteRenderer != null && fruitSprites.Length > NextFruitIndex)
        {
            nextFruitSpriteRenderer.sprite = fruitSprites[NextFruitIndex];

            // Apply scale safely with Z = 1
            if (nextFruitScales.Length > NextFruitIndex)
            {
                Vector3 scale = nextFruitScales[NextFruitIndex];
                scale.z = 1f;
                nextFruitSpriteRenderer.transform.localScale = scale;
            }
        }
    }

    private int GetRandomIndex()
    {
        return Random.Range(0, HighestStartingIndex + 1);
    }
}