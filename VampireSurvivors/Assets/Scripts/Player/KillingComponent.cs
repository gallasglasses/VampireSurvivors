using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using TMPro;

public class KillingComponent : MonoBehaviour
{
    private int _enemiesKilled;
    public event Action OnKilledNewEnemy;
    HealthComponent healthComponent;

    public int EnemiesKilled
    {  get => _enemiesKilled; }

    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _enemiesKilled = 0;
        if (OnKilledNewEnemy != null)
        {
            OnKilledNewEnemy?.Invoke();
        }
    }

    public void FoundEnemy(GameObject enemy)
    {
        healthComponent = enemy.GetComponent<HealthComponent>();
        if (healthComponent)
        {
            healthComponent.OnDeath += CountKills;
        }
    }

    void CountKills()
    {
        healthComponent.OnDeath -= CountKills;
        _enemiesKilled++;
        //Debug.Log($"Count Kills {_enemiesKilled}");
        if (OnKilledNewEnemy != null)
        {
            OnKilledNewEnemy?.Invoke();
        }
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
