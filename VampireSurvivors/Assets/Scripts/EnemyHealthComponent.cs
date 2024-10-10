using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthComponent : HealthComponent
{
    [Range(1.1f, 2f)]
    [SerializeField] private float maxHealthMultiplier = 1.1f;
    private float defaultMaxHealth;
    public void Initialize(
        float maxHealth,
        float maxHealthMultiplier)
    {
        this.maxHealth = maxHealth;
        this.maxHealthMultiplier = maxHealthMultiplier;
    }

    protected override void OnEnable()
    {
        defaultMaxHealth = maxHealth; 
        PowerUpHealth();
        base.OnEnable();
    }

    private void PowerUpHealth()
    {
        if(GameManager.Instance.player.GetCurrentLevel() >= 1)
        {
            maxHealth = defaultMaxHealth * Mathf.Pow(maxHealthMultiplier, GameManager.Instance.player.GetCurrentLevel());
        }
        else
        {
            maxHealth = defaultMaxHealth;
        }

        //Debug.Log("PowerUpHealth");
        //InvokeHealthAction();
    }
}
