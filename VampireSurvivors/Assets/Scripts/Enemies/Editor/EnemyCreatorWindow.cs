using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.Animations;
using System.Configuration;
using System.Reflection;
using UnityEditor.VersionControl;
using static UnityEngine.GridBrushBase;

public class EnemyCreatorWindow : EditorWindow
{
    private SerializedObject _serializedContainer;
    private EnemyDataContainer enemyDataContainer;
    private SerializedObject _serializedData;
    private SerializedObject _serializedMovementData;
    private SerializedObject _serializedMovementSettings;
    private SerializedObject _serializedChasingSettings;
    private SerializedObject _serializedActionSettings;
    private SerializedObject _serializedRangeSettings;
    private SerializedObject _serializedDodgeSettings;

    private EMovementType _previousMoveType;
    private EMovementType _previousFollowType;
    private EActionType _previousActionType;

    ScriptableObject previousMovementSettingsSO;
    ScriptableObject previousChasingSettingsSO;
    ScriptableObject previousActionSettingsSO;

    private Dictionary<ScriptableObject, string> scriptableObjectPaths = new Dictionary<ScriptableObject, string>();
    string assetPath = "Assets/Prefab/Enemies/Data/";
    private Vector2 _scrollPosition;

    [MenuItem("Tools/Enemy Creator")]
    public static void ShowWindow()
    {
        GetWindow<EnemyCreatorWindow>("Enemy Creator");
    }
    void OnEnable()
    {
        CreateInstances();
    }

