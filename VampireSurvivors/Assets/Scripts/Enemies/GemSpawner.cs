using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class GemSpawner : MonoBehaviour
{
    public ObjectPool<ExperienceGem> _pool;
    private EnemyManager enemyManager;

    private void Start()
    {
        enemyManager = GetComponent<EnemyManager>();
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
    }

    private void OnReturnGemToPool(ExperienceGem gem)
    {
        gem.gameObject.SetActive(false);
    }

    private void OnDestroyGem(ExperienceGem gem)
    {
        Destroy(gem.gameObject);
    }
}
