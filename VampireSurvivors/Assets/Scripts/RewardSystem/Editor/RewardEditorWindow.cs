using GluonGui.WorkspaceWindow.Views.WorkspaceExplorer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Rendering.CameraUI;
using static UnityEngine.EventSystems.EventTrigger;
using System.IO;

public class RewardEditorWindow : EditorWindow
{
    private List<RewardObject> rewards = new List<RewardObject>();
    private RewardObject newReward = null; 

    private RewardObject originalReward;
    private string originalRewardName;

    private bool isWeaponReward = false;
    private bool isPlayerStatReward = false; 
    private bool showRewardButtons = false;
    private int editingRewardIndex = -2;
    private int warningIndex = 0;
    private Vector2 scrollPosition;
    private string warningMessage;
    string prefabWeaponPath = "Assets/Prefab/PlayerWeapons/";

    [MenuItem("Tools/Reward Editor")]
    public static void ShowWindow()
    {
        GetWindow<RewardEditorWindow>("Reward Editor");
    }
    private void OnEnable()
    {
        LoadExistingRewards();
    }

    private void OnGUI()
    {
        EditorGUILayout.Space();

        if (editingRewardIndex >= -1)
        {
            if (GUILayout.Button("Cancel editing"))
            {
                CancelEditing();
            }
        }
        else
        {
            if (GUILayout.Button("Create a new reward"))
            {
                StartCreatingNewReward();
            }
        }

        if (showRewardButtons)
        {
            DisplayRewardSelectionButtons();
        }

        EditorGUILayout.Space();

        if (newReward != null)
        {
            isWeaponReward = newReward is WeaponReward;
            isPlayerStatReward = newReward is PlayerStatReward;

            DisplayRewardFields();

            if (!string.IsNullOrEmpty(warningMessage))
            {
                EditorGUILayout.HelpBox(warningMessage, MessageType.Warning);
            }
            if (GUILayout.Button("Save the reward"))
            {
                SaveReward();
            }
        }
        
        //if(rewards != null)
        //{
        //    if (GUILayout.Button("Add rewards to Reward Manager"))
        //    {
        //        AddRewardsToRewardManager();
        //    }
        //}

        DisplayRewardList();
    }

    //private void AddRewardsToRewardManager()
    //{
    //    NewRewardManager rewardManager = FindObjectOfType<NewRewardManager>();

    //    if (rewardManager != null)
    //    {
    //        rewardManager.AddRewards(rewards);
    //    }
    //    else
    //    {
    //        Debug.LogError("NewRewardManager not found in scene.");
    //    }
    //}

    private void StartCreatingNewReward()
    {
        // If haven't saved the previous reward, clear the fields
        if (newReward != null && editingRewardIndex == -1)
        {
            if (EditorUtility.DisplayDialog("Unsaved data", "You have unsaved data. Are you sure you want to create a new reward?", "Yes", "No"))
            {
                newReward.rewardName = string.Empty;
                showRewardButtons = true;
            }
        }
        else
        {
            showRewardButtons = true;
        }
    }

    private void DisplayRewardSelectionButtons()
    {
        if (showRewardButtons)
        {
            EditorGUILayout.LabelField("Select a reward type:", EditorStyles.boldLabel);

            if (GUILayout.Button("Weapon"))
            {
                newReward = CreateInstance<WeaponReward>();
                showRewardButtons = false;
            }

            if (GUILayout.Button("Weapon Upgrade"))
            {
                newReward = CreateInstance<WeaponUpgradeReward>();
                showRewardButtons = false;
            }

            if (GUILayout.Button("Player stat"))
            {
                newReward = CreateInstance<PlayerStatReward>();
                showRewardButtons = false;
            }
            editingRewardIndex = -1;
        }
    }

    private void DisplayRewardFields()
    {
        EditorGUILayout.LabelField("Basic Reward Settings", EditorStyles.boldLabel);
        newReward.rewardName = EditorGUILayout.TextField("Reward Name", newReward.rewardName);
        if (string.IsNullOrEmpty(newReward.rewardName))
        {
            EditorGUILayout.HelpBox("Reward Name cannot be empty. Please fill it in.", MessageType.Warning);
        }
        newReward.description = EditorGUILayout.TextField("Reward Description", newReward.description);
        newReward.icon = (Sprite)EditorGUILayout.ObjectField("Reward Icon", newReward.icon, typeof(Sprite), false);

        if (isWeaponReward)
        {
            DisplayWeaponRewardFields(newReward as WeaponReward);
        }
        else if (newReward is WeaponUpgradeReward)
        {
            DisplayWeaponUpgradeRewardFields(newReward as WeaponUpgradeReward);
        }
        else if (isPlayerStatReward)
        {
            DisplayPlayerStatRewardFields(newReward as PlayerStatReward);
        }
    }

