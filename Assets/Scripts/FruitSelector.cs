using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FruitSelector : MonoBehaviour
{
    public static FruitSelector instance;

    [Header("Fruit Prefabs")]
    public GameObject[] Fruits;

    [Header("UI")]
    [SerializeField] private Image nextFruitImage;
    [SerializeField] private Sprite[] fruitSprites;

    [Header("Fruit Settings")]
    public int HighestStartingIndex = 3; // Fruit pool: 0 to this index
    [SerializeField] private float[] weights; // Matching spawn chances

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
        ValidateWeights();
        PickNextFruit();
    }

    public void PickNextFruit()
    {
        NextFruitIndex = GetWeightedRandomIndex();
        NextFruit = Fruits[NextFruitIndex];

        if (nextFruitImage != null && fruitSprites.Length > NextFruitIndex)
        {
            nextFruitImage.sprite = fruitSprites[NextFruitIndex];
        }
    }

    public GameObject GetFruitToSpawn()
    {
        GameObject fruit = NextFruit;
        PickNextFruit();
        return fruit;
    }

    private int GetWeightedRandomIndex()
    {
        float totalWeight = 0f;
        for (int i = 0; i <= HighestStartingIndex && i < weights.Length; i++)
        {
            totalWeight += weights[i];
        }

        float randomValue = Random.value * totalWeight;

        for (int i = 0; i <= HighestStartingIndex && i < weights.Length; i++)
        {
            if (randomValue < weights[i])
                return i;
            randomValue -= weights[i];
        }

        return 0; // fallback
    }

    private void ValidateWeights()
    {
        if (weights == null || weights.Length < HighestStartingIndex + 1)
        {
            Debug.LogWarning("Weights not set properly. Applying default equal weights.");
            weights = new float[HighestStartingIndex + 1];
            for (int i = 0; i <= HighestStartingIndex; i++)
                weights[i] = 1f;
        }
    }
}