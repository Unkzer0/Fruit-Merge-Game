using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameSaveManager : MonoBehaviour
{
    public static GameSaveManager instance;
    private string saveFilePath;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            saveFilePath = Application.persistentDataPath + "/savegame.json";
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveGame(GameSaveData data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Game saved to: " + saveFilePath);
    }

    public void RestoreGame()
    {
        if (!File.Exists(saveFilePath)) return;

        string json = File.ReadAllText(saveFilePath);
        GameSaveData data = JsonUtility.FromJson<GameSaveData>(json);

        // Restore score
        ScoreManager.instance?.ResetScore();
        for (int i = 0; i < data.score; i++)
            ScoreManager.instance?.AddScore(1);

        // Restore fruits
        foreach (var fruitData in data.fruits)
        {
            GameObject prefab = FruitSelector.instance.Fruits[fruitData.fruitIndex];
            GameObject fruit = Object.Instantiate(prefab, fruitData.position, Quaternion.identity);
            Fruit fruitScript = fruit.GetComponent<Fruit>();
            if (fruitScript != null) fruitScript.fruitIndex = fruitData.fruitIndex;
        }

        // Restore dropper position
        FruitDropperController dropper = Object.FindObjectOfType<FruitDropperController>();
        if (dropper != null)
            dropper.transform.position = data.dropperPosition;

        // Restore current/next fruit
        FruitSelector selector = FruitSelector.instance;
        if (selector != null)
        {
            selector.SetCurrentFruit(data.currentFruitIndex);
            selector.SetNextFruit(data.nextFruitIndex);
            selector.UpdateFruitUI();
        }

        // Restore game over
        if (data.isGameOver)
        {
            PanelManager.instance.ShowGameOver();
            PanelManager.instance.gameOverPanelScript?.ShowScore();
        }
    }

    public void DeleteSave()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            Debug.Log("Save file deleted.");
        }
    }

    public bool HasSavedGame()
    {
        return File.Exists(saveFilePath);
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SaveCurrentGameState();
        }
    }

    private void OnApplicationQuit()
    {
        SaveCurrentGameState();
    }

    private void SaveCurrentGameState()
    {
        if (PanelManager.AnyPanelOrJustClosed) return; // Optional: skip saving if in UI

        GameSaveData saveData = new GameSaveData();

        // 1. Save score
        saveData.score = ScoreManager.instance?.GetCurrentScore() ?? 0;
        saveData.highScore = ScoreManager.instance?.GetHighScore() ?? 0;

        // 2. Save fruits in scene
        GameObject[] fruits = GameObject.FindGameObjectsWithTag("Fruit");
        foreach (var fruit in fruits)
        {
            Fruit f = fruit.GetComponent<Fruit>();
            if (f != null)
            {
                FruitSaveData data = new FruitSaveData
                {
                    fruitIndex = f.fruitIndex,
                    position = fruit.transform.position
                };
                saveData.fruits.Add(data);
            }
        }

        // 3. Save current/next fruit
        saveData.currentFruitIndex = FruitSelector.instance?.CurrentFruitIndex ?? 0;
        saveData.nextFruitIndex = FruitSelector.instance?.NextFruitIndex ?? 0;

        // 4. Save dropper position
        var dropper = FindObjectOfType<FruitDropperController>();
        if (dropper != null)
        {
            saveData.dropperPosition = dropper.transform.position;
        }

        // 5. Save game state
        saveData.isGameOver = PanelManager.instance?.IsGameOverShown ?? false;

        // Save to file
        SaveGame(saveData);
    }
   
    }
