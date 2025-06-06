using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeManager : MonoBehaviour
{
    public static MergeManager instance;

    [SerializeField] private AudioClip mergeSound;
    [SerializeField] private float bounceScale = 1.2f;
    [SerializeField] private float bounceDuration = 0.2f;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void MergeFruits(int currentIndex, GameObject fruit1, GameObject fruit2, Vector3 spawnPos)
    {
        Fruit fruitScript1 = fruit1.GetComponent<Fruit>();

        if (fruitScript1 != null && fruitScript1.fruitData != null)
        {
            int points = fruitScript1.fruitData.scoreValue;
            ScoreManager.instance.AddScore(points);
        }

        Destroy(fruit1);
        Destroy(fruit2);

        int nextIndex = currentIndex + 1;
        if (nextIndex < FruitSelector.instance.Fruits.Length)
        {
            // Get the prefab scale
            Vector3 prefabScale = FruitSelector.instance.Fruits[nextIndex].transform.localScale;

            // Spawn the new fruit and apply correct scale
            GameObject newFruit = Instantiate(FruitSelector.instance.Fruits[nextIndex], spawnPos, Quaternion.identity);
            newFruit.transform.localScale = prefabScale;

            //  Play merge sound
            if (mergeSound != null)
            {
                AudioSource.PlayClipAtPoint(mergeSound, spawnPos);
            }

            //  Bounce animation
            LeanTween.scale(newFruit, prefabScale * bounceScale, bounceDuration)
                .setEaseOutBounce()
                .setOnComplete(() =>
                {
                    LeanTween.scale(newFruit, prefabScale, 0.1f);
                });

            // Setup merged fruit
            Fruit fruitScript = newFruit.GetComponent<Fruit>();
            if (fruitScript != null)
            {
                fruitScript.fruitIndex = nextIndex;

                // Allow dropper to reset when fruit settles
                fruitScript.onSettled = () =>
                {
                    FruitDropperController dropper = FindObjectOfType<FruitDropperController>();
                    if (dropper != null)
                    {
                        dropper.StartCoroutine(dropper.EnableDropAfterDelay());
                    }
                };
            }
        }
    }
}
