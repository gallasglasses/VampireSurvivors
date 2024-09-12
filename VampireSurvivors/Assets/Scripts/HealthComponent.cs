using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] public float maxHealth = 100f;
    private float health = 0f;

    public event Action OnDeath;

    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnEnable()
    {
        SetHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
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

        //Debug.Log("SetHealth " + health);
        //broadcast event OnHealthChanged
    }

    private bool IsHealthFull()
    {
        return Mathf.Approximately(health, maxHealth);
    }

    public void TakeDamage(float damage)
    {
        //Debug.Log("damage " + damage + " Health " + health);
        if (damage <= 0.0f || IsDead()) return;
        SetHealth(health - damage);

        if (IsDead() && OnDeath != null)
        {
            OnDeath.Invoke();
        }
    }
}
