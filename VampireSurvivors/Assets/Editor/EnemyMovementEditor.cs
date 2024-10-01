using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CustomEditor(typeof(EnemyMovement), true)]
public class EnemyMovementEditor : Editor
{
    private EnemyMovement enemyMovement;
    private SerializedObject serializedEnemyMovement;
    [SerializeField]
    private SerializedProperty commonMovementSettings;
    [SerializeField]
    private SerializedProperty rangeMovementSettings;
    [SerializeField]
    private SerializedProperty moveTypeProperty;
    [SerializeField]
    private SerializedProperty followTypeProperty;
    [SerializeField]
    private SerializedProperty actionTypeProperty;
    

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        commonMovementSettings = serializedObject.FindProperty("commonMovementSettings");

        moveTypeProperty = commonMovementSettings.FindPropertyRelative("_moveType");
        if (commonMovementSettings != null)
        {
            EditorGUILayout.PropertyField(commonMovementSettings, new GUIContent("Common Movement Settings"), true);
            if (moveTypeProperty != null)
            {
                DrawMovementSettingsByType((EMovementType)moveTypeProperty.enumValueIndex);
            }
            else
            {
                Debug.LogError("moveTypeProperty not found!");
            }

            followTypeProperty = commonMovementSettings.FindPropertyRelative("_followType");
            if (followTypeProperty != null)
            {
                DrawMovementSettingsByType((EMovementType)followTypeProperty.enumValueIndex);
            }
            else
            {
                Debug.LogError("followTypeProperty not found!");
            }

            actionTypeProperty = commonMovementSettings.FindPropertyRelative("_actionType");
            if (actionTypeProperty != null)
            {
                DrawActionSettingsByType((EActionType)actionTypeProperty.enumValueIndex);
            }
            else
            {
                Debug.LogError("actionTypeProperty not found!");
            }
        }
        else
        {
            Debug.LogError("commonMovementSettings not found!");
        }

        rangeMovementSettings = serializedObject.FindProperty("rangeMovementSettings");
        if (rangeMovementSettings != null)
        {
            EditorGUILayout.PropertyField(rangeMovementSettings, new GUIContent("Range Movement Settings"), true);
        }
        else
        {
            Debug.LogError("rangeMovementSettings not found!");
        }

        serializedObject.ApplyModifiedProperties();
        //if (GUI.changed)
        //{
        //    EditorUtility.SetDirty(enemyMovement);
        //}

        //DrawDefaultInspector();
    }

    private void DrawMovementSettingsByType(EMovementType _type)
    {
        switch (_type)
        {
            case EMovementType.SPIRAL:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("spiralMovementSettings"), new GUIContent("Spiral Movement Settings"), true);
                break;
            case EMovementType.WAVE:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("waveMovementSettings"), new GUIContent("Wave Movement Settings"), true);
                break;
            case EMovementType.CIRCLE:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("circleMovementSettings"), new GUIContent("Circle Movement Settings"), true);
                break;
            case EMovementType.STRAIGHT:
                break;
        }
    }

    private void DrawActionSettingsByType(EActionType _type)
    {
        switch (_type)
        {
            case EActionType.DASH:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("dashActionSettings"), new GUIContent("Dash Action Settings"), true);
                break;
            case EActionType.SURROUND:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("circleMovementSettings"), new GUIContent("Circle Action Settings"), true);
                break;
        }
    }
}
