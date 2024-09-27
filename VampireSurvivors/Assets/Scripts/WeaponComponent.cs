using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;

public class WeaponComponent : GameplayMonoBehaviour
{
    private Camera mainCamera;
    private Vector3 mousePosition;
    public Quaternion projectileRotation;

    public Projectile projectile;
    public GarlicAura garlicAura;
    public Transform projectileTransform;

    private ProjectileSpawner projectileSpawner;
    private int projectileCount = 1;

    private bool canFire;
    private bool isReadyAttack;
    private bool isGarlicTurnOn = false;
    private float timer;
    [SerializeField] private float timeBetweenFire;
    //[SerializeField] private float delay = 0.1f;
    [SerializeField] private float powerUpStep = 1f;
    [SerializeField] private float increasingMultiplier = 0.5f;
    [SerializeField] private float spawnRadius = 5f;
    private PlayerController playerController;

    void Start()
    {
        if (GameManager.Instance.player.TryGetComponent<PlayerController>(out PlayerController playerController))
        {
            playerController.OnAttack += HandleAttack;
        }
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        projectileSpawner = GetComponent<ProjectileSpawner>();
    }

    protected override void UnPausableUpdate()
    {
        base.UnPausableUpdate();
        if (GetPauseDuration() > 0)
        {
            timer -= GetPauseDuration();
            ResetDuration();
        }

        if (!canFire)
        {
            timer += Time.deltaTime;
            if (timer > timeBetweenFire)
            {
                canFire = true;
                timer = 0;
            }
        }

        if (isReadyAttack && canFire)
        {
            Attack();
        }
    }

    private void StartShooting()
    {
        float initialAngle = GetRawRotation();
        float angleStep = 360f / projectileCount;
        for (int i = 0; i < projectileCount; i++)
        {

            float currentAngle = initialAngle + i * angleStep;

            Vector3 projectileDirection = new Vector3(
                Mathf.Cos(currentAngle * Mathf.Deg2Rad),
                Mathf.Sin(currentAngle * Mathf.Deg2Rad),
                0
            );
            Vector3 spawnPosition = transform.position + projectileDirection * spawnRadius;
            projectileRotation = Quaternion.Euler(0, 0, currentAngle - 90);
            var projectile = projectileSpawner._pool.Get();
            projectile.transform.position = spawnPosition;
            projectile.transform.rotation = projectileRotation;
            projectile.SetDirection(projectileDirection);
            projectile.SetPowerUpDamage(powerUpStep);
            projectile.SetVelocity();
        }
    }

    public Vector3 GetDirection()
    {
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        return mousePosition - transform.position;
    }

    public float GetRawRotation()
    {
        Vector3 direction = GetDirection();
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }

    private void HandleAttack(bool canStart)
    {
        //Debug.Log("isReadyAttack " + canStart);
        isReadyAttack = canStart;
    }

    private void Attack()
    {
        canFire = false;

        StartShooting();
    }

    private void OnDestroy()
    {
        if (playerController != null)
        {
            playerController.OnAttack -= HandleAttack;
        }
    }

    public void PowerUpDamage()
    {
        powerUpStep = powerUpStep * increasingMultiplier + powerUpStep;
        //Debug.Log("PowerUpDamage");
    }

    public void PowerUpProjectile()
    {
        projectileCount++;
        //Debug.Log($"PowerUpProjectile | all {projectileCount} projectiles");
    }

    public void PowerUpGarlic()
    {
        if (isGarlicTurnOn)
        {
            garlicAura.SetPowerUpDamage(powerUpStep);
        }
        if (!isGarlicTurnOn)
        {
            if (garlicAura != null)
            {
                //Debug.Log($"PowerUpGarlic");
                isGarlicTurnOn = true;
                garlicAura.EnableAura();
            }
        }
    }

}
