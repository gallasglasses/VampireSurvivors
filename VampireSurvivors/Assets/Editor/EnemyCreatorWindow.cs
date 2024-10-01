using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.Animations;

public class EnemyCreatorWindow : EditorWindow
{
    // Enemy
    private Enemy enemySettings;
    private string enemyTypeString = "Default Type";
    private Sprite enemySprite;
    private AnimatorController enemyAnimatorController;
    private float maxHealth;

    //EnemyMovement
    //CommonMovementSettings
    private CommonMovementSettings commonMovementSettings;
    //SpiralMovementSettings
    private SpiralMovementSettings spiralMovementSettings;
    //WaveMovementSettings
    private WaveMovementSettings waveMovementSettings;
    //CircleMovementSettings
    private CircleMovementSettings circleMovementSettings;
    //DashActionSettings
    private DashActionSettings dashActionSettings;
    //RangeMovementSettings
    private RangeMovementSettings rangeMovementSettings;
    //EnemyDodgeSettings
    private EnemyDodge enemyDodgeSettings;

    [MenuItem("Tools/Enemy Creator")]
    public static void ShowWindow()
    {
        GetWindow<EnemyCreatorWindow>("Enemy Creator");
    }
    void OnEnable()
    {
        enemySettings = new Enemy();
        commonMovementSettings = new CommonMovementSettings();
        spiralMovementSettings = new SpiralMovementSettings();
        waveMovementSettings = new WaveMovementSettings();
        circleMovementSettings = new CircleMovementSettings();
        dashActionSettings = new DashActionSettings();
        rangeMovementSettings = new RangeMovementSettings();
        enemyDodgeSettings = new EnemyDodge();
    }

