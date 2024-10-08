using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.EventTrigger;
using Random = UnityEngine.Random;

public class EnemyManager : GameplayMonoBehaviour
{
    public static EnemyManager Instance { get; private set; }
    [Header("Wave Settings")]
    [SerializeField] private float waveDuration = 60f;
    [SerializeField] private int maxWaves = 10;
    [SerializeField] private float intervalDuration = 10f;
    private float nextWaveTime = 0;
    private float nextIntervalTime = 0;
    private int spawnedWaves = -1; 
    private int currentInterval = 0;
    private bool isSpawningResiduals = false;

    [Header("Enemy Spawn Settings")]
    [SerializeField] private List<string> enemyTypes = new List<string>();
    [SerializeField] private List<Enemy> enemyPrefabs = new List<Enemy>();

    //[SerializeField] private MyDictionary<string, Enemy> enemies;
    public Dictionary<string, Enemy> enemiesDict = new();

    private EnemySpawner enemySpawner;
    [SerializeField] private Vector2 spawnArea;
    [SerializeField] private int totalEnemiesPerWave = 60;
    [SerializeField] private int maxEnemiesAlive = 300;
    [SerializeField] private int startEnemiesAmount = 10;
    private int enemiesPerInterval;
    private int enemiesSpawnedThisWave = 0;
    private int enemiesAlive = 0;

    [Header("Gem Spawn Settings")]
    public ExperienceGem gem;
    private GemSpawner gemSpawner;
    private Sprite[] gemSprites;
    [SerializeField] private string spriteFolderPath = "ExperienceGemSprites";

    private Transform playerTransform;
    private Movement playerMovement;
    private Vector2 playerMovementVector;


