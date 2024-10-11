using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ProjectileWeapon))]
public class ProjectileWeaponEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //EditorGUILayout.LabelField("Projectile Settings Dictionary", EditorStyles.boldLabel);
        //var projectileSettingsDict = ((ProjectileWeapon)target).projectileSettings;
        //DisplayDict(projectileSettingsDict);
        //EditorGUILayout.LabelField("Projectile Dictionary", EditorStyles.boldLabel);
        //var projectilesDict = ((ProjectileWeapon)target).projectiles;
        //DisplayDict(projectilesDict);
        //EditorGUILayout.LabelField("Cooldown Timers Dictionary", EditorStyles.boldLabel);
        //var cooldownTimersDict = ((ProjectileWeapon)target).cooldownTimers;
        //DisplayDict(cooldownTimersDict);

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
