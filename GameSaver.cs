using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// Класс, содержащий методы сохранения
/// и загрузки игры
/// </summary>
public static class GameSaver
{
    private static string Path
    {
        get
        {
            return System.IO.Path.Combine(Application.dataPath, "save.json");
        }
    }

    // Метод сохранения
    public static void Save(GameState state)
    {
        var json = JsonConvert.SerializeObject(state);
        File.WriteAllText(Path, json);
    }

    // Метод загрузки
    public static GameState Load()
    {
        if(File.Exists(Path))
        {
            string json = File.ReadAllText(Path);
            GameState state = JsonConvert.DeserializeObject<GameState>(json);
            return state;
        }

        return null;
    }
}