    void OnGUI()
    {
        GUILayout.Label("Create a New Enemy", EditorStyles.boldLabel);

        GUILayout.Label("Enemy", EditorStyles.boldLabel);
        enemyTypeString = EditorGUILayout.TextField("Enemy Name/Type", enemyTypeString);
        enemySettings.TypeXPGem = (TypeXPGem)EditorGUILayout.EnumPopup("XP Gem Type", enemySettings.TypeXPGem);
        enemySettings.Damage = EditorGUILayout.FloatField("Damage", enemySettings.Damage);
        enemySettings.BloodEffect = (ParticleSystem)EditorGUILayout.ObjectField("Blood Effect", enemySettings.BloodEffect, typeof(ParticleSystem), false);

        GUILayout.Label("Enemy Visuals", EditorStyles.boldLabel);
        enemySprite = (Sprite)EditorGUILayout.ObjectField("Enemy Sprite", enemySprite, typeof(Sprite), false);
        enemyAnimatorController = (AnimatorController)EditorGUILayout.ObjectField("Animator Controller", enemyAnimatorController, typeof(AnimatorController), false);

        GUILayout.Label("Enemy Health", EditorStyles.boldLabel);
        maxHealth = EditorGUILayout.FloatField("Enemy Max Health", maxHealth);

        GUILayout.Label("Enemy Movement", EditorStyles.boldLabel);
        GUILayout.Label("Common Movement Settings", EditorStyles.boldLabel);
        commonMovementSettings.MoveSpeed = EditorGUILayout.FloatField("Move Speed", commonMovementSettings.MoveSpeed);
        commonMovementSettings.SpeedMultiplier = EditorGUILayout.FloatField("Speed Multiplier", commonMovementSettings.SpeedMultiplier);
        commonMovementSettings.AccelerationTime = EditorGUILayout.FloatField("Acceleration Time", commonMovementSettings.AccelerationTime);
        commonMovementSettings.MoveType = (EMovementType)EditorGUILayout.EnumPopup("Move Type", commonMovementSettings.MoveType);
        commonMovementSettings.FollowType = (EMovementType)EditorGUILayout.EnumPopup("Follow Type", commonMovementSettings.FollowType);
        commonMovementSettings.ActionType = (EActionType)EditorGUILayout.EnumPopup("Action Type", commonMovementSettings.ActionType);

        GUILayout.Label("Move Type Settings", EditorStyles.boldLabel);
        switch (commonMovementSettings.MoveType)
        {
            case EMovementType.SPIRAL:
                GUILayout.Label("Spiral Movement Settings", EditorStyles.boldLabel);
                spiralMovementSettings.SpiralTightnessRange = EditorGUILayout.Vector2Field("Spiral Tightness Range", spiralMovementSettings.SpiralTightnessRange);
                spiralMovementSettings.SpiralRadiusIncreaseTime = EditorGUILayout.FloatField("Spiral Radius Increase Time", spiralMovementSettings.SpiralRadiusIncreaseTime);
                break;
            case EMovementType.WAVE:
                GUILayout.Label("Wave Movement Settings", EditorStyles.boldLabel);
                waveMovementSettings.WaveAmplitudeRange = EditorGUILayout.Vector2Field("Wave Amplitude Range", waveMovementSettings.WaveAmplitudeRange);
                waveMovementSettings.WaveFrequency = EditorGUILayout.FloatField("Wave Frequency", waveMovementSettings.WaveFrequency);
                waveMovementSettings.AngleRange = EditorGUILayout.FloatField("Angle Range", waveMovementSettings.AngleRange);
                break;
            case EMovementType.CIRCLE:
                GUILayout.Label("Circle Movement Settings", EditorStyles.boldLabel);
                circleMovementSettings.CircleRadius = EditorGUILayout.FloatField("Circle Radius", circleMovementSettings.CircleRadius);
                break;
            case EMovementType.STRAIGHT:
                break;
            default: break;
        }

        GUILayout.Label("Follow Type Settings", EditorStyles.boldLabel);
        switch (commonMovementSettings.FollowType)
        {
            case EMovementType.SPIRAL:
                GUILayout.Label("Spiral Movement Settings", EditorStyles.boldLabel);
                spiralMovementSettings.SpiralTightnessRange = EditorGUILayout.Vector2Field("Spiral Tightness Range", spiralMovementSettings.SpiralTightnessRange);
                spiralMovementSettings.SpiralRadiusIncreaseTime = EditorGUILayout.FloatField("Spiral Radius Increase Time", spiralMovementSettings.SpiralRadiusIncreaseTime);
                break;
            case EMovementType.WAVE:
                GUILayout.Label("Wave Movement Settings", EditorStyles.boldLabel);
                waveMovementSettings.WaveAmplitudeRange = EditorGUILayout.Vector2Field("Wave Amplitude Range", waveMovementSettings.WaveAmplitudeRange);
                waveMovementSettings.WaveFrequency = EditorGUILayout.FloatField("Wave Frequency", waveMovementSettings.WaveFrequency);
                waveMovementSettings.AngleRange = EditorGUILayout.FloatField("Angle Range", waveMovementSettings.AngleRange);
                break;
            case EMovementType.CIRCLE:
                GUILayout.Label("Circle Movement Settings", EditorStyles.boldLabel);
                circleMovementSettings.CircleRadius = EditorGUILayout.FloatField("Circle Radius", circleMovementSettings.CircleRadius);
                break;
            case EMovementType.STRAIGHT:
                break;
            default: break;
        }

        GUILayout.Label("ActionType Type Settings", EditorStyles.boldLabel);
        switch (commonMovementSettings.ActionType)
        {
            case EActionType.DASH:
                GUILayout.Label("Dash Action Settings", EditorStyles.boldLabel);
                dashActionSettings.DashInterval = EditorGUILayout.FloatField("Dash Interval", dashActionSettings.DashInterval);
                dashActionSettings.DashDuration = EditorGUILayout.FloatField("Dash Duration", dashActionSettings.DashDuration);
                break;
            case EActionType.SURROUND:
                GUILayout.Label("Circle Movement Settings", EditorStyles.boldLabel);
                circleMovementSettings.CircleRadius = EditorGUILayout.FloatField("Circle Radius", circleMovementSettings.CircleRadius);
                break;
            default: break;
        }

        GUILayout.Label("Range Movement Settings", EditorStyles.boldLabel);
        rangeMovementSettings.MinDistance = EditorGUILayout.FloatField("Min Distance", rangeMovementSettings.MinDistance);
        rangeMovementSettings.DetectionRange = EditorGUILayout.FloatField("DetectionRange", rangeMovementSettings.DetectionRange);
        rangeMovementSettings.ExclusionRange = EditorGUILayout.FloatField("ExclusionRange", rangeMovementSettings.ExclusionRange);
        rangeMovementSettings.MinTimeBeforeExclusion = EditorGUILayout.FloatField("MinTimeBeforeExclusion", rangeMovementSettings.MinTimeBeforeExclusion);

        GUILayout.Label("Enemy Dodge Settings", EditorStyles.boldLabel);
        enemyDodgeSettings.DodgeSpeed = EditorGUILayout.FloatField("Dodge Speed", enemyDodgeSettings.DodgeSpeed);
        enemyDodgeSettings.DodgeDistance = EditorGUILayout.FloatField("Dodge Distance", enemyDodgeSettings.DodgeDistance);
        enemyDodgeSettings.DodgeDuration = EditorGUILayout.FloatField("Dodge Duration", enemyDodgeSettings.DodgeDuration);
        enemyDodgeSettings.DetectionRadius = EditorGUILayout.FloatField("Detection Radius", enemyDodgeSettings.DetectionRadius);
        enemyDodgeSettings.ProjectileLayer = EditorGUILayout.LayerField("Projectile Layer", enemyDodgeSettings.ProjectileLayer);

        if (GUILayout.Button("Create Enemy"))
        {
            CreateEnemy();
        }
    }

