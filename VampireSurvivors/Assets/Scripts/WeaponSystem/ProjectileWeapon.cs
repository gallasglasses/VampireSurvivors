using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static UnityEditor.Rendering.CameraUI;

public class ProjectileWeapon : GameplayMonoBehaviour
{
    [SerializeField] private RewardsInventory projectileInventory;
    public Dictionary<ProjectileType, Projectile> projectiles = new Dictionary<ProjectileType, Projectile>();
    public Dictionary<ProjectileType, ProjectileSettings> projectileSettings = new Dictionary<ProjectileType, ProjectileSettings>();
    public Dictionary<ProjectileType, float> cooldownTimers = new Dictionary<ProjectileType, float>();
    private ProjectileSpawner projectileSpawner;

    [SerializeField] private float spawnRadius = 5f;
    [SerializeField] private Projectile defaultProjectile;
    private Quaternion projectileRotation;
    private Camera mainCamera;

    protected override void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
            return;
        if (mainCamera == null)
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            if (mainCamera == null)
            {
                Debug.LogError("mainCamera not found!");
            }
        }
        foreach (var obtainedProjectile in projectileInventory.rewardsInventory)
        {
            Debug.Log($"obtainedProjectile {obtainedProjectile}...");
        }
        if(projectileInventory.rewardsInventory == null)
        {
            Debug.Log($"obtainedProjectile is null");
        }
        if (projectileSpawner == null)
        {
            projectileSpawner = GetComponent<ProjectileSpawner>();
        }

        InitializeProjectiles();
    }

    void InitializeProjectiles()
    {
        Debug.Log("Initializing projectiles...");
        foreach (var projactileFromInventory in projectileInventory.rewardsInventory)
        {
            if(projactileFromInventory is WeaponReward obtainedProjectile)
            {
                Debug.Log($"Processing {obtainedProjectile.projectileType}...");
                if (!projectiles.ContainsKey(obtainedProjectile.projectileType))
                {
                    projectiles.Add(obtainedProjectile.projectileType, obtainedProjectile.projectilePrefab);
                    Debug.Log($"projectile {obtainedProjectile.projectileType} is added.");
                }
                else
                {
                    Debug.Log($"projectile {obtainedProjectile.projectileType} already exist.");
                }

                if (!projectileSettings.ContainsKey(obtainedProjectile.projectileType))
                {
                    ProjectileSettings settings = new ProjectileSettings();
                    settings.Initialize(obtainedProjectile);

                    projectileSettings.Add(obtainedProjectile.projectileType, settings);
                    Debug.Log($"projectile settings {obtainedProjectile.projectileType} is added.");
                }
                else
                {
                    Debug.Log($"projectile settings {obtainedProjectile.projectileType} already exist.");
                }
            }
        }
        Debug.Log("Projectile initialization complete.");

        //foreach (var settings in projectileSettings.Values)
        //{
        //    settings.InitializeStrategy(mainCamera);
        //}

        InitializeCooldownTimers();
    }

    private void InitializeCooldownTimers()
    {
        foreach (var projectileEntry in projectileSettings)
        {
            if (!cooldownTimers.ContainsKey(projectileEntry.Key))
            {
                cooldownTimers[projectileEntry.Key] = 0f;
            }
        }
    }
    public void AddNewProjectile(WeaponReward weaponReward)
    {
        if (!projectileInventory.rewardsInventory.Contains(weaponReward))
        {
            projectileInventory.rewardsInventory.Add(weaponReward);
            Debug.Log($"Projectile {weaponReward.rewardName} is added.");
        }
        else
        {
            Debug.Log($"projectile {weaponReward.rewardName} already exist.");
        }

        #if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(projectileInventory);
            UnityEditor.AssetDatabase.SaveAssets();
        #endif
    }

    public void DeleteProjectile(WeaponReward weaponReward)
    {
        if (projectileInventory.rewardsInventory.Contains(weaponReward))
        {
            projectileInventory.rewardsInventory.Remove(weaponReward);
        }

        #if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(projectileInventory);
            UnityEditor.AssetDatabase.SaveAssets();
        #endif
        if (projectiles.ContainsKey(weaponReward.projectileType))
        {
            projectiles.Remove(weaponReward.projectileType);
            Debug.Log($"projectile {weaponReward.projectileType} is deleted.");
        }
        else
        {
            Debug.Log($"projectile {weaponReward.projectileType} not found.");
        }
        if (projectileSettings.ContainsKey(weaponReward.projectileType))
        {
            projectileSettings.Remove(weaponReward.projectileType);
            Debug.Log($"projectile settings {weaponReward.projectileType} is deleted.");
        }
        else
        {
            Debug.Log($"projectile settings {weaponReward.projectileType} not found.");
        }
    }

    protected override void FinishableUpdate()
    {
        base.FinishableUpdate();
        if (projectileSpawner != null)
        {
            projectileSpawner.ReturnActiveObjectsToPool();
        }
    }

    protected override void UnPausableUpdate()
    {
        base.UnPausableUpdate();

        if (projectileSettings == null) return;

        if (GetPauseDuration() > 0)
        {
            AdjustCooldownTimersAfterPause(GetPauseDuration());
            ResetDuration();
        }

        foreach (var projectileEntry in projectileSettings)
        {
            var projectileType = projectileEntry.Key;
            ProjectileSettings settings = projectileEntry.Value;

            if (cooldownTimers.ContainsKey(projectileType))
            {
                cooldownTimers[projectileType] += Time.deltaTime;

                if (cooldownTimers[projectileType] > settings.cooldown)
                {
                    cooldownTimers[projectileType] = 0;
                    Attack(projectileType);
                }
            }
        }
    }

    public void Attack(ProjectileType projectileType)
    {
        ProjectileSettings settings = projectileSettings[projectileType];
        

        switch (settings.AngleStepType)
        {
            case ProjectileAngleStepType.NONE:
                StartCoroutine(ShootWithInterval(settings));
                break;

            case ProjectileAngleStepType.VALUE:
                ShootWithCustomAngle(settings);
                break;

            case ProjectileAngleStepType.EQUAL:
                ShootWithEqualAngle(settings);
                break;
        }
    }

    private void AdjustCooldownTimersAfterPause(float pauseDuration)
    {
        foreach (var projectileType in cooldownTimers.Keys.ToList())
        {
            cooldownTimers[projectileType] -= pauseDuration;

            if (cooldownTimers[projectileType] < 0)
            {
                cooldownTimers[projectileType] = 0;
            }
        }
    }

    private IEnumerator ShootWithInterval(ProjectileSettings settings)
    {
        Vector3 direction = GetDirection(settings);
        for (int i = 0; i < settings.projectileCount; i++)
        {
            SpawnProjectile(direction, 0);
            yield return new WaitForSeconds(settings.TimeBetweenShots);
        }
    }

    private void ShootWithCustomAngle(ProjectileSettings settings)
    {
        float centerOffset = -(settings.projectileCount - 1) * settings.AngleStepValue / 2;

        Vector3 direction = GetDirection(settings);
        for (int i = 0; i < settings.projectileCount; i++)
        {
            float angle = centerOffset + i * settings.AngleStepValue;
            Vector3 newDirection = RotateDirection(direction, angle);
            SpawnProjectile(newDirection, angle);
        }
    }

    private void ShootWithEqualAngle(ProjectileSettings settings)
    {
        Vector3 direction = GetDirection(settings);
        float initialAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float angleStep = 360f / settings.projectileCount;

        for (int i = 0; i < settings.projectileCount; i++)
        {
            float currentAngle = /*initialAngle +*/ i * angleStep;
            Vector3 newDirection = RotateDirection(direction, currentAngle);
            SpawnProjectile(newDirection, currentAngle);
        }
    }

    private Vector3 RotateDirection(Vector3 baseDirection, float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);

        //return new Vector3( cos, sin, baseDirection.z );
        return new Vector3(
            cos * baseDirection.x - sin * baseDirection.y,
            sin * baseDirection.x + cos * baseDirection.y,
            baseDirection.z
        );
    }

    private void SpawnProjectile(Vector3 direction, float angle)
    {
        Vector3 spawnPosition = transform.position + direction * spawnRadius;
        Quaternion rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90);

        var projectile = projectileSpawner._pool.Get();
        projectile.transform.position = spawnPosition;
        projectile.transform.rotation = rotation;
        projectile.SetDirection(direction);
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private Vector3 GetDirection(ProjectileSettings settings)
    {
        Vector3 direction = Vector3.zero;
        if (settings.projectileDirectionType == ProjectileDirectionType.MOUSE)
        {
            direction = GetDirectionFromMouse();
        }
        else if(settings.projectileDirectionType == ProjectileDirectionType.NearestEnemy)
        {
            direction = GetDirectionToNearestEnemy();
        }
        else if (settings.projectileDirectionType == ProjectileDirectionType.PlayerMovement)
        {
            direction = GetDirectionFromPlayerMovement();
        }
        else if (settings.projectileDirectionType == ProjectileDirectionType.RANDOM)
        {
            direction = GetRandomDirection();
        }
        return direction;
    }

    public Vector3 GetDirectionFromMouse()
    {
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = (mousePosition - transform.position)/*.normalized*/;
        return new Vector3(direction.x, direction.y, 0);
    }

    public Vector3 GetDirectionToNearestEnemy()
    {
        Transform nearestEnemy = FindNearestEnemy();
        if (nearestEnemy != null)
        {
            return (nearestEnemy.position - transform.position).normalized;
        }

        return new RandomDirectionStrategy().GetDirection(transform);
    }

    private Transform FindNearestEnemy()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform nearestEnemy = null;
        float minDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (var enemy in enemies)
        {
            float distance = Vector3.Distance(currentPosition, enemy.transform.position);
            if (distance < minDistance)
            {
                nearestEnemy = enemy.transform;
                minDistance = distance;
            }
        }
        return nearestEnemy;
    }

    public Vector3 GetRandomDirection()
    {
        float randomAngle = Random.Range(0f, 360f);
        return new Vector3(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad), 0);
    }

    public Vector3 GetDirectionFromPlayerMovement()
    {
        var playerMovement = GameManager.Instance.player.GetComponent<Movement>();
        Vector2 playerMovementVector = new();
        if (playerMovement != null)
        {
            playerMovementVector = playerMovement.PlayerMovementDirection.normalized;
        }
        return playerMovementVector;
    }
}
