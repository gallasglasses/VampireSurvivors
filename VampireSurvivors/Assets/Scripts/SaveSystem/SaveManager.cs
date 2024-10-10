using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    private string savePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            savePath = Path.Combine(Application.persistentDataPath, "playerStats.json");
            Debug.Log($"savePath {savePath}");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveStats(PlayerStats stats)
    {
        string json = JsonUtility.ToJson(stats);
        File.WriteAllText(savePath, json);
    }

    public PlayerStats LoadStats()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            return JsonUtility.FromJson<PlayerStats>(json);
        }
        //if (File.Exists(savePath))
        //{
        //    string json = File.ReadAllText(savePath);

        //    var maxLevelMatch = Regex.Match(json, "\"maxLevel\":\\s*(\\d+)");
        //    var maxEnemiesKilledMatch = Regex.Match(json, "\"maxEnemiesKilled\":\\s*(\\d+)");

        //    PlayerStats stats = new PlayerStats();
        //    if (maxLevelMatch.Success)
        //    {
        //        Debug.Log($"maxLevel {int.Parse(maxLevelMatch.Groups[1].Value)}");
        //        stats.maxLevel = int.Parse(maxLevelMatch.Groups[1].Value);
        //    }
        //    if (maxEnemiesKilledMatch.Success)
        //    {
        //        Debug.Log($"maxEnemiesKilled {int.Parse(maxEnemiesKilledMatch.Groups[1].Value)}");
        //        stats.maxEnemiesKilled = int.Parse(maxEnemiesKilledMatch.Groups[1].Value);
        //    }

        //    return stats;
        //}
        else
        {
            return new PlayerStats();
        }
    }
}
