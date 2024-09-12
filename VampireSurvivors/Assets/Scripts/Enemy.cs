using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Enemy : MonoBehaviour
{
    private HealthComponent healthComponent;
    private ObjectPool<Enemy> _pool;

    private void Awake()
    {
        healthComponent = GetComponent<HealthComponent>();

        if (healthComponent != null)
        {
            healthComponent.OnDeath -= HandleDeath;
            healthComponent.OnDeath += HandleDeath;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

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
        Debug.Log("Destroy ");
        _pool.Release(this);
    }

    private void OnDestroy()
    {
        if (healthComponent != null)
        {
            healthComponent.OnDeath -= HandleDeath;
        }
    }

    public void SetPool(ObjectPool<Enemy> pool)
    {
        _pool = pool;
    }
}
