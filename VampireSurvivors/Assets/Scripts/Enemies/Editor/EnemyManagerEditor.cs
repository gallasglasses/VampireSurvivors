using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CustomEditor(typeof(EnemyManager))]
public class EnemyManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("Enemies Dictionary", EditorStyles.boldLabel);
        var enemiesDict = ((EnemyManager)target).enemiesDict;
        DisplayDict<string, Enemy>(enemiesDict);

        base.OnInspectorGUI();
    }

    private void DisplayDict<TKey, TValue>(Dictionary<TKey, TValue> dict)
    {
        foreach (var kvp in dict)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(kvp.Key.ToString(), GUILayout.Width(100));
            EditorGUILayout.LabelField(kvp.Value.ToString(), GUILayout.Width(100));
            EditorGUILayout.EndHorizontal();
        }
    }
}
