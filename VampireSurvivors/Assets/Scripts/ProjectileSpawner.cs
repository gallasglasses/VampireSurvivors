using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


public class ProjectileSpawner : MonoBehaviour
{
    public ObjectPool<Projectile> _pool;
    private WeaponComponent weaponComponent;

    private void Start()
    {
        weaponComponent = GetComponent<WeaponComponent>();
        _pool = new ObjectPool<Projectile>(CreateProjectile, OnTakeProjectileFromPool, OnReturnProjectileToPool, OnDestroyProjectile, true, 100, 200);

    }

    private Projectile CreateProjectile()
    {
        var projectile = Instantiate(weaponComponent.projectile, weaponComponent.projectileTransform.position, weaponComponent.projectileRotation);
        if (projectile != null)
        {
            projectile.SetPool(_pool);
        }

        return projectile;
    }

    private void OnTakeProjectileFromPool(Projectile projectile)
    {
        projectile.transform.position = weaponComponent.projectileTransform.position;
        projectile.transform.rotation = weaponComponent.projectileRotation;
        projectile.SetDirection(weaponComponent.GetDirection());
        projectile.SetVelocity();

        projectile.gameObject.SetActive(true);
    }

    private void OnReturnProjectileToPool(Projectile projectile)
    {
        projectile.gameObject.SetActive(false);
    }

    private void OnDestroyProjectile(Projectile projectile)
    {
        Destroy(projectile.gameObject);
    }
}
