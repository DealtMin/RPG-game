using UnityEngine;
using System.IO; 

public class GameRepository : ISaveService
{
    
    private readonly string _filePath = Path.Combine(Application.persistentDataPath, "save.json");

    public void Save(PlayerData data)
    {
        string json = JsonUtility.ToJson(data, true); 
        File.WriteAllText(_filePath, json);
        Debug.Log($"[Repository] Данные сохранены в файл: {_filePath}");
    }

    public PlayerData LoadGame()
    {
        if (!File.Exists(_filePath))
        {
            Debug.LogWarning("[Repository] Файл сохранения не найден.");
            return null;
        }

        string json = File.ReadAllText(_filePath);
        PlayerData data = JsonUtility.FromJson<PlayerData>(json);
        return data; // Возвращаем загруженный объект
    }
}