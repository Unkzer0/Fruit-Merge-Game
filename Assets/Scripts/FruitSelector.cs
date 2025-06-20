using System.Collections;
using UnityEngine;

public class FruitSelector : MonoBehaviour
{
    public static FruitSelector instance;

    [Header("Fruit Prefabs & Sprites")]
    public GameObject[] Fruits;
    [SerializeField] private Sprite[] fruitSprites;

    [Header("Current & Next Fruit Display")]
    [SerializeField] private SpriteRenderer currentFruitSpriteRenderer;
    [SerializeField] private SpriteRenderer nextFruitSpriteRenderer;
    [SerializeField] private Transform displayFollowTransform;
    [SerializeField] private Vector3 offset = new Vector3(0, 1f, 0);
    [SerializeField] private float bobbingAmplitude = 0.1f;
    [SerializeField] private float bobbingSpeed = 2f;
    [SerializeField] private Vector3[] nextFruitScales; // Scale overrides for next fruit display

    [Header("Fruit Settings")]
    public int HighestStartingIndex = 3;

    public GameObject CurrentFruit { get; private set; }
    public int CurrentFruitIndex { get; private set; }

    public GameObject NextFruit { get; private set; }
    public int NextFruitIndex { get; private set; }

    private float lastTouchTime;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        lastTouchTime = Time.time;
        InitializeFruitQueue();
    }

    private void Update()
    {
        if (displayFollowTransform == null || currentFruitSpriteRenderer == null) return;

        Vector3 position = displayFollowTransform.position + offset;

        // Bobbing effect if idle
        if (Time.time - lastTouchTime > 10f)
        {
            position += new Vector3(0, Mathf.Sin(Time.time * bobbingSpeed) * bobbingAmplitude, 0);
        }

        currentFruitSpriteRenderer.transform.position = position;
    }

    public void NotifyTouch()
    {
        lastTouchTime = Time.time;
    }

    private void InitializeFruitQueue()
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

        // Shift next fruit to current
        CurrentFruitIndex = NextFruitIndex;
        CurrentFruit = Fruits[CurrentFruitIndex];

        // Pick new next
        NextFruitIndex = GetRandomIndex();
        NextFruit = Fruits[NextFruitIndex];

        UpdateUI();
        return fruit;
    }

    private void UpdateUI()
    {
        if (fruitSprites.Length > CurrentFruitIndex)
            currentFruitSpriteRenderer.sprite = fruitSprites[CurrentFruitIndex];

        if (fruitSprites.Length > NextFruitIndex)
        {
            nextFruitSpriteRenderer.sprite = fruitSprites[NextFruitIndex];

            // Apply consistent scaling with z = 1
            if (nextFruitScales != null && nextFruitScales.Length > NextFruitIndex)
            {
                Vector3 fixedScale = nextFruitScales[NextFruitIndex];
                fixedScale.z = 1f;
                nextFruitSpriteRenderer.transform.localScale = fixedScale;
            }
        }
    }

    private int GetRandomIndex()
    {
        return Random.Range(0, Mathf.Min(HighestStartingIndex + 1, Fruits.Length));
    }
}
