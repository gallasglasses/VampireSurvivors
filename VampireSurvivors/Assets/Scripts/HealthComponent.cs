using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealthComponent : MonoBehaviour
{
    [Range(1f, 100f)] 
    public float maxHealth = 100f;

    private float health = 0f;

    public event Action OnDeath;

    private void OnEnable()
    {
        SetHealth(maxHealth);
    }

    public bool IsDead()
    {
        return Mathf.Approximately(health, 0f);
    }

    private void SetHealth(float newHealth)
    {
        var nextHealth = Mathf.Clamp(newHealth, 0f, maxHealth);
        var healthDelta = nextHealth - health;
        health = nextHealth;

        //broadcast event OnHealthChanged for ui
    }

    private bool IsHealthFull()
    {
        return Mathf.Approximately(health, maxHealth);
    }

    public void TakeDamage(float damage)
    {
        if (damage <= 0.0f || IsDead()) return;
        SetHealth(health - damage);

        if (IsDead() && OnDeath != null)
        {
            OnDeath.Invoke();
        }
    }
}