    void OnGUI()
    {
        _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
        {
            EditorGUI.BeginChangeCheck();

            if (_serializedContainer == null || _serializedContainer.targetObject == null)
            {
                EditorGUILayout.HelpBox("SerializedObject has been destroyed or the asset does not exist. Please create a new one.", MessageType.Warning);
                if (GUILayout.Button("Reload Editor Window"))
                {
                    ReloadSerializedObject();
                }
                GUILayout.EndScrollView();
                return;
            }

            _serializedContainer.Update();

            GUILayout.Label("Create a New Enemy", EditorStyles.boldLabel);

            if (EditorGUI.EndChangeCheck())
            {
                _serializedContainer.ApplyModifiedProperties();
                AssetDatabase.SaveAssets();
            }

            EditorGUILayout.LabelField("Enemy Data", EditorStyles.boldLabel);
            SerializedProperty enemyDataProperty = _serializedContainer.FindProperty("enemyData");
            EditorGUILayout.PropertyField(enemyDataProperty, true);
            DisplaySerializedObject(_serializedData);

            EditorGUILayout.LabelField("Enemy Movement Data", EditorStyles.boldLabel);
            SerializedProperty enemyMovementDataProperty = _serializedContainer.FindProperty("enemyMovementData");
            EditorGUILayout.PropertyField(enemyMovementDataProperty, true);
            DisplaySerializedObject(_serializedMovementData);

            if (_serializedMovementData != null)
            {
                //SerializedProperty movementSettings = _serializedMovementData.FindProperty("movementSettings");
                //EditorGUILayout.PropertyField(movementSettings, true);

                if (enemyDataContainer.enemyMovementData.movementSettings != null)
                {
                    if (enemyDataContainer.enemyMovementData.moveType != _previousMoveType)
                    {
                        switch (enemyDataContainer.enemyMovementData.moveType)
                        {
                            case EMovementType.SPIRAL:
                                if (enemyDataContainer.enemyMovementData.movementSettings is not SpiralMovementSettingsData)
                                {
                                    ScriptableObject newMovementSettings = ScriptableObject.CreateInstance<SpiralMovementSettingsData>();
                                    CreateOrUpdateAsset(newMovementSettings, assetPath + enemyDataContainer.enemyData.enemyType + "MovementSettingsData.asset");
                                    enemyDataContainer.enemyMovementData.movementSettings = newMovementSettings;
                                }
                                break;
                            case EMovementType.WAVE:
                                if (enemyDataContainer.enemyMovementData.movementSettings is not WaveMovementSettingsData)
                                {
                                    ScriptableObject newMovementSettings = ScriptableObject.CreateInstance<WaveMovementSettingsData>();
                                    CreateOrUpdateAsset(newMovementSettings, assetPath + enemyDataContainer.enemyData.enemyType + "MovementSettingsData.asset");
                                    enemyDataContainer.enemyMovementData.movementSettings = newMovementSettings;
                                }
                                break;
                            case EMovementType.CIRCLE:
                                if (enemyDataContainer.enemyMovementData.movementSettings is not CircleMovementSettingsData)
                                {
                                    ScriptableObject newMovementSettings = ScriptableObject.CreateInstance<CircleMovementSettingsData>();
                                    CreateOrUpdateAsset(newMovementSettings, assetPath + enemyDataContainer.enemyData.enemyType + "MovementSettingsData.asset");
                                    enemyDataContainer.enemyMovementData.movementSettings = newMovementSettings;
                                }
                                break;
                            case EMovementType.STRAIGHT:
                                if (enemyDataContainer.enemyMovementData.movementSettings is not StraightMovementSettingsData)
                                {
                                    ScriptableObject newMovementSettings = ScriptableObject.CreateInstance<StraightMovementSettingsData>();
                                    CreateOrUpdateAsset(newMovementSettings, assetPath + enemyDataContainer.enemyData.enemyType + "MovementSettingsData.asset");
                                    enemyDataContainer.enemyMovementData.movementSettings = newMovementSettings;
                                }
                                break;
                            default:
                                EditorGUILayout.HelpBox("Select a movement type to view settings", MessageType.Warning);
                                break;
                        }

                        scriptableObjectPaths.Add(enemyDataContainer.enemyMovementData.movementSettings, assetPath + enemyDataContainer.enemyData.enemyType + "MovementSettingsData.asset");
                        if (previousMovementSettingsSO != null)
                        {
                            DeleteAssetOnPath(previousMovementSettingsSO);
                        }
                        previousMovementSettingsSO = enemyDataContainer.enemyMovementData.movementSettings;
                        _serializedMovementSettings = new SerializedObject(enemyDataContainer.enemyMovementData.movementSettings);
                        _previousMoveType = enemyDataContainer.enemyMovementData.moveType;
                        AssetDatabase.SaveAssets();
                    }

                    //DisplaySerializedObject(_serializedMovementSettings);

                }
                else
                {
                    EditorGUILayout.HelpBox("Movement settings are missing", MessageType.Warning);
                }

                //SerializedProperty chasingSettings = _serializedMovementData.FindProperty("chasingSettings");

                if (enemyDataContainer.enemyMovementData.chasingSettings != null /*&& chasingSettings.objectReferenceValue != null*/)
                {
                    //EditorGUILayout.PropertyField(chasingSettings, true);
                    if (enemyDataContainer.enemyMovementData.followType != _previousFollowType)
                    {
                        switch (enemyDataContainer.enemyMovementData.followType)
                        {
                            case EMovementType.SPIRAL:
                                if (enemyDataContainer.enemyMovementData.chasingSettings is not SpiralChasingSettingsData)
                                {
                                    ScriptableObject newChasingSettings = ScriptableObject.CreateInstance<SpiralChasingSettingsData>();
                                    CreateOrUpdateAsset(newChasingSettings, assetPath + enemyDataContainer.enemyData.enemyType + "ChasingSettingsData.asset");
                                    enemyDataContainer.enemyMovementData.chasingSettings = newChasingSettings;
                                }
                                break;
                            case EMovementType.WAVE:
                                if (enemyDataContainer.enemyMovementData.chasingSettings is not WaveChasingSettingsData)
                                {
                                    ScriptableObject newChasingSettings = ScriptableObject.CreateInstance<WaveChasingSettingsData>();
                                    CreateOrUpdateAsset(newChasingSettings, assetPath + enemyDataContainer.enemyData.enemyType + "ChasingSettingsData.asset");
                                    enemyDataContainer.enemyMovementData.chasingSettings = newChasingSettings;
                                }
                                break;
                            case EMovementType.CIRCLE:
                                if (enemyDataContainer.enemyMovementData.chasingSettings is not CircleChasingSettingsData)
                                {
                                    ScriptableObject newChasingSettings = ScriptableObject.CreateInstance<CircleChasingSettingsData>();
                                    CreateOrUpdateAsset(newChasingSettings, assetPath + enemyDataContainer.enemyData.enemyType + "ChasingSettingsData.asset");
                                    enemyDataContainer.enemyMovementData.chasingSettings = newChasingSettings;
                                }
                                break;
                            case EMovementType.STRAIGHT:
                                if (enemyDataContainer.enemyMovementData.chasingSettings is not StraightChasingSettingsData)
                                {
                                    ScriptableObject newChasingSettings = ScriptableObject.CreateInstance<StraightChasingSettingsData>();
                                    CreateOrUpdateAsset(newChasingSettings, assetPath + enemyDataContainer.enemyData.enemyType + "ChasingSettingsData.asset");
                                    enemyDataContainer.enemyMovementData.chasingSettings = newChasingSettings;
                                }
                                break;
                            default:
                                EditorGUILayout.HelpBox("Select a follow type to view settings", MessageType.Warning);
                                break;
                        }
                        scriptableObjectPaths.Add(enemyDataContainer.enemyMovementData.chasingSettings, assetPath + enemyDataContainer.enemyData.enemyType + "ChasingSettingsData.asset");
                        if (previousChasingSettingsSO != null)
                        {
                            DeleteAssetOnPath(previousChasingSettingsSO);
                        }
                        previousChasingSettingsSO = enemyDataContainer.enemyMovementData.chasingSettings;
                        _serializedChasingSettings = new SerializedObject(enemyDataContainer.enemyMovementData.chasingSettings);
                        _previousFollowType = enemyDataContainer.enemyMovementData.followType;
                        AssetDatabase.SaveAssets();
                    }

                    //DisplaySerializedObject(_serializedChasingSettings);
                }
                else
                {
                    EditorGUILayout.HelpBox("Chasing settings are missing", MessageType.Warning);
                }

                //SerializedProperty actionSettings = _serializedMovementData.FindProperty("actionSettings");

                if (enemyDataContainer.enemyMovementData.actionSettings != null /*&& actionSettings.objectReferenceValue != null*/)
                {
                    if (enemyDataContainer.enemyMovementData.actionType != _previousActionType)
                    {
                        switch (enemyDataContainer.enemyMovementData.actionType)
                        {
                            case EActionType.DASH:
                                if (enemyDataContainer.enemyMovementData.actionSettings is not DashActionSettingsData)
                                {
                                    ScriptableObject newActionSettings = ScriptableObject.CreateInstance<DashActionSettingsData>();
                                    CreateOrUpdateAsset(newActionSettings, assetPath + enemyDataContainer.enemyData.enemyType + "ActionSettingsData.asset");
                                    enemyDataContainer.enemyMovementData.actionSettings = newActionSettings;
                                }
                                break;
                            case EActionType.SURROUND:
                                if (enemyDataContainer.enemyMovementData.actionSettings is not CircleActionSettingsData)
                                {
                                    ScriptableObject newActionSettings = ScriptableObject.CreateInstance<CircleActionSettingsData>();
                                    CreateOrUpdateAsset(newActionSettings, assetPath + enemyDataContainer.enemyData.enemyType + "ActionSettingsData.asset");
                                    enemyDataContainer.enemyMovementData.actionSettings = newActionSettings;

                                    //DeleteAssetOnPath(previousActionSettingsSO);
                                    //enemyDataContainer.enemyMovementData.actionSettings = ScriptableObject.CreateInstance<CircleActionSettingsData>();
                                    //CreateOrUpdateAsset(enemyDataContainer.enemyMovementData.actionSettings, assetPath + enemyDataContainer.enemyData.enemyType + "ActionSettingsData.asset");
                                }
                                break;
                            default:
                                EditorGUILayout.HelpBox("Select a action type to view settings", MessageType.Warning);
                                break;
                        }
                        scriptableObjectPaths.Add(enemyDataContainer.enemyMovementData.actionSettings, assetPath + enemyDataContainer.enemyData.enemyType + "ActionSettingsData.asset");
                        if (previousActionSettingsSO != null)
                        {
                            DeleteAssetOnPath(previousActionSettingsSO);
                        }
                        previousActionSettingsSO = enemyDataContainer.enemyMovementData.actionSettings;
                        _serializedActionSettings = new SerializedObject(enemyDataContainer.enemyMovementData.actionSettings);
                        _previousActionType = enemyDataContainer.enemyMovementData.actionType;
                        AssetDatabase.SaveAssets();
                    }

                    //DisplaySerializedObject(_serializedActionSettings);
                }
                else
                {
                    EditorGUILayout.HelpBox("Action settings are missing", MessageType.Warning);
                }

            }
            else
            {
                EditorGUILayout.HelpBox("Enemy Movement Data is null", MessageType.Warning);
            }

            foreach (var p in scriptableObjectPaths)
            {
                var key = p.Key;
                var value = p.Value;

                Debug.Log($"ScriptableObject: {key}, Path: {value}");
            }

            EditorGUILayout.LabelField("Movement Settings", EditorStyles.boldLabel);
            DisplaySerializedObject(_serializedMovementSettings);

            EditorGUILayout.LabelField("Chasing Settings", EditorStyles.boldLabel);
            DisplaySerializedObject(_serializedChasingSettings);

            EditorGUILayout.LabelField("Action Settings", EditorStyles.boldLabel);
            DisplaySerializedObject(_serializedActionSettings);

            EditorGUILayout.LabelField("Range Settings", EditorStyles.boldLabel);
            DisplaySerializedObject(_serializedRangeSettings);


            EditorGUILayout.LabelField("Enemy Dodge Data", EditorStyles.boldLabel);
            SerializedProperty enemyDodgeDataProperty = _serializedContainer.FindProperty("enemyDodgeData");
            EditorGUILayout.PropertyField(enemyDodgeDataProperty, true);

            DisplaySerializedObject(_serializedDodgeSettings);


            if (GUILayout.Button("Save Enemy Data Container"))
            {
                SaveContainer();
            }

            if (GUILayout.Button("Create Enemy"))
            {
                CreateEnemy();
            }

        }
        GUILayout.EndScrollView();

        //AssetDatabase.SaveAssets();
        //AssetDatabase.Refresh();
    }

