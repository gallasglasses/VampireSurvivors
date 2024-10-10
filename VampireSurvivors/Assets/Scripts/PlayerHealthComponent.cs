using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerHealthComponent : HealthComponent
{
    [SerializeField] private float maxHealthMultiplier = 1f;
    [SerializeField] private float healMultiplier = 1f;
    [SerializeField] private float increasingMultiplier = 0.1f;
    [SerializeField] private float healUpdateTime = 1f;
    [SerializeField] private float healModifier = 0.2f;

    private bool isAutoHeal = false;
    private Coroutine autoHealCoroutine;

    public void Initialize(
        float maxHealth,
        float maxHealthMultiplier,
        float healMultiplier,
        float increasingMultiplier,
        float healUpdateTime,
        float healModifier)
    {
        this.maxHealth = maxHealth;
        this.maxHealthMultiplier = maxHealthMultiplier;
        this.healMultiplier = healMultiplier;
        this.increasingMultiplier = increasingMultiplier;
        this.healUpdateTime = healUpdateTime;
        this.healModifier = healModifier;
    }

    protected override void UnPausableUpdate()
    {
        base.UnPausableUpdate();
        StartAutoHeal();
    }

    public override void TakeDamage(float damage, bool isEnemyDamaged)
    {
        base.TakeDamage(damage, false);
        StartAutoHeal();
    }

    public void PowerUpHealth()
    {
        maxHealthMultiplier = maxHealthMultiplier * increasingMultiplier + maxHealthMultiplier;
        maxHealth = maxHealth * maxHealthMultiplier;

        //Debug.Log("PowerUpHealth");
        InvokeHealthAction();
    }

    public void PowerUpAutoHeal()
    {
        if (!isAutoHeal)
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
        while (!IsHealthFull() && !IsDead())
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
