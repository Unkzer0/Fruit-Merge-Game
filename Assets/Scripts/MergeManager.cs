using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeManager : MonoBehaviour
{
    public static MergeManager instance;

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
            GameObject newFruit = Instantiate(FruitSelector.instance.Fruits[nextIndex], spawnPos, Quaternion.identity);

            Fruit fruitScript = newFruit.GetComponent<Fruit>();
            if (fruitScript != null)
            {
                fruitScript.fruitIndex = nextIndex;
            }
        }
    }


}
