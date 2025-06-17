using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FruitBarUIManager : MonoBehaviour
{
    public static FruitBarUIManager instance;
    public int StartIndex = 5; // First unlockable fruit index


    [SerializeField] private FruitBarSlot[] slots; // index matches fruit index

    private bool[] unlocked;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        unlocked = new bool[slots.Length];
    }

    public void UnlockFruit(int fruitIndex)
    {
        int slotIndex = fruitIndex - StartIndex;
        if (slotIndex < 0 || slotIndex >= slots.Length || unlocked[slotIndex]) return;

        unlocked[slotIndex] = true;
        slots[slotIndex].Reveal();
    }


    public bool IsUnlocked(int fruitIndex)
    {
        return fruitIndex < unlocked.Length && unlocked[fruitIndex];
    }
}

 
