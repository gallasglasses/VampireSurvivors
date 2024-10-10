using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

public class GemSpawner : MonoBehaviour
{
    public ObjectPool<ExperienceGem> _pool;
    private List<ExperienceGem> activeGems = new List<ExperienceGem>();
    private EnemyManager enemyManager;
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
        _pool = null;
        CreatePool();
    }


    public void ReturnActiveObjectsToPool()
    {
        if(activeGems != null && activeGems.Count > 0)
        {
            isReturningActiveObjectsToPool = true; 
            for (int i = activeGems.Count - 1; i >= 0; i--)
            {
                if (activeGems[i] != null && activeGems[i].gameObject.activeSelf)
                {
                    Debug.Log($"activeGems {activeGems[i]}");
                    _pool.Release(activeGems[i]);
                }
                else
                {
                    activeGems.RemoveAt(i);
                }
            }
            activeGems.Clear();
        }
    }

    private void Start()
    {
        //CreatePool();
    }

    private void CreatePool()
    {
        enemyManager = FindObjectOfType<EnemyManager>();
        _pool = new ObjectPool<ExperienceGem>(CreateGem, OnTakeGemFromPool, OnReturnGemToPool, OnDestroyGem, true, 50, 50);
    }

    private ExperienceGem CreateGem()
    {
        var gem = Instantiate(enemyManager.gem, Vector3.zero, Quaternion.identity);
        if (gem != null)
        {
            gem.SetPool(_pool);
        }

        return gem;
    }

    private void OnTakeGemFromPool(ExperienceGem gem)
    {
        gem.gameObject.SetActive(true);
        activeGems.Add(gem);
    }

    private void OnReturnGemToPool(ExperienceGem gem)
    {
        gem.gameObject.SetActive(false);
        if (activeGems.Contains(gem))
        {
            activeGems.Remove(gem);
            if (isReturningActiveObjectsToPool)
            {
                Destroy(gem.gameObject);
            }
        }
    }

    private void OnDestroyGem(ExperienceGem gem)
    {
        Destroy(gem.gameObject);
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
