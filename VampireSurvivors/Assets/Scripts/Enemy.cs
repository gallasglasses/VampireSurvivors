using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Enemy : MonoBehaviour
{
    private HealthComponent healthComponent;
    private EnemyMovement enemyMovement;

    private ObjectPool<Enemy> _pool;

    private void Awake()
    {
        if (TryGetComponent<HealthComponent>(out HealthComponent healthComponent))
        {
            healthComponent.OnDeath += HandleDeath;
        }

        if (TryGetComponent<EnemyMovement>(out EnemyMovement enemyMovement))
        {
            enemyMovement.OnExclusion += HandleDeath;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("SlowDown ");
            Movement movement = other.GetComponent<Movement>();
            if (movement != null)
            {
                movement.SlowDown();
            }
            else
            {
                Debug.LogWarning("Movement component not found on the Player.");
            }
        }
    }

    private void HandleDeath()
    {
        if (healthComponent != null)
        {
            healthComponent.OnDeath -= HandleDeath;
        }

        if (enemyMovement != null)
        {
            enemyMovement.OnExclusion -= HandleDeath;
        }

        _pool.Release(this);
    }

    private void OnDestroy()
    {
        if (healthComponent != null)
        {
            healthComponent.OnDeath -= HandleDeath;
        }

        if (enemyMovement != null)
        {
            enemyMovement.OnExclusion -= HandleDeath;
        }
    }

    public void SetPool(ObjectPool<Enemy> pool)
    {
        _pool = pool;
    }
}
