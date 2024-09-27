using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    //public ObjectPool<Enemy> _pool;
    private EnemyManager enemyManager;
    [SerializeField] private int _spawnCount = 50; 
    public Dictionary<TypeEnemy, ObjectPool<Enemy>> pools = new Dictionary<TypeEnemy, ObjectPool<Enemy>>();

    private void Start()
    {
        enemyManager = GetComponent<EnemyManager>();

        foreach (var e in enemyManager.enemiesDict)
        {
            var prefab = e.Value;
            if (prefab != null)
            {
                var _pool = new ObjectPool<Enemy>(()=>CreateEnemy(prefab), OnTakeEnemyFromPool, OnReturnEnemyToPool, OnDestroyEnemy, true, _spawnCount, _spawnCount);
                pools.Add(e.Key, _pool);
            }
        }
    }

    private Enemy CreateEnemy(Enemy _prefab)
    {
        var enemy = Instantiate(_prefab, enemyManager.GetRandomSpawnPosition(), Quaternion.identity);
        if (enemy != null)
        {
            var pooledEnemy = pools[_prefab.GetTypeEnemy()];
            enemy.SetPool(pooledEnemy);
        }

        return enemy;
    }

    private void OnTakeEnemyFromPool(Enemy enemy)
    {
        enemy.transform.position = enemyManager.GetRandomSpawnPosition();

        enemy.gameObject.SetActive(true);
    }

    private void OnReturnEnemyToPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
    }

    private void OnDestroyEnemy(Enemy enemy)
    {
        Destroy(enemy.gameObject);
    }
}
