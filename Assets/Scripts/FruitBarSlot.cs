using UnityEngine;
using UnityEngine.UI;

public class FruitBarSlot : MonoBehaviour
{
    [Header("Slot Elements")]
    public Image questionMarkImage;
    public Image fruitImage;

    /// <summary>
    /// Reveals the fruit image by destroying the question mark and setting full alpha.
    /// </summary>
    public void Reveal()
    {
        if (questionMarkImage != null)
        {
            Destroy(questionMarkImage.gameObject);
            questionMarkImage = null;
        }

        if (fruitImage != null)
        {
            var color = fruitImage.color;
            color.a = 1f;
            fruitImage.color = color;
        }
    }
}