    private void DisplaySerializedObject(SerializedObject serializedObject)
    {
        serializedObject.Update();

        FieldInfo[] fields = serializedObject.targetObject.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        //EditorGUILayout.HelpBox("fields" + fields.Length, MessageType.Warning);
        foreach (FieldInfo field in fields)
        {
            var prop = serializedObject.FindProperty(field.Name);
            if (prop != null)
            {
                EditorGUILayout.PropertyField(prop, new GUIContent(ObjectNames.NicifyVariableName(field.Name)));

                if (field.Name == "enemyType" && string.IsNullOrEmpty(prop.stringValue))
                {
                    EditorGUILayout.HelpBox("Enemy Type cannot be empty. Please fill it in.", MessageType.Warning);
                }
            }
        }

        serializedObject.ApplyModifiedProperties();
        AssetDatabase.SaveAssets();
        //AssetDatabase.Refresh();
    }

    private void SaveContainer()
    {
        CreateOrUpdateAsset(enemyDataContainer.enemyMovementData.rangeMovementSettingsData, assetPath + enemyDataContainer.enemyData.enemyType + "RangeMovementSettingsData.asset");
        CreateOrUpdateAsset(enemyDataContainer.enemyMovementData.actionSettings, assetPath + enemyDataContainer.enemyData.enemyType + "ActionSettingsData.asset");
        CreateOrUpdateAsset(enemyDataContainer.enemyMovementData.chasingSettings, assetPath + enemyDataContainer.enemyData.enemyType + "ChasingSettingsData.asset");
        CreateOrUpdateAsset(enemyDataContainer.enemyMovementData.movementSettings, assetPath + enemyDataContainer.enemyData.enemyType + "MovementSettingsData.asset");
        CreateOrUpdateAsset(enemyDataContainer.enemyMovementData, assetPath + enemyDataContainer.enemyData.enemyType + "EnemyMovementData.asset");
        CreateOrUpdateAsset(enemyDataContainer.enemyDodgeData, assetPath + enemyDataContainer.enemyData.enemyType + "EnemyDodgeData.asset");
        CreateOrUpdateAsset(enemyDataContainer.enemyData, assetPath + enemyDataContainer.enemyData.enemyType + "EnemyData.asset");
        CreateOrUpdateAsset(enemyDataContainer, assetPath + enemyDataContainer.enemyData.enemyType + "EnemyDataContainer.asset");

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    void CreateOrUpdateAsset(ScriptableObject asset, string path)
    {
        if (scriptableObjectPaths.ContainsKey(asset))
        {
            if (scriptableObjectPaths[asset] != path)
            {
                AssetDatabase.CreateAsset(asset, path);
            }
        }
        var existingAsset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);

        if (existingAsset != null && existingAsset.GetType() != asset.GetType())
        {
            AssetDatabase.DeleteAsset(path);
            existingAsset = null;
        }

        if (existingAsset == null)
        {
            AssetDatabase.CreateAsset(asset, path);
        }
        else
        {
            EditorUtility.CopySerialized(asset, existingAsset);
            EditorUtility.SetDirty(existingAsset);
        }

        AssetDatabase.SaveAssets();
    }

