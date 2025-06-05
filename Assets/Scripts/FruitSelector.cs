using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FruitSelector : MonoBehaviour
{
    public static FruitSelector instance;

    [Header("Fruit Prefabs")]
    public GameObject[] Fruits;

    [Header("Fruit Sprites")]
    [SerializeField] private Sprite[] fruitSprites;

    [Header("Next Fruit Display")]
    [SerializeField] private Transform nextFruitDisplayTransform; // GameObject that moves with dropper
    [SerializeField] private SpriteRenderer nextFruitSpriteRenderer;
    [SerializeField] private Transform dropper;
    [SerializeField] private Vector3 offset = new Vector3(0, 1f, 0); // offset above dropper
    [SerializeField] private float bobbingAmplitude = 0.1f;
    [SerializeField] private float bobbingSpeed = 2f;

    [Header("Fruit Settings")]
    public int HighestStartingIndex = 3;
    [SerializeField] private float[] weights;

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

    private void Update()
    {
        if (nextFruitDisplayTransform != null && dropper != null)
        {
            Vector3 bobbingOffset = offset + new Vector3(0, Mathf.Sin(Time.time * bobbingSpeed) * bobbingAmplitude, 0);
            nextFruitDisplayTransform.position = dropper.position + bobbingOffset;
        }
    }

    public void PickNextFruit()
    {
        NextFruitIndex = GetWeightedRandomIndex();
        NextFruit = Fruits[NextFruitIndex];

        // Update the preview sprite
        if (nextFruitSpriteRenderer != null && fruitSprites.Length > NextFruitIndex)
        {
            nextFruitSpriteRenderer.sprite = fruitSprites[NextFruitIndex];
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
