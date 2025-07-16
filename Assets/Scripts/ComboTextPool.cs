using UnityEngine;
using System.Collections.Generic;

public class ComboTextPool : MonoBehaviour
{
    [System.Serializable]
    public class ComboEntry
    {
        public int mergeCount;                    // e.g., 4, 6, 8...
        public ComboTextPoolItem comboTextItem;   // The actual GameObject with animation
    }

    [SerializeField] private List<ComboEntry> comboEntries;
    private Dictionary<int, ComboTextPoolItem> comboMap;

    private void Awake()
    {
        comboMap = new Dictionary<int, ComboTextPoolItem>();
        foreach (var entry in comboEntries)
        {
            if (entry.comboTextItem != null)
            {
                entry.comboTextItem.gameObject.SetActive(false); // Hide initially
                comboMap[entry.mergeCount] = entry.comboTextItem;
            }
        }
    }

    public void TryPlayComboText(int totalMergeCount)
    {
        if (comboMap.TryGetValue(totalMergeCount, out ComboTextPoolItem item))
        {
            item.Play();
        }
    }
}
