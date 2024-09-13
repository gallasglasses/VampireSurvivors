using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Wave Settings")]
    [SerializeField] private int enemiesPerWave = 10;
    [SerializeField] private float waveInterval = 5f;
    private float currentWaveTime;
    private int spawnedWaves;

    [Header("Spawn Settings")]
    public Enemy enemy;
    public Transform enemyTransform;
    [SerializeField] private Vector2 spawnArea;

    private EnemySpawner enemySpawner;
    private Transform playerTransform;
    private Movement playerMovement;
    private Vector2 playerMovementVector;
    private int spawnedEnemiesInCurrentWave;
    private int currentEnemyCount;

    void Start()
    {
        enemySpawner = GetComponent<EnemySpawner>();
        playerTransform = GameManager.Instance.playerTransform;

        playerMovement = playerTransform.GetComponent<Movement>();
        if (playerMovement != null)
        {
            playerMovement.OnMovementChanged -= HandlePlayerMovementChanged;
            playerMovement.OnMovementChanged += HandlePlayerMovementChanged;
        }

        currentEnemyCount = 0;
        spawnedWaves = 0;
    }

    void Update()
    {
        currentWaveTime += Time.deltaTime;

        if (currentWaveTime >= waveInterval)
        {
            currentWaveTime = 0f;
            StartNewWave();
        }

    }

    void StartNewWave()
    {
        spawnedEnemiesInCurrentWave = 0;
        spawnedWaves++;

        while (spawnedEnemiesInCurrentWave < enemiesPerWave)
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
            //Debug.Log("healthComponent ");
            healthComponent.OnDeath -= HandleEnemyDeath;
            healthComponent.OnDeath += HandleEnemyDeath;
        }

        spawnedEnemiesInCurrentWave++;
        currentEnemyCount++;
    }

    private void HandleEnemyDeath()
    {
        currentEnemyCount--;

        if (currentEnemyCount < enemiesPerWave * spawnedWaves)
        {
            SpawnEnemy();
        }
    }

    public Vector2 GetRandomSpawnPosition()
    {
        Vector3 position = new Vector3();
        Vector2 movementDirection = playerMovementVector.normalized;
        if (movementDirection != Vector2.zero)
        {
            if (Mathf.Abs(movementDirection.x) > Mathf.Abs(movementDirection.y))
            {
                if (movementDirection.x > 0)
                {
                    position.x = spawnArea.x;  // spawn right
                    position.y = Random.Range(-spawnArea.y, spawnArea.y);
                }
                else
                {
                    position.x = -spawnArea.x; // spawn left
                    position.y = Random.Range(-spawnArea.y, spawnArea.y);
                }
            }
            else
            {
                if (movementDirection.y > 0)
                {
                    position.y = spawnArea.y;  // spawn up
                    position.x = Random.Range(-spawnArea.x, spawnArea.x);
                }
                else
                {
                    position.y = -spawnArea.y; // spawn down
                    position.x = Random.Range(-spawnArea.x, spawnArea.x);
                }
            }
        }
        else
        {
            float k = Random.value > 0.5f ? -1f : 1f;
            if (Random.value > 0.5f)
            {
                position.x = Random.Range(-spawnArea.x, spawnArea.x);
                position.y = spawnArea.y * k;
            }
            else
            {
                position.y = Random.Range(-spawnArea.y, spawnArea.y);
                position.x = spawnArea.x * k;
            }
        }

        position.z = 0f;

        position += playerTransform.position;

        return position;
    }
    void HandlePlayerMovementChanged(Vector2 movementVector)
    {
        playerMovementVector = movementVector;
    }
}