    // WeaponReward
    private void DisplayWeaponRewardFields(WeaponReward weaponReward)
    {
        EditorGUILayout.LabelField("Weapon settings", EditorStyles.boldLabel);
        weaponReward.weaponSprite = (Sprite)EditorGUILayout.ObjectField("Weapon Sprite", weaponReward.weaponSprite, typeof(Sprite), false);
        weaponReward.weaponType = (WeaponType)EditorGUILayout.EnumPopup("Weapon Type", weaponReward.weaponType);

        if (weaponReward.weaponType == WeaponType.AURA)
        {
            weaponReward.auraType = (AuraType)EditorGUILayout.EnumPopup("Aura Type", weaponReward.auraType);
            if (!string.IsNullOrEmpty(newReward.rewardName) && !IsValueInEnum<AuraType>(newReward.rewardName, weaponReward.auraType))
            {
                warningIndex = -1;
                EditorGUILayout.HelpBox("Add a new type to the enumeration AuraType or correct the name.", MessageType.Warning);
                if (GUILayout.Button("Open Enumeration Script"))
                {
                    OpenEnumerationFile("AuraType");
                }
            }
            else
            {
                warningIndex = 0;
            }
            weaponReward.radiusOfEffect = EditorGUILayout.FloatField("Radius Of Effect", weaponReward.radiusOfEffect);
        }
        else if (weaponReward.weaponType == WeaponType.PROJECTILE)
        {
            weaponReward.projectileType = (ProjectileType)EditorGUILayout.EnumPopup("Projectile Type", weaponReward.projectileType);
            if (!string.IsNullOrEmpty(newReward.rewardName) && !IsValueInEnum<ProjectileType>(newReward.rewardName, weaponReward.projectileType))
            {
                warningIndex = -1;
                EditorGUILayout.HelpBox("Add a new type to the enumeration ProjectileType or correct the name.", MessageType.Warning);
                if (GUILayout.Button("Open Enumeration Script"))
                {
                    OpenEnumerationFile("ProjectileType");
                }
            }
            else
            {
                warningIndex = 0;
            }
            weaponReward.projectileDirectionType = (ProjectileDirectionType)EditorGUILayout.EnumPopup("Projectile Direction", weaponReward.projectileDirectionType);
            weaponReward.projectileAngleStepType = (ProjectileAngleStepType)EditorGUILayout.EnumPopup("Projectile Angle Step", weaponReward.projectileAngleStepType);

            if (weaponReward.projectileAngleStepType == ProjectileAngleStepType.VALUE)
            {
                weaponReward.projectileAngleStepValue = EditorGUILayout.FloatField("Angle Step Value", weaponReward.projectileAngleStepValue);
            }

            weaponReward.timeBetweenShots = EditorGUILayout.FloatField("Time Between Shots", weaponReward.timeBetweenShots);
            weaponReward.projectileForce = EditorGUILayout.FloatField("Projectile Force", weaponReward.projectileForce);
            weaponReward.projectileCount = EditorGUILayout.IntField("Projectile Count", weaponReward.projectileCount);
            weaponReward.projectileDeactivationTime = EditorGUILayout.FloatField("Projectile Deactivation Time", weaponReward.projectileDeactivationTime);
        }

        weaponReward.cooldown = EditorGUILayout.FloatField("Cooldown", weaponReward.cooldown);
        weaponReward.damage = EditorGUILayout.FloatField("Damage", weaponReward.damage);
    }

