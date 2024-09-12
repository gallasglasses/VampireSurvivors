using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public Enemy enemy;
    public Transform enemyTransform;

    private EnemySpawner enemySpawner;

    public int enemiesPerWave = 10;
    public float waveInterval = 5f;
    private float waveTimer;
    private int enemiesSpawnedInCurrentWave;
    private int currentEnemyCount;
    private int spawnedWaves;

    // Start is called before the first frame update
    void Start()
    {
        enemySpawner = GetComponent<EnemySpawner>();

        currentEnemyCount = 0;
        spawnedWaves = 0;
    }

    // Update is called once per frame 
    void Update()
    {
        waveTimer += Time.deltaTime;

        if (waveTimer >= waveInterval)
        {
            waveTimer = 0f;
            StartNewWave();
        }

    }

    void StartNewWave()
    {
        enemiesSpawnedInCurrentWave = 0;
        spawnedWaves++;

        while (enemiesSpawnedInCurrentWave < enemiesPerWave)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        if (enemySpawner == null)
        {
            Debug.LogError("Enemy pool is not set.");
            return;
        }
        var enemy = enemySpawner._pool.Get();

        enemy.transform.position = GetRandomSpawnPosition();

        var healthComponent = enemy.GetComponent<HealthComponent>();

        if (healthComponent != null)
        {
            Debug.Log("healthComponent ");
            healthComponent.OnDeath -= HandleEnemyDeath;
            healthComponent.OnDeath += HandleEnemyDeath;
        }

        enemiesSpawnedInCurrentWave++;
        currentEnemyCount++;
    }

    private void HandleEnemyDeath()
    {
        // Decrement the current enemy count
        currentEnemyCount--;

        // Ensure that if the wave is complete and there are dead enemies, they are replaced
        if (currentEnemyCount < enemiesPerWave * spawnedWaves)
        {
            SpawnEnemy();
        }
    }

    public Vector2 GetRandomSpawnPosition()
    {
        Vector3 center = transform.position;
        float radius = 10f;
        Vector2 randomCircle = Random.insideUnitCircle * radius;
        return new Vector2(center.x + randomCircle.x, center.y + randomCircle.y);
    }
}
