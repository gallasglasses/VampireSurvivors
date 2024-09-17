using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    public ObjectPool<Enemy> _pool;
    private EnemyManager enemyManager;

    private void Start()
    {
        enemyManager = GetComponent<EnemyManager>();
        _pool = new ObjectPool<Enemy>(CreateEnemy, OnTakeEnemyFromPool, OnReturnEnemyToPool, OnDestroyEnemy, true, 50, 50);

    }

    private Enemy CreateEnemy()
    {
        var enemy = Instantiate(enemyManager.enemy, enemyManager.GetRandomSpawnPosition(), Quaternion.identity);
        if (enemy != null)
        {
            enemy.SetPool(_pool);
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