    // WeaponUpgradeReward
    private void DisplayWeaponUpgradeRewardFields(WeaponUpgradeReward weaponUpgradeReward)
    {
        EditorGUILayout.LabelField("Weapon Upgrade Settings", EditorStyles.boldLabel);
        weaponUpgradeReward.weaponUpgradeType = (WeaponUpgradeType)EditorGUILayout.EnumPopup("Weapon Upgrade Type", weaponUpgradeReward.weaponUpgradeType);
        weaponUpgradeReward.weaponType = (WeaponType)EditorGUILayout.EnumPopup("Weapon Type", weaponUpgradeReward.weaponType);

        if (weaponUpgradeReward.weaponType == WeaponType.AURA)
        {
            weaponUpgradeReward.auraType = (AuraType)EditorGUILayout.EnumPopup("Aura Type", weaponUpgradeReward.auraType);
        }
        else if (weaponUpgradeReward.weaponType == WeaponType.PROJECTILE)
        {
            weaponUpgradeReward.projectileType = (ProjectileType)EditorGUILayout.EnumPopup("Projectile Type", weaponUpgradeReward.projectileType);
        }
        if(weaponUpgradeReward.weaponUpgradeType == WeaponUpgradeType.AdditionalProjectile)
        {
            weaponUpgradeReward.numberOfAdditionalProjectiles = EditorGUILayout.IntField("Number Of Additional Projectiles", weaponUpgradeReward.numberOfAdditionalProjectiles);
        }
        else
        {
            weaponUpgradeReward.upgradeValue = EditorGUILayout.FloatField("Upgrade Value", weaponUpgradeReward.upgradeValue);
        }
    }

    // PlayerStatReward
    private void DisplayPlayerStatRewardFields(PlayerStatReward playerStatReward)
    {
        EditorGUILayout.LabelField("Player Stat Settings", EditorStyles.boldLabel);
        playerStatReward.statType = (PlayerStatReward.StatType)EditorGUILayout.EnumPopup("Stat Type", playerStatReward.statType);
        playerStatReward.statIncreaseAmount = EditorGUILayout.FloatField("Stat Increase Amount", playerStatReward.statIncreaseAmount);
    }

    private bool IsValueInEnum<T>(string name, T enumName) where T : Enum
    {
        string sanitizedValue = name.Replace(" ", "");
        string enumNameString = enumName.ToString();
        return sanitizedValue == enumNameString;
    }

    private void OpenEnumerationFile(string enumClassName)
    {
        MonoScript script = FindMonoScriptByName(enumClassName);
        if (script != null)
        {
            AssetDatabase.OpenAsset(script);
        }
        else
        {
            Debug.LogError("Could not find script for enum: " + enumClassName);
        }
    }

    private MonoScript FindMonoScriptByName(string className)
    {
        MonoScript[] allScripts = Resources.FindObjectsOfTypeAll<MonoScript>();

        foreach (MonoScript script in allScripts)
        {
            if (script.name == className)
            {
                return script;
            }
        }

        return null;
    }

