using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Fruit/Fruit Data")]
public class FruitData : ScriptableObject
{
    public string fruitName;
    public Sprite sprite;
    public int scoreValue;
}

