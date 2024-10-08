using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.ShaderGraph.Internal;
using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set; }
    private EnemyManager enemyManager;
    [SerializeField] private int _spawnCount = 50; 
    public Dictionary<string, ObjectPool<Enemy>> pools = new(); 
    private List<Enemy> activeEnemies = new List<Enemy>();
    private bool isReturningActiveObjectsToPool = false;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        isReturningActiveObjectsToPool = false;
        pools = new();
        CreatePool();
    }

    public void ReturnActiveObjectsToPool()
    {
        Debug.Log($"Return Active Enemies {activeEnemies.Count}");
        if (activeEnemies != null && activeEnemies.Count > 0)
        {
            isReturningActiveObjectsToPool = true;
            for (int i = activeEnemies.Count - 1; i >= 0; i--)
            {
                if (activeEnemies[i] != null && activeEnemies[i].gameObject.activeSelf)
                {
                    Debug.Log($"activeEnemies {activeEnemies[i]}");
                    string enemyType = activeEnemies[i].GetTypeEnemy();
                    var pooledEnemy = pools[enemyType];
                    pooledEnemy.Release(activeEnemies[i]);
                }
                else
                {
                    activeEnemies.RemoveAt(i);
                }
            }
            activeEnemies.Clear();
            Debug.Log($"activeEnemies.Clear {activeEnemies.Count}");
        }

        PrintEnemiesDict(pools);
    }

    private void Start()
    {
        PrintEnemiesDict(pools);
    }

    private void CreatePool()
    {
        enemyManager = GetComponent<EnemyManager>();

        foreach (var e in enemyManager.enemiesDict)
        {
            var prefab = e.Value;
            if (prefab != null)
            {
                var _pool = new ObjectPool<Enemy>(() => CreateEnemy(prefab), OnTakeEnemyFromPool, OnReturnEnemyToPool, OnDestroyEnemy, true, _spawnCount, _spawnCount);
                pools.Add(e.Key, _pool);
            }
        }
    }

    public void PrintEnemiesDict(Dictionary<string, ObjectPool<Enemy>> pool)
    {
        foreach (var p in pool)
        {
            string enemyType = p.Key;
            var enemy = p.Value;

            Debug.Log($"EnemyType: {enemyType}, Enemy: {enemy}");
        }
    }

    private Enemy CreateEnemy(Enemy _prefab)
    {
        var enemy = Instantiate(_prefab, enemyManager.GetRandomSpawnPosition(), Quaternion.identity);
        if (enemy != null)
        {
            string enemyType = _prefab.GetTypeEnemy();
            var pooledEnemy = pools[enemyType];
            Debug.Log($"Spawning enemy of type: {enemyType}");
            enemy.SetPool(pooledEnemy);
        }
        Debug.Log($"Spawning enemy of type: {_prefab.GetTypeEnemy()}");

        return enemy;
    }

    private void OnTakeEnemyFromPool(Enemy enemy)
    {
        Debug.Log($"OnTakeEnemyFromPool: {enemy}");
        enemy.transform.position = enemyManager.GetRandomSpawnPosition();
        enemy.gameObject.SetActive(true);
        activeEnemies.Add(enemy);
    }

    private void OnReturnEnemyToPool(Enemy enemy)
    {
        Debug.Log($"OnReturnEnemyToPool: {enemy}");
        enemy.gameObject.SetActive(false); 
        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);
            if (isReturningActiveObjectsToPool)
            {
                Destroy(enemy.gameObject);
            }
        }
    }

    private void OnDestroyEnemy(Enemy enemy)
    {
        Debug.Log($"OnDestroyEnemy | enemy: {enemy}");
        Destroy(enemy.gameObject);
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
