using UnityEngine;
using System.Collections.Generic;

public class SmallFruitRemovePowerUp : MonoBehaviour
{
    private bool isActive = false;

    public void Activate()
    {
        isActive = true;
        TryRemoveSmallestFruits();
    }

    private void TryRemoveSmallestFruits()
    {
        GameObject[] allFruits = GameObject.FindGameObjectsWithTag("Fruit");
        if (allFruits.Length == 0)
        {
            isActive = false;
            return;
        }

        Dictionary<int, List<GameObject>> fruitsByIndex = new Dictionary<int, List<GameObject>>();

        foreach (GameObject fruit in allFruits)
        {
            string fruitName = fruit.name;

            // Extract the numeric index from name (e.g., "2. Orange")
            int dotIndex = fruitName.IndexOf('.');
            if (dotIndex > 0 && int.TryParse(fruitName.Substring(0, dotIndex), out int fruitIndex))
            {
                if (!fruitsByIndex.ContainsKey(fruitIndex))
                    fruitsByIndex[fruitIndex] = new List<GameObject>();

                fruitsByIndex[fruitIndex].Add(fruit);
            }
        }

        if (fruitsByIndex.Count == 0)
        {
            isActive = false;
            return;
        }

        // Find the smallest index
        int minIndex = int.MaxValue;
        foreach (int index in fruitsByIndex.Keys)
        {
            if (index < minIndex)
                minIndex = index;
        }

        // Destroy all fruits with the smallest index
        foreach (GameObject fruit in fruitsByIndex[minIndex])
        {
            Destroy(fruit);
        }

        isActive = false;
    }
}