    void CreateEnemy()
    {
        GameObject enemy = new GameObject(enemyTypeString);
        Enemy enemyComponent = enemy.AddComponent<Enemy>();
        //GameObject enemyObject = new GameObject(enemyTypeString);
        //Enemy enemy = ScriptableObject.CreateInstance<Enemy>();

        enemyComponent.Initialize(enemyTypeString, enemySettings.TypeXPGem, enemySettings.Damage, enemySettings.BloodEffect);
        //enemy.Initialize(enemyType, enemySettings.TypeXPGem, enemySettings.Damage, enemySettings.BloodEffect);


        SpriteRenderer spriteRenderer = enemy.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = enemySprite;

        Animator animator = enemy.AddComponent<Animator>();
        animator.runtimeAnimatorController = enemyAnimatorController;

        HealthComponent healthComponent = enemy.AddComponent<HealthComponent>();
        healthComponent.MaxHealth = maxHealth;

        EnemyMovement enemyMovement = enemy.AddComponent<EnemyMovement>();
        enemyMovement.CommonMovementSettings.Initialize(
            commonMovementSettings.MoveSpeed, 
            commonMovementSettings.SpeedMultiplier,
            commonMovementSettings.AccelerationTime,
            commonMovementSettings.MoveType,
            commonMovementSettings.FollowType,
            commonMovementSettings.ActionType,
            animator);

        enemyMovement.SpiralMovementSettings.Initialize(spiralMovementSettings.SpiralTightnessRange, spiralMovementSettings.SpiralRadiusIncreaseTime);
        enemyMovement.WaveMovementSettings.Initialize(waveMovementSettings.WaveAmplitudeRange, waveMovementSettings.WaveFrequency, waveMovementSettings.AngleRange);
        enemyMovement.CircleMovementSettings.Initialize(circleMovementSettings.CircleRadius);
        enemyMovement.DashActionSettings.Initialize(dashActionSettings.DashInterval, dashActionSettings.DashDuration);

        enemyMovement.RangeMovementSettings.Initialize(
            rangeMovementSettings.MinDistance, 
            rangeMovementSettings.DetectionRange,
            rangeMovementSettings.ExclusionRange,
            rangeMovementSettings.MinTimeBeforeExclusion);

        EnemyDodge enemyDodge = enemy.AddComponent<EnemyDodge>();
        enemyDodge.Initialize(
            enemyDodgeSettings.DodgeSpeed,
            enemyDodgeSettings.DodgeDistance,
            enemyDodgeSettings.DodgeDuration,
            enemyDodgeSettings.DetectionRadius,
            enemyMovement,
            enemyDodgeSettings.ProjectileLayer);

        Rigidbody2D rigidbody2D = enemy.AddComponent<Rigidbody2D>();
        rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        rigidbody2D.simulated = true;
        rigidbody2D.mass = 0.75f;

        BoxCollider2D boxCollider2D = enemy.AddComponent<BoxCollider2D>();
        boxCollider2D.isTrigger = false;

        // Save as prefab in the project
        string localPath = "Assets/Prefab/Enemies/" + enemyTypeString + ".prefab";
        localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);
        PrefabUtility.SaveAsPrefabAsset(enemy, localPath);

        //// Save the asset
        //AssetDatabase.CreateAsset(enemy, localPath);
        //AssetDatabase.SaveAssets();
        //EditorUtility.FocusProjectWindow();
        //Selection.activeObject = enemy;

        // Clean up the temporary GameObject
        DestroyImmediate(enemy);

        Debug.Log("Enemy created and saved as prefab at " + localPath);
    }
}
