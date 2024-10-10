using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Rendering.CameraUI;

public class ProjectileWeapon : Weapon
{
    public Dictionary<ProjectileType, ProjectileSettings> projectileTypes = new Dictionary<ProjectileType, ProjectileSettings>();
    private ProjectileSpawner projectileSpawner;
    private Dictionary<ProjectileType, float> cooldownTimers = new Dictionary<ProjectileType, float>();


    private float spawnRadius = 5f;
    private Quaternion projectileRotation;

    public void InitializeProjectile(ProjectileType type, ProjectileSettings settings)
    {
        if (!projectileTypes.ContainsKey(type))
        {
            projectileTypes.Add(type, settings);
        }
    }

    private void InitializeCooldownTimers()
    {
        foreach (var projectileEntry in projectileTypes)
        {
            if (!cooldownTimers.ContainsKey(projectileEntry.Key))
            {
                cooldownTimers[projectileEntry.Key] = 0f;
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();

        InitializeCooldownTimers();
    }

    protected override void UnPausableUpdate()
    {
        base.UnPausableUpdate();
        Attack();
    }

    public override void Attack()
    {
        if (projectileTypes == null) return;

        if (GetPauseDuration() > 0)
        {
            AdjustCooldownTimersAfterPause(GetPauseDuration());
            ResetDuration();
        }

        foreach (var projectileEntry in projectileTypes)
        {
            var projectileType = projectileEntry.Key;
            ProjectileSettings settings = projectileEntry.Value;

            if (cooldownTimers.ContainsKey(projectileType))
            {
                cooldownTimers[projectileType] += Time.deltaTime;

                if (cooldownTimers[projectileType] > settings.cooldown)
                    continue;
            }

            Vector3 direction = settings.DirectionStrategy.GetDirection(transform);

            switch (settings.AngleStepType)
            {
                case ProjectileAngleStepType.NONE:
                    StartCoroutine(ShootWithInterval(settings, direction));
                    break;

                case ProjectileAngleStepType.VALUE:
                    ShootWithCustomAngle(settings, direction);
                    break;

                case ProjectileAngleStepType.EQUAL:
                    ShootWithEqualAngle(settings, direction);
                    break;
            }
            cooldownTimers[projectileType] = 0;
        }
    }

    private void AdjustCooldownTimersAfterPause(float pauseDuration)
    {
        foreach (var projectileType in cooldownTimers.Keys.ToList())
        {
            cooldownTimers[projectileType] -= pauseDuration;

            //if (cooldownTimers[projectileType] < 0)
            //{
            //    cooldownTimers[projectileType] = 0;
            //}
        }
    }

    private IEnumerator ShootWithInterval(ProjectileSettings settings, Vector3 direction)
    {
        for (int i = 0; i < settings.projectileCount; i++)
        {
            SpawnProjectile(direction);
            yield return new WaitForSeconds(settings.TimeBetweenShots);
        }
    }

    private void ShootWithCustomAngle(ProjectileSettings settings, Vector3 direction)
    {
        float centerOffset = -(settings.projectileCount - 1) * settings.AngleStepValue / 2;

        for (int i = 0; i < settings.projectileCount; i++)
        {
            float angle = centerOffset + i * settings.AngleStepValue;
            Vector3 newDirection = RotateDirection(direction, angle);
            SpawnProjectile(newDirection);
        }
    }

    private void ShootWithEqualAngle(ProjectileSettings settings, Vector3 direction)
    {
        float angleStep = 360f / settings.projectileCount;

        for (int i = 0; i < settings.projectileCount; i++)
        {
            Vector3 newDirection = RotateDirection(direction, i * angleStep);
            SpawnProjectile(newDirection);
        }
    }

    private Vector3 RotateDirection(Vector3 baseDirection, float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);

        return new Vector3(
            cos * baseDirection.x - sin * baseDirection.y,
            sin * baseDirection.x + cos * baseDirection.y,
            baseDirection.z
        );
    }

    private void SpawnProjectile(Vector3 direction)
    {
        Vector3 spawnPosition = transform.position + direction * spawnRadius;
        Quaternion rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

        var projectile = projectileSpawner._pool.Get();
        projectile.transform.position = spawnPosition;
        projectile.transform.rotation = rotation;
        projectile.SetDirection(direction);
    }
}
