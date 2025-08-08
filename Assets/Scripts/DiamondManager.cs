using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondManager : MonoBehaviour
{
    public static DiamondManager Instance { get; private set; }

    private int diamondCount;
    private const string DiamondKey = "DiamondCount";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadDiamonds();
    }

    private void LoadDiamonds()
    {
        diamondCount = PlayerPrefs.GetInt(DiamondKey, 0);
    }

    private void SaveDiamonds()
    {
        PlayerPrefs.SetInt(DiamondKey, diamondCount);
        PlayerPrefs.Save();
    }

    // Public read-only access
    public int GetDiamondCount()
    {
        return diamondCount;
    }

    // Public method to add diamonds (used by IAP or rewards)
    public void AddDiamonds(int amount)
    {
        if (amount <= 0) return;
        diamondCount += amount;
        SaveDiamonds();
    }

    // Method to spend diamonds, returns true if successful
    public bool SpendDiamonds(int amount)
    {
        if (amount <= 0 || amount > diamondCount) return false;

        diamondCount -= amount;
        Debug.Log($"Spent {amount} diamonds. Remaining: {diamondCount}");
        SaveDiamonds();
        return true;
    }

    // Optional for debug or reset
    [ContextMenu("Reset Diamonds")]
    private void ResetDiamonds()
    {
        diamondCount = 0;
        SaveDiamonds();
    }
}

