using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class WeaponComponent : MonoBehaviour
{
    private Camera mainCamera;
    private Vector3 mousePosition;
    public Quaternion projectileRotation;

    public Projectile projectile;
    public Transform projectileTransform;

    private ProjectileSpawner projectileSpawner;

    private bool canFire;
    private bool isReadyAttack;
    private float timer;
    [SerializeField] private float timeBetweenFire; 
    private PlayerController playerController;

    private void Awake()
    {
        if (GameManager.Instance.player.TryGetComponent<PlayerController>(out PlayerController playerController))
        {
            playerController.OnAttack += HandleAttack;
        }
    }

    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        projectileSpawner = GetComponent<ProjectileSpawner>();
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, GetRawRotation());

        if (!canFire)
        {
            timer += Time.deltaTime;
            if (timer > timeBetweenFire)
            {
                canFire = true;
                timer = 0;
            }
        }

        if(isReadyAttack && canFire)
        {
            Attack();
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
        Debug.Log("isReadyAttack " + canStart);
        isReadyAttack = canStart;
    }

    private void Attack()
    {
        canFire = false;
        projectileRotation = Quaternion.Euler(0, 0, GetRawRotation() - 90);
        var projectile = projectileSpawner._pool.Get();
    }
    private void OnDestroy()
    {
        if (playerController != null)
        {
            playerController.OnAttack -= HandleAttack;
        }
    }
}
