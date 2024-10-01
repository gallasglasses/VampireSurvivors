using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor.Animations;

public class EnemyDataLoader : MonoBehaviour
{
    [SerializeField] private MyDictionary<EEnemyType, EnemyData> enemyData;
    private Dictionary<EEnemyType, EnemyData> enemyDataDict = new Dictionary<EEnemyType, EnemyData>();
    //public string csvFilePath = "Assets/Resources/FRPGItemData.csv";

    public class EnemyData
    {
        public Sprite sprite;
        public AnimatorController animatorController;

        public EnemyData(Sprite newSprite, AnimatorController newAnimatorController)
        {
            this.sprite = newSprite;
            this.animatorController = newAnimatorController;
        }
    }

    //[ContextMenu("Load Enemy Data From CSV")]
    //public void LoadEnemyDataFromCSV()
    //{
    //    if (enemyDataDict != null)
    //    {
    //        enemyDataDict.Clear();
    //    }

    //    try
    //    {
    //        using (StreamReader file = new StreamReader(csvFilePath))
    //        {
    //            string[] lines = File.ReadAllLines(csvFilePath);

    //            for (int i = 1; i < lines.Length; i++)
    //            {
    //                string[] values = lines[i].Split(',');

    //                string sprite = values[1];
    //                string animatorControllerPath = values[2];
    //                string ID = values[0];
    //                bool successID = Enum.TryParse<EEnemyType>(ID, out EEnemyType resultID);
    //                if (successID)
    //                {
    //                    enemyDataDict[resultID] = new EnemyData(sprite, animatorControllerPath);
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception e)
    //    {
    //        Debug.LogError($"Exception occurred: {e.Message}");
    //        Console.WriteLine("The file could not be read:");
    //        Console.WriteLine(e.Message);
    //    }

    //}

    public EnemyData GetEnemyData(EEnemyType enemyType)
    {
        if (enemyDataDict.TryGetValue(enemyType, out EnemyData data))
        {
            return data;
        }

        return null;
    }
}