    private void SaveReward()
    {
        if(string.IsNullOrEmpty(newReward.rewardName))
        {
            warningMessage = "Reward Name cannot be empty. Please fill it in.";
        }
        else if(warningIndex == -1)
        {
            warningMessage = "Check another warning. Please fix it.";
        }
        else
        {
            warningMessage = null;

            string assetPath = "Assets/Prefab/Rewards/" + newReward.rewardName + ".asset";

            if (editingRewardIndex >= 0)
            {
                newReward.name = originalRewardName;
                rewards[editingRewardIndex] = newReward;
                EditorUtility.SetDirty(rewards[editingRewardIndex]);
            }
            else
            {
                if (AssetDatabase.LoadAssetAtPath<RewardObject>(assetPath) != null)
                {
                    warningMessage = "A reward with this name already exists. Please choose a different name.";
                    return;
                }
                AssetDatabase.CreateAsset(newReward, assetPath);
                rewards.Add(newReward);
            }

            newReward = null;
            editingRewardIndex = -2;

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

    private void CancelEditing()
    {
        if (newReward != null && editingRewardIndex == -1)
        {
            if (EditorUtility.DisplayDialog("Unsaved data", "You have unsaved data. Are you sure you want to cancel a new reward?", "Yes", "No"))
            {
                newReward = null;
                editingRewardIndex = -2;
                showRewardButtons = false;
            }
        }
        else if(newReward != null && editingRewardIndex >= 0)
        {
            if (EditorUtility.DisplayDialog("Unsaved data", "You have unsaved data. Are you sure you want to cancel a new reward?", "Yes", "No"))
            {
                EditorUtility.CopySerialized(originalReward, rewards[editingRewardIndex]);
                originalRewardName = null;
                newReward = null;
                originalReward = null;
                editingRewardIndex = -2;
            }
        }
        else
        {
            newReward = null;
            editingRewardIndex = -2;
            showRewardButtons = false;
        }

        warningIndex = 0;
        warningMessage = null;
        DestroyImmediate(newReward);

        GUI.FocusControl(null);
        //AssetDatabase.Refresh();
    }

    private void LoadExistingRewards()
    {
        rewards.Clear();

        string[] guids = AssetDatabase.FindAssets("t:RewardObject", new[] { "Assets/Prefab/Rewards" });

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            RewardObject reward = AssetDatabase.LoadAssetAtPath<RewardObject>(path);
            if (reward != null)
            {
                rewards.Add(reward);
            }
        }
    }

    private void DisplayRewardList()
    {
        EditorGUILayout.LabelField("Reward List", EditorStyles.boldLabel);

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(200));

        Dictionary<int, RewardObject> rewardsToRemove = new Dictionary<int, RewardObject>();

        for (int i = 0; i < rewards.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(rewards[i].rewardName, GUILayout.Width(150));

            GUIContent toggleAddRewardContent = new GUIContent("Added to Manager", "Add this award to the reward manager. If not selected, the reward will not be added to the manager.");
            EditorGUILayout.LabelField(toggleAddRewardContent, GUILayout.Width(110));
            bool isAdded = EditorGUILayout.Toggle(rewards[i].isAddedToManager);
            if (isAdded != rewards[i].isAddedToManager)
            {
                rewards[i].isAddedToManager = isAdded;
                NewRewardManager rewardManager = FindObjectOfType<NewRewardManager>();
                if (rewardManager != null)
                {
                    if (isAdded)
                    {
                        rewardManager.AddRewards(new List<RewardObject> { rewards[i] });
                    }
                    else
                    {
                        rewardManager.DeleteReward(rewards[i]);
                    }
                }
            }

            if (rewards[i] is WeaponReward weaponReward)
            {
                bool isAnyDefault = rewards.OfType<WeaponReward>().Any(r => r.isDefault);
                bool isEnabled = !isAnyDefault || weaponReward.isDefault;
                EditorGUI.BeginDisabledGroup(!isEnabled);

                GUIContent toggleDefaultContent = new GUIContent("Default Weapon", "Set this weapon as default. If selected, no other weapons can be set as default.");
                EditorGUILayout.LabelField(toggleDefaultContent, GUILayout.Width(100));

                bool newDefault = EditorGUILayout.Toggle(weaponReward.isDefault, GUILayout.Width(10));

                if (newDefault != weaponReward.isDefault)
                {
                    if (newDefault)
                    {
                        foreach (var reward in rewards.OfType<WeaponReward>())
                        {
                            reward.isDefault = false;
                        }
                    }
                    weaponReward.isDefault = newDefault;

                    if (weaponReward.isDefault)
                    {
                        ProjectileWeapon projectileWeapon = FindObjectOfType<ProjectileWeapon>();
                        if (projectileWeapon != null)
                        {
                            projectileWeapon.AddNewProjectile(weaponReward);
                        }
                        if(weaponReward.isAddedToManager)
                        {
                            NewRewardManager rewardManager = FindObjectOfType<NewRewardManager>();
                            if (rewardManager != null)
                            {
                                rewardManager.DeleteReward(weaponReward);
                                weaponReward.isAddedToManager = false;
                            }
                        }
                    }
                    if (!weaponReward.isDefault)
                    {
                        ProjectileWeapon projectileWeapon = FindObjectOfType<ProjectileWeapon>();
                        if (projectileWeapon != null)
                        {
                            projectileWeapon.DeleteProjectile(weaponReward);
                        }
                    }
                }

                if (GUILayout.Button("Create Prefab", GUILayout.Width(100)))
                {
                    CreatePrefabForWeaponType(weaponReward);
                }

                EditorGUI.EndDisabledGroup();

            }

            if (GUILayout.Button("Edit", GUILayout.Width(50)))
            {
                EditReward(i);
            }

            if (GUILayout.Button("Delete", GUILayout.Width(50)))
            {
                if (EditorUtility.DisplayDialog("Confirm Delete", "Are you sure you want to delete the award?", "Yes", "No"))
                {
                    rewardsToRemove.Add(i, rewards[i]);
                }
            }

            EditorGUILayout.EndHorizontal();
            if (GUI.changed)
            {
                EditorUtility.SetDirty(rewards[i]);
            }
        }

        EditorGUILayout.EndScrollView();

        foreach (var reward in rewardsToRemove)
        {
            if (reward.Value is WeaponReward weaponReward)
            {
                if (weaponReward.weaponType == WeaponType.PROJECTILE)
                {
                    ProjectileWeapon projectileWeapon = FindObjectOfType<ProjectileWeapon>();
                    if (projectileWeapon != null)
                    {
                        projectileWeapon.DeleteProjectile(weaponReward);
                    }
                    else
                    {
                        Debug.LogError("ProjectileWeapon not found in scene.");
                    }
                }
                if (weaponReward.weaponType == WeaponType.AURA)
                {

                }
                string prefabPath = prefabWeaponPath + weaponReward.rewardName + ".prefab";
                if (File.Exists(prefabPath))
                {
                    AssetDatabase.DeleteAsset(prefabPath);

                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();

                    Debug.Log($"Prefab {weaponReward.rewardName} is deleted.");
                }
                else
                {
                    Debug.LogWarning($"Prefab {weaponReward.rewardName} not found at path {prefabPath}.");
                }
            }
            NewRewardManager rewardManager = FindObjectOfType<NewRewardManager>();
            if (rewardManager != null)
            {
                rewardManager.DeleteReward(reward.Value);
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(reward.Value));
                rewards.RemoveAt(reward.Key);
            }
            else
            {
                Debug.LogError("NewRewardManager not found in scene.");
            }
        }
    }

    private void EditReward(int index)
    {
        if (rewards[index] is WeaponReward)
        {
            originalReward = CreateInstance<WeaponReward>();
        }
        else if (rewards[index] is WeaponUpgradeReward)
        {
            originalReward = CreateInstance<WeaponUpgradeReward>();
        }
        else if (rewards[index] is PlayerStatReward)
        {
            originalReward = CreateInstance<PlayerStatReward>();
        }
        else
        {
            Debug.LogError("Unsupported reward type!");
            return;
        }
        originalRewardName = rewards[index].rewardName;
        EditorUtility.CopySerialized(rewards[index], originalReward);

        newReward = rewards[index];
        editingRewardIndex = index;
    }

    private void CreatePrefabForWeaponType(WeaponReward weaponReward)
    {
        string newPrefabWeaponPath = prefabWeaponPath + weaponReward.rewardName + ".prefab";

        GameObject newPrefab = new GameObject(weaponReward.rewardName);
        newPrefab.tag = "PlayerAttack";

        if (weaponReward.weaponType == WeaponType.AURA)
        {
            newPrefab.layer = LayerMask.NameToLayer("Aura");

            //+++++++++++++++++++++++++
            //newPrefab.AddComponent<Aura>();

            PrefabUtility.SaveAsPrefabAsset(newPrefab, newPrefabWeaponPath);
            Debug.Log("Created prefab with Aura component");
        }
        else if (weaponReward.weaponType == WeaponType.PROJECTILE)
        {
            newPrefab.layer = LayerMask.NameToLayer("Projectiles");

            Projectile projectile = newPrefab.AddComponent<Projectile>();
            projectile.Initialize(weaponReward);

            SpriteRenderer spriteRenderer = newPrefab.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = weaponReward.weaponSprite;

            Rigidbody2D rigidbody2D = newPrefab.AddComponent<Rigidbody2D>();
            rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
            rigidbody2D.simulated = true;
            rigidbody2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rigidbody2D.sleepMode = RigidbodySleepMode2D.StartAwake;
            rigidbody2D.interpolation = RigidbodyInterpolation2D.Interpolate;

            BoxCollider2D boxCollider2D = newPrefab.AddComponent<BoxCollider2D>();
            boxCollider2D.isTrigger = true;

            var projectilePrefab = PrefabUtility.SaveAsPrefabAsset(newPrefab, newPrefabWeaponPath);
            weaponReward.projectilePrefab = projectilePrefab.GetComponent<Projectile>();
            projectile.WeaponReward = weaponReward;

            Debug.Log("Created prefab with Projectile component");
        }


        GameObject.DestroyImmediate(newPrefab);

        Debug.Log("Prefab created at: " + prefabWeaponPath);
    }
}