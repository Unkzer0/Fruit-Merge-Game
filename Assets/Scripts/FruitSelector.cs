using UnityEngine;
using UnityEngine.UI;

public class FruitSelector : MonoBehaviour
{
    public static FruitSelector instance;

    [Header("Fruit Prefabs")]
    public GameObject[] Fruits; // First 4 fruits: Cherry, Lemon, Orange, Grapes

    [Header("UI Display")]
    [SerializeField] private Sprite[] fruitSprites;
    [SerializeField] private Sprite[] nextFruitSprites;
    [SerializeField] private SpriteRenderer currentFruitSpriteRenderer;
    [SerializeField] private Image nextFruitUIImage;

    [SerializeField] private Transform displayFollowTransform;
    [SerializeField] private Vector3 offset = new Vector3(0, 1f, 0);
    [SerializeField] private float bobbingAmplitude = 0.1f;
    [SerializeField] private float bobbingSpeed = 2f;

    [Header("Fruit Settings")]
    public int HighestStartingIndex = 3; // Only 0 to 3 fruits are droppable


    private int[] layerFruitCounts = new int[3]; // Index 0 = Layer1, 1 = Layer2, 2 = Layer3
    private bool[] activeLayers = new bool[3];
    private Layer previousLayer = Layer.Layer1;

    [Header("Layer Probabilities (Cherry to Grapes)")]
    [SerializeField] private float[] layer1Chances = { 65f, 25f, 8f, 2f };
    [SerializeField] private float[] layer2Chances = { 40f, 30f, 20f, 10f };
    [SerializeField] private float[] layer3Chances = { 10f, 20f, 30f, 40f };

    private enum Layer { Layer1, Layer2, Layer3 }
    [SerializeField] private Layer currentLayer = Layer.Layer1;

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

    private int GetRandomIndex()
    {
        float[] chances = currentLayer switch
        {
            Layer.Layer1 => layer1Chances,
            Layer.Layer2 => layer2Chances,
            Layer.Layer3 => layer3Chances,
            _ => layer1Chances
        };

        float total = 0f;
        foreach (float c in chances) total += c;

        float rand = Random.Range(0f, total);
        float sum = 0f;

        for (int i = 0; i < chances.Length; i++)
        {
            sum += chances[i];
            if (rand <= sum) return i;
        }

        return 0; // fallback
    }

    public void SetLayerFromTrigger(int layerNum)
    {
        Layer newLayer = layerNum switch
        {
            1 => Layer.Layer1,
            2 => Layer.Layer2,
            3 => Layer.Layer3,
            _ => Layer.Layer1
        };

        if (currentLayer != newLayer)
        {
            previousLayer = currentLayer;
            currentLayer = newLayer;
        }
    }
    public void FruitEnteredLayer(int layerNum)
    {
        layerFruitCounts[layerNum - 1]++;
    }

    public void FruitExitedLayer(int layerNum)
    {
        layerFruitCounts[layerNum - 1]--;

        if (layerFruitCounts[0] <= 0 && layerFruitCounts[1] <= 0 && layerFruitCounts[2] <= 0)
        {
            currentLayer = previousLayer;
        }
    }


    public void NotifyLayerEmpty(int layerNum)
    {
        activeLayers[layerNum - 1] = false;

        // If all layers are inactive, return to previous layer
        if (!activeLayers[0] && !activeLayers[1] && !activeLayers[2])
        {
            currentLayer = previousLayer;
        }
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

    private void UpdateUI()
    {
        if (fruitSprites.Length > CurrentFruitIndex && currentFruitSpriteRenderer != null)
            currentFruitSpriteRenderer.sprite = fruitSprites[CurrentFruitIndex];

        if (nextFruitSprites.Length > NextFruitIndex && nextFruitUIImage != null)
            nextFruitUIImage.sprite = nextFruitSprites[NextFruitIndex];
    }

    public string DebugLayer => currentLayer.ToString();
}
