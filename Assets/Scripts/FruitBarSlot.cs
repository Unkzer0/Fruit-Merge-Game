using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FruitBarSlot : MonoBehaviour
{
   
    public Image questionMarkImage;
    public Image fruitImage;

    public void Reveal()
    {
        if (questionMarkImage != null) Destroy(questionMarkImage.gameObject);

        if (fruitImage != null)
        {
            Color c = fruitImage.color;
            c.a = 1f;
            fruitImage.color = c;
        }
    }
}


