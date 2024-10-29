using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;


public class ProjectileSpawner : MonoBehaviour
{
    public ObjectPool<Projectile> _pool;
    [SerializeField] private int _spawnCount = 100;
    public Dictionary<ProjectileType, ObjectPool<Projectile>> pools = new();
    private List<Projectile> activeProjectiles = new List<Projectile>();
    private ProjectileWeapon projectileWeapon;
    private bool isReturningActiveObjectsToPool = false;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
            return;
        isReturningActiveObjectsToPool = false;
        CreatePool();
    }

    public void ReturnActiveObjectsToPool()
    {
        if(activeProjectiles != null && activeProjectiles.Count > 0)
        {
            isReturningActiveObjectsToPool = true;
            for (int i = activeProjectiles.Count - 1; i >= 0; i--)
            {
                if (activeProjectiles[i] != null && activeProjectiles[i].gameObject.activeSelf)
                {
                    Debug.Log($"activeProjectiles {activeProjectiles[i]}");
                    ProjectileType projectileType = activeProjectiles[i].ProjectileType;
                    var pooledProjectile = pools[projectileType];
                    pooledProjectile.Release(activeProjectiles[i]);
                }
                else
                {
                    activeProjectiles.RemoveAt(i);
                }
            }
            activeProjectiles.Clear();
        }
    }

    private void Start()
    {
        //
    }

    private void CreatePool()
    {
        projectileWeapon = GetComponent<ProjectileWeapon>();

        foreach (var e in projectileWeapon.projectiles)
        {
            var prefab = e.Value;
            if (prefab != null)
            {
                var _pool = new ObjectPool<Projectile>(() => CreateProjectile(prefab), OnTakeProjectileFromPool, OnReturnProjectileToPool, OnDestroyProjectile, true, _spawnCount, _spawnCount);
                pools.Add(e.Key, _pool);
            }
        }
    }

    private Projectile CreateProjectile(Projectile _prefab)
    {
        var projectile = Instantiate(_prefab, projectileWeapon.transform.position, Quaternion.identity);
        if (projectile != null)
        {
            var projectileType = _prefab.ProjectileType;
            var pooledProjectile = pools[projectileType];
            projectile.SetPool(pooledProjectile);
        }

        return projectile;
    }

    private void OnTakeProjectileFromPool(Projectile projectile)
    {
        projectile.gameObject.SetActive(true);
        activeProjectiles.Add(projectile);
    }

    private void OnReturnProjectileToPool(Projectile projectile)
    {
        projectile.gameObject.SetActive(false);
        if(activeProjectiles.Contains(projectile))
        {
            activeProjectiles.Remove(projectile);
            if (isReturningActiveObjectsToPool)
            {
                Destroy(projectile.gameObject);
            }
        }
    }

    private void OnDestroyProjectile(Projectile projectile)
    {
        Destroy(projectile.gameObject);
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