    protected override void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }

        base.Awake();
        UpdateEnemiesDictionary();

    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (enemySpawner == null)
        {
            enemySpawner = GetComponent<EnemySpawner>();
        }
        if (gemSpawner == null)
        {
            gemSpawner = GetComponent<GemSpawner>();
        }
        enemiesAlive = 0;
        enemiesSpawnedThisWave = 0;
        currentInterval = 0;
        spawnedWaves = -1;
        Debug.Log($"spawnedWaves: {spawnedWaves}");
        isSpawningResiduals = false;
        nextWaveTime = Time.time;
        nextIntervalTime = Time.time;
    }

    public void PrintEnemiesDict(Dictionary<string, Enemy> enemies)
    {
        foreach (var p in enemies)
        {
            string enemyType = p.Key;
            var enemy = p.Value;

            Debug.Log($"EnemyType: {enemyType}, Enemy: {enemy}");
        }
    }

    void Start()
    {
        enemySpawner = GetComponent<EnemySpawner>();
        gemSpawner = GetComponent<GemSpawner>();

        playerTransform = GameManager.Instance.player.GetComponent<Transform>();
        playerMovement = GameManager.Instance.player.GetComponent<Movement>();
        if (playerMovement != null)
        {
            playerMovement.OnMovementChanged -= HandlePlayerMovementChanged;
            playerMovement.OnMovementChanged += HandlePlayerMovementChanged;
        }

        gemSprites = Resources.LoadAll<Sprite>(spriteFolderPath);

        nextWaveTime = Time.time;
        nextIntervalTime = Time.time;
    }

    protected override void FinishableUpdate()
    {
        base.FinishableUpdate();
        if (enemySpawner != null)
        {
            enemySpawner.ReturnActiveObjectsToPool();
        }
        if (gemSpawner != null)
        {
            gemSpawner.ReturnActiveObjectsToPool();
        }
    }

    protected override void UnPausableUpdate()
    {
        if(GetPauseDuration() > 0)
        {
            nextWaveTime += GetPauseDuration();
            nextIntervalTime += GetPauseDuration();
            ResetDuration();
        }

        if (Time.time >= nextWaveTime)
        {
            spawnedWaves++;
            StartNewWave();

            Debug.Log($"Spawned wave {spawnedWaves} ");

            nextWaveTime = Time.time + waveDuration;
        }

        if (Time.time >= nextIntervalTime && !isSpawningResiduals)
        {
            if (currentInterval <= (int)(waveDuration/intervalDuration) && enemiesAlive <= maxEnemiesAlive)
            {
                SpawnIntervalEnemies();
                currentInterval++;
            }
            nextIntervalTime = Time.time + intervalDuration;
        }
    }

    void StartNewWave()
    {
        Debug.Log("StartNewWave");
        enemiesSpawnedThisWave = 0;
        currentInterval = 0;
        isSpawningResiduals = false;

        nextIntervalTime = Time.time + intervalDuration;
        SpawnIntervalEnemies();
        currentInterval++;
        nextIntervalTime = Time.time + intervalDuration;
    }

    void SpawnIntervalEnemies()
    {
        enemiesPerInterval = Mathf.RoundToInt(CalculateEnemiesForTime(Time.time));
        Debug.Log($"Spawning {enemiesPerInterval} enemies for interval {currentInterval + 1}");
        for (int i = 0; i < enemiesPerInterval; i++)
        {
            SpawnEnemy();
            enemiesSpawnedThisWave++;
        }

    }

    float CalculateEnemiesForTime(float timeInSeconds)
    {
        float adjustedTime = timeInSeconds % (float)(waveDuration * maxWaves);
        return Mathf.CeilToInt(startEnemiesAmount + (adjustedTime / (float)(waveDuration * maxWaves)) * (totalEnemiesPerWave - startEnemiesAmount));
    }

    void SpawnEnemy()
    {
        if (enemySpawner == null)
        {
            //Debug.LogError("Enemy pool is not set.");
            return;
        }
        List<string> enemyKeys = enemySpawner.pools.Keys.ToList();
        int enemyIndex = spawnedWaves % enemySpawner.pools.Count;
        string selectedEnemyType = enemyKeys[enemyIndex];

        Enemy enemyToSpawn = null;
        if (enemySpawner.pools.TryGetValue(selectedEnemyType, out var pool))
        {
            Debug.Log($"selectedEnemyType : {selectedEnemyType} | pool : {pool}");
            enemyToSpawn = pool.Get();
            Debug.Log($"enemyToSpawn : {enemyToSpawn}");
        }

        enemyToSpawn.transform.position = GetRandomSpawnPosition();
        enemyToSpawn.OnRelease -= HandleEnemyRelease;
        enemyToSpawn.OnRelease += HandleEnemyRelease;
        enemyToSpawn.OnDeath -= HandleEnemyDeath;
        enemyToSpawn.OnDeath += HandleEnemyDeath;

        enemiesAlive++;
    }

    void SpawnGem(Vector3 position, TypeXPGem _type)
    {
        if (gemSpawner == null)
        {
            Debug.LogError("Gem pool is not set.");
            return;
        }
        var gem = gemSpawner._pool.Get();

        gem.SetTypeXPGem(_type);
        gem.UpdateGemSprite(GetSpriteByType(_type));
        gem.transform.position = position;
    }

    public Sprite GetSpriteByType(TypeXPGem type)
    {
        foreach (var sprite in gemSprites)
        {
            if (sprite.name == type.ToString())
            {
                return sprite;
            }
        }

        Sprite defaultSprite = System.Array.Find(gemSprites, sprite => sprite.name == "DEFAULT");

        return defaultSprite;
    }

    private void HandleEnemyDeath(Vector3 position, TypeXPGem _type)
    {
        enemiesAlive--;
        SpawnGem(position,_type);
    }

    private void HandleEnemyRelease()
    {
        enemiesAlive--;

        if (enemiesAlive < totalEnemiesPerWave * spawnedWaves)
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
    public void AddEnemyToDictionary(string key, Enemy enemy)
    {
        if (!enemiesDict.ContainsKey(key) && !enemyTypes.Contains(key))
        {
            enemiesDict.Add(key, enemy);
            enemyTypes.Add(key);
            enemyPrefabs.Add(enemy);
            Debug.Log($"Enemy {key} is added.");
        }
        else
        {
            Debug.Log($"Enemy {key} already exist.");
        }
    }

    public void UpdateEnemiesDictionary()
    {
        enemiesDict = new Dictionary<string, Enemy>();

        for(int i = 0; i < enemyTypes.Count; i++ )
        {
            enemiesDict[enemyTypes[i]] = enemyPrefabs[i];
        }
    }

    private void OnDisable()
    {
        if (playerMovement != null)
        {
            playerMovement.OnMovementChanged -= HandlePlayerMovementChanged;
        }
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
