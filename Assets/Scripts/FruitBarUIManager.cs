using UnityEngine;

public class FruitBarUIManager : MonoBehaviour
{
    public static FruitBarUIManager instance;

    [Header("Fruit Unlock Settings")]
    public int StartIndex = 5; // First unlockable fruit index

    [SerializeField] private FruitBarSlot[] slots; // Matches fruit index - StartIndex
    [SerializeField] private FruitUnlockPanel fruitUnlockPanel; //  Add this

    private bool[] unlocked;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Initialize()
    {
        if (slots == null || slots.Length == 0)
        {
            Debug.LogWarning("FruitBarUIManager: No slots assigned.");
            return;
        }

        unlocked = new bool[slots.Length];
    }

    public void UnlockFruit(int fruitIndex)
    {
        int slotIndex = fruitIndex - StartIndex;

        if (!IsValidSlotIndex(slotIndex) || unlocked[slotIndex]) return;

        unlocked[slotIndex] = true;
        slots[slotIndex].Reveal();

        //  Show unlock animation panel
        if (fruitUnlockPanel != null)
        {
            fruitUnlockPanel.ShowFruitUnlock(fruitIndex);
        }
    }

    public bool IsUnlocked(int fruitIndex)
    {
        int slotIndex = fruitIndex - StartIndex;
        return IsValidSlotIndex(slotIndex) && unlocked[slotIndex];
    }

    private bool IsValidSlotIndex(int index)
    {
        return index >= 0 && index < slots.Length;
    }
}
