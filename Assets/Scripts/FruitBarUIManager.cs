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
        // Load unlocked fruits from PlayerPrefs
        for (int i = 0; i < unlocked.Length; i++)
        {
            int fruitIndex = StartIndex + i;
            unlocked[i] = PlayerPrefs.GetInt(GetFruitUnlockKey(fruitIndex), 0) == 1;
            if (unlocked[i])
            {
                slots[i].Reveal();
            }
        }
    }

    public void UnlockFruit(int fruitIndex)
    {
        int slotIndex = fruitIndex - StartIndex;

        if (!IsValidSlotIndex(slotIndex) || unlocked[slotIndex]) return;

        unlocked[slotIndex] = true;
        slots[slotIndex].Reveal();

        // Save unlock state in PlayerPrefs
        PlayerPrefs.SetInt(GetFruitUnlockKey(fruitIndex), 1);
        PlayerPrefs.Save();

        // Show unlock animation panel
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

    private string GetFruitUnlockKey(int fruitIndex)
    {
        return $"FruitUnlocked_{fruitIndex}";
    }
}
