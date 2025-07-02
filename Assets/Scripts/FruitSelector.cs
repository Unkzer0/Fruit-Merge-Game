using UnityEngine;
using UnityEngine.UI;

public class FruitSelector : MonoBehaviour
{
    public static FruitSelector instance;

    [Header("Fruit Prefabs")]
    public GameObject[] Fruits;

    [Header("UI Display")]
    [SerializeField] private Sprite[] fruitSprites;               // For current fruit
    [SerializeField] private Sprite[] nextFruitSprites;           // NEW: For UI next fruit
    [SerializeField] private SpriteRenderer currentFruitSpriteRenderer;
    [SerializeField] private Image nextFruitUIImage;              // NEW: UI Image on canvas

    [SerializeField] private Transform displayFollowTransform;
    [SerializeField] private Vector3 offset = new Vector3(0, 1f, 0);
    [SerializeField] private float bobbingAmplitude = 0.1f;
    [SerializeField] private float bobbingSpeed = 2f;

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

        CurrentFruitIndex = NextFruitIndex;
        CurrentFruit = Fruits[CurrentFruitIndex];

        NextFruitIndex = GetRandomIndex();
        NextFruit = Fruits[NextFruitIndex];

        UpdateUI();
        return fruit;
    }

    public void UpdateUI()
    {
        if (fruitSprites.Length > CurrentFruitIndex && currentFruitSpriteRenderer != null)
        {
            currentFruitSpriteRenderer.sprite = fruitSprites[CurrentFruitIndex];
        }

        if (nextFruitSprites.Length > NextFruitIndex && nextFruitUIImage != null)
        {
            nextFruitUIImage.sprite = nextFruitSprites[NextFruitIndex];
          
        }
    }

    private int GetRandomIndex()
    {
        return Random.Range(0, Mathf.Min(HighestStartingIndex + 1, Fruits.Length));
    }

    public void SetCurrentFruit(int index)
    {
        if (index >= 0 && index < Fruits.Length)
        {
            CurrentFruitIndex = index;
            CurrentFruit = Fruits[index];
        }
    }

    public void SetNextFruit(int index)
    {
        if (index >= 0 && index < Fruits.Length)
        {
            NextFruitIndex = index;
            NextFruit = Fruits[index];
        }
    }

    public void UpdateFruitUI()
    {
        UpdateUI();
    }

}
