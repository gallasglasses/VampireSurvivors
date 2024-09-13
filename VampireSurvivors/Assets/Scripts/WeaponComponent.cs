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

    public bool canFire;
    private float timer;
    public float timeBetweenFire;

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

        if(Input.GetMouseButton(0) && canFire)
        {
            canFire = false;
            projectileRotation = Quaternion.Euler(0, 0, GetRawRotation() - 90);
            var projectile = projectileSpawner._pool.Get();
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
}
