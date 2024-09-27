using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealthComponent : GameplayMonoBehaviour
{
    [Range(1f, 100f)] 
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float maxHealthMultiplier = 1f;
    [SerializeField] private float healMultiplier = 1f;
    [SerializeField] private float increasingMultiplier = 0.1f;
    [SerializeField] private float healUpdateTime = 1f;
    [SerializeField] private float healModifier = 0.2f;

    private float health = 0f;
    private bool isAutoHeal = false;

    private Coroutine autoHealCoroutine;

    public event Action OnDeath;
    public event Action OnHealthChanged;

    public delegate void OnTakeDamageEvent(float _damage);
    public event OnTakeDamageEvent OnTakeDamage;

    private void OnEnable()
    {
        SetHealth(maxHealth);
    }

    protected override void UnPausableUpdate()
    {
        StartAutoHeal();
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

        if (OnHealthChanged != null)
        {
            OnHealthChanged?.Invoke();
        }
    }

    private bool IsHealthFull()
    {
        return Mathf.Approximately(health, maxHealth);
    }

    public void TakeDamage(float damage)
    {
        if (damage <= 0.0f || IsDead()) return;
        SetHealth(health - damage);

        //Debug.Log($"TakeDamage {damage}");
        if (OnTakeDamage != null)
        {
            OnTakeDamage?.Invoke(damage);
        }

        if (IsDead() && OnDeath != null)
        {
            OnDeath?.Invoke();
        }
        StartAutoHeal();
    }

    public float GetHealthPercent()
    {
	    return health / maxHealth;
    }

    public void PowerUpHealth()
    {
        maxHealthMultiplier = maxHealthMultiplier * increasingMultiplier + maxHealthMultiplier;
        maxHealth = maxHealth * maxHealthMultiplier;

        //Debug.Log("PowerUpHealth");
        if (OnHealthChanged != null)
        {
            OnHealthChanged?.Invoke();
        }
    }

    public void PowerUpAutoHeal()
    {
        if(!isAutoHeal)
        {
            isAutoHeal = true;
        }
        else
        {
            //Debug.Log("PowerUpAutoHeal");
            healMultiplier = healMultiplier * increasingMultiplier + healMultiplier;
            healModifier = healModifier * healMultiplier;
        }

    }

    private IEnumerator HealUpdate()
    {
        while(!IsHealthFull() && !IsDead())
        {
            if (!Paused)
            {
                SetHealth(health + healModifier);

                //Debug.Log("Healing... Current Health: " + health);
                if (IsHealthFull() && IsDead())
                {
                    break;
                }
                yield return new WaitForSeconds(healUpdateTime);
            }
            else
            {
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
        autoHealCoroutine = null;
    }

    private void StartAutoHeal()
    {
        if (isAutoHeal && autoHealCoroutine == null && !IsHealthFull())
        {
            autoHealCoroutine = StartCoroutine(HealUpdate());
        }
    }
}
