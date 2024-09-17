using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Enemy : MonoBehaviour
{
    private HealthComponent healthComponent;

    private ObjectPool<Enemy> _pool;
    private bool _hasBeenReleased = false;

    public delegate void OnReleaseEvent();
    public event OnReleaseEvent OnRelease;

    protected virtual void OnEnable()
    {
        if (TryGetComponent<HealthComponent>(out HealthComponent healthComponent))
        {
            healthComponent.OnDeath += HandleDeath;
        }

        _hasBeenReleased = false;
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

    protected void HandleDeath()
    {
        Unsubcribe();

        if (!_hasBeenReleased)
        {
            _hasBeenReleased = true;
            OnRelease?.Invoke();
            _pool.Release(this);
        }
    }

    private void OnDisable()
    {
        Unsubcribe();
    }

    public void SetPool(ObjectPool<Enemy> pool)
    {
        _pool = pool;
    }

    protected virtual void Unsubcribe()
    {
        if (healthComponent != null)
        {
            healthComponent.OnDeath -= HandleDeath;
        }
    }
}