    void DeleteAssetOnPath(ScriptableObject obj)
    {
        if (obj != null && AssetDatabase.GetAssetPath(obj) != null)
        {
            if(scriptableObjectPaths.ContainsKey(obj))
            {
                AssetDatabase.DeleteAsset(scriptableObjectPaths[obj]);
                scriptableObjectPaths.Remove(obj);
            }
        }
    }

    void CreateEnemy()
    {
        if (!string.IsNullOrEmpty(enemyDataContainer.enemyData.enemyType))
        {
            GameObject enemy = new(enemyDataContainer.enemyData.enemyType);
            enemy.tag = "Enemy";
            enemy.layer = LayerMask.NameToLayer("Enemy");
            Enemy enemyComponent = enemy.AddComponent<Enemy>();
            //GameObject enemyObject = new GameObject(enemyTypeString);
            //Enemy enemy = ScriptableObject.CreateInstance<Enemy>();
            enemyComponent.Initialize(enemyDataContainer.enemyData);


            SpriteRenderer spriteRenderer = enemy.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = enemyDataContainer.enemyData.enemySprite;

            Animator animator = enemy.AddComponent<Animator>();
            animator.runtimeAnimatorController = enemyDataContainer.enemyData.enemyAnimatorController;

            HealthComponent healthComponent = enemy.AddComponent<HealthComponent>();
            healthComponent.MaxHealth = enemyDataContainer.enemyData.maxHealth;

            EnemyMovement enemyMovement = enemy.AddComponent<EnemyMovement>();
            enemyMovement.CommonMovementSettings.Initialize(
                enemyDataContainer.enemyMovementData.moveSpeed,
                enemyDataContainer.enemyMovementData.speedMultiplier,
                enemyDataContainer.enemyMovementData.accelerationTime,
                enemyDataContainer.enemyMovementData.moveType,
                enemyDataContainer.enemyMovementData.followType,
                enemyDataContainer.enemyMovementData.actionType,
                animator);

            enemyMovement.RangeMovementSettings.Initialize(
                enemyDataContainer.enemyMovementData.rangeMovementSettingsData._minDistance,
                enemyDataContainer.enemyMovementData.rangeMovementSettingsData._detectionRange,
                enemyDataContainer.enemyMovementData.rangeMovementSettingsData._exclusionRange,
                enemyDataContainer.enemyMovementData.rangeMovementSettingsData._minTimeBeforeExclusion);

            enemyMovement.EnemyDataContainer = enemyDataContainer;

            EnemyDodge enemyDodge = enemy.AddComponent<EnemyDodge>();
            enemyDodge.Initialize(
                enemyDataContainer.enemyDodgeData.dodgeSpeed,
                enemyDataContainer.enemyDodgeData.dodgeDistance,
                enemyDataContainer.enemyDodgeData.dodgeDuration,
                enemyDataContainer.enemyDodgeData.detectionRadius,
                enemyMovement,
                enemyDataContainer.enemyDodgeData.projectileLayer);

            Rigidbody2D rigidbody2D = enemy.AddComponent<Rigidbody2D>();
            rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            rigidbody2D.simulated = true;
            rigidbody2D.mass = 0.75f;
            rigidbody2D.angularDrag = 0f;
            rigidbody2D.gravityScale = 0f;
            rigidbody2D.freezeRotation = true;

            BoxCollider2D boxCollider2D = enemy.AddComponent<BoxCollider2D>();
            boxCollider2D.isTrigger = false;

            // Save as prefab in the project
            string localPath = "Assets/Prefab/Enemies/Prefabs/" + enemyDataContainer.enemyData.enemyType + ".prefab";
            localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);
            var enemyPrefab = PrefabUtility.SaveAsPrefabAsset(enemy, localPath);
            EnemyManager enemyManager = FindObjectOfType<EnemyManager>();

            if (enemyManager != null)
            {
                enemyManager.AddEnemyToDictionary(enemyDataContainer.enemyData.enemyType, enemyPrefab.GetComponent<Enemy>());
                Debug.Log($"Enemy {enemyDataContainer.enemyData.enemyType} added to EnemyManager.");
            }
            else
            {
                Debug.LogError("EnemyManager not found in scene.");
            }

            //// Save the asset
            //AssetDatabase.CreateAsset(enemy, localPath);
            //AssetDatabase.SaveAssets();
            //EditorUtility.FocusProjectWindow();
            //Selection.activeObject = enemy;

            // Clean up the temporary GameObject
            Destroy(enemy);

            Debug.Log("Enemy created and saved as prefab at " + localPath);
        }
        else
        {
            EditorGUILayout.HelpBox("Enter enemy name", MessageType.Warning);
        }

    }

    void ReloadSerializedObject()
    {
        CreateInstances();
        Repaint();
    }

    void CreateInstances()
    {
        if (enemyDataContainer == null)
        {
            enemyDataContainer = ScriptableObject.CreateInstance<EnemyDataContainer>();
        }
        _serializedContainer = new SerializedObject(enemyDataContainer);

        if (enemyDataContainer.enemyData == null)
        {
            enemyDataContainer.enemyData = ScriptableObject.CreateInstance<EnemyData>();
        }
        _serializedData = new SerializedObject(enemyDataContainer.enemyData);

        if (enemyDataContainer.enemyMovementData == null)
        {
            enemyDataContainer.enemyMovementData = ScriptableObject.CreateInstance<EnemyMovementData>();
        }
        _serializedMovementData = new SerializedObject(enemyDataContainer.enemyMovementData);

        if (enemyDataContainer.enemyMovementData.movementSettings == null)
        {
            enemyDataContainer.enemyMovementData.movementSettings = ScriptableObject.CreateInstance<SpiralMovementSettingsData>();
            //previousMovementSettingsSO = enemyDataContainer.enemyMovementData.movementSettings;
        }
        _serializedMovementSettings = new SerializedObject(enemyDataContainer.enemyMovementData.movementSettings);

        if (enemyDataContainer.enemyMovementData.chasingSettings == null)
        {
            enemyDataContainer.enemyMovementData.chasingSettings = ScriptableObject.CreateInstance<SpiralChasingSettingsData>();
            //previousChasingSettingsSO = enemyDataContainer.enemyMovementData.chasingSettings;
        }
        _serializedChasingSettings = new SerializedObject(enemyDataContainer.enemyMovementData.chasingSettings);

        if (enemyDataContainer.enemyMovementData.actionSettings == null)
        {
            enemyDataContainer.enemyMovementData.actionSettings = ScriptableObject.CreateInstance<DashActionSettingsData>();
            //previousActionSettingsSO = enemyDataContainer.enemyMovementData.actionSettings;
        }
        _serializedActionSettings = new SerializedObject(enemyDataContainer.enemyMovementData.actionSettings);

        if (enemyDataContainer.enemyMovementData.rangeMovementSettingsData == null)
        {
            enemyDataContainer.enemyMovementData.rangeMovementSettingsData = ScriptableObject.CreateInstance<RangeMovementSettingsData>();
        }
        _serializedRangeSettings = new SerializedObject(enemyDataContainer.enemyMovementData.rangeMovementSettingsData);

        if (enemyDataContainer.enemyDodgeData == null)
        {
            enemyDataContainer.enemyDodgeData = ScriptableObject.CreateInstance<EnemyDodgeData>();
        }
        _serializedDodgeSettings = new SerializedObject(enemyDataContainer.enemyDodgeData);
    }
}
