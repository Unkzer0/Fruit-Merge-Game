using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSaveData
{
    public int score;
    public int highScore;

    public int currentFruitIndex;
    public int nextFruitIndex;

    public Vector3 dropperPosition;

    public bool isGameOver;

    public List<FruitSaveData> fruits = new List<FruitSaveData>();
}
