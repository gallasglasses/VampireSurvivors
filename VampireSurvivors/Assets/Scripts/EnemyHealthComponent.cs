using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthComponent : HealthComponent
{
    private float maxHealthMultiplier = 1f;
    private float defaultMaxHealth;
    public void Initialize(
        float maxHealth,
        float maxHealthMultiplier)
    {
        this.maxHealth = maxHealth;
        this.maxHealthMultiplier = maxHealthMultiplier;
    }

    protected override void Awake()
    {
        defaultMaxHealth = maxHealth; 
        PowerUpHealth();
        base.Awake();
    }

    private void PowerUpHealth()
    {
        if(GameManager.Instance.player.GetCurrentLevel() > 1)
        {
            maxHealthMultiplier = GameManager.Instance.player.GetCurrentLevel();
        }
        maxHealth = defaultMaxHealth * maxHealthMultiplier;

        //Debug.Log("PowerUpHealth");
        InvokeHealthAction();
    }
}
