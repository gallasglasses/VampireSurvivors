using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class HealthComponent : GameplayMonoBehaviour
{
    [Range(1f, 100f)] 
    [SerializeField] protected float maxHealth = 100f;

    protected float health = 0f;

    public event Action OnDeath;
    public event Action OnHealthChanged;

    public delegate void OnTakeDamageEvent(float _damage);
    public event OnTakeDamageEvent OnTakeDamage;

    public float MaxHealth
    {
        get => maxHealth;
        set => maxHealth = value;
    }
    public virtual void Initialize(
        float maxHealth)
    {
        this.maxHealth = maxHealth;
    }

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        SetHealth(maxHealth);
        Debug.Log($"maxHealth {maxHealth}");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetHealth(maxHealth);
        Debug.Log($"maxHealth {maxHealth}");
    }

    public bool IsDead()
    {
        return Mathf.Approximately(health, 0f);
    }

    protected void SetHealth(float newHealth)
    {
        var nextHealth = Mathf.Clamp(newHealth, 0f, maxHealth);
        var healthDelta = nextHealth - health;
        health = nextHealth;
        InvokeHealthAction();
    }

    protected void InvokeHealthAction()
    {
        if (OnHealthChanged != null)
        {
            OnHealthChanged?.Invoke();
        }
    }

    protected bool IsHealthFull()
    {
        return Mathf.Approximately(health, maxHealth);
    }

    public virtual void TakeDamage(float damage)
    {
        if (damage <= 0.0f || IsDead()) return;
        SetHealth(health - damage);

        //Debug.Log($"TakeDamage {_damage}");
        if (OnTakeDamage != null)
        {
            OnTakeDamage?.Invoke(damage);
        }

        if (IsDead() && OnDeath != null)
        {
            OnDeath?.Invoke();
        }
    }

    public float GetHealthPercent()
    {
	    return health / maxHealth;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
