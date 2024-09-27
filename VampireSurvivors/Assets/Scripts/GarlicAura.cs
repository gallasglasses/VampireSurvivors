using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GarlicAura : GameplayMonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float cooldown = 5f;
    //[SerializeField] private float auraRadius = 1f; // for changing radius of collision
    public LayerMask enemyLayer;
    private Animator garlicAnimator;

    private float nextCooldown;
    private float powerUpDamage = 1;
    private bool isGarlicEnable = false;

    private List<EnemyCooldownData> enemiesInCooldown = new List<EnemyCooldownData>();
    
    protected override void Awake()
    {
        base.Awake();
        garlicAnimator = GetComponent<Animator>();

        if (garlicAnimator == null)
        {
            Debug.LogError("Animator component not found on this GameObject.");
        }
    }

    void Start()
    {
        originalAnimationSpeed = garlicAnimator.speed;
    }

    protected override void UnPausableUpdate()
    {
        base.UnPausableUpdate();
        if (GetPauseDuration() > 0)
        {
            nextCooldown += GetPauseDuration();
            UpdateCooldowns(enemiesInCooldown, GetPauseDuration());
            ResetDuration();
        }
        if (garlicAnimator != null)
        {
            garlicAnimator.speed = originalAnimationSpeed;
        }
        if (isGarlicEnable)
        {
            UpdateCooldowns();
        }
    }

    protected override void PausableFixedUpdate()
    {
        base.PausableFixedUpdate();
        if (garlicAnimator != null)
        {
            garlicAnimator.speed = 0f;
        }
    }

    public void TriggerAnimation()
    {
        if (garlicAnimator != null)
        {
            garlicAnimator.enabled = true; 
            garlicAnimator.SetTrigger("EnableAuraTrigger");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && isGarlicEnable)
        {
            if (collision.gameObject.TryGetComponent<Enemy>(out Enemy enemy) && !IsEnemyOnCooldown(enemy))
            {
                if (collision.gameObject.TryGetComponent<HealthComponent>(out HealthComponent healthComponent))
                {
                    healthComponent.TakeDamage(damage * powerUpDamage);
                    nextCooldown = Time.time + cooldown;
                    enemiesInCooldown.Add(new EnemyCooldownData(enemy, nextCooldown));
                }
            }
        }
    }

    public void SetPowerUpDamage(float powerUp)
    {
        powerUpDamage = powerUp;
    }

    public void EnableAura()
    {
        isGarlicEnable = true;
        //Debug.Log($"EnableAura");
        TriggerAnimation();
    }

    private bool IsEnemyOnCooldown(Enemy enemy)
    {
        return enemiesInCooldown.Any(e => e.enemy == enemy);
    }

    private void UpdateCooldowns()
    {
        enemiesInCooldown.RemoveAll(e => e.cooldownEndTime <= Time.time);
    }
    private void UpdateCooldowns(List<EnemyCooldownData> cooldownList, float pauseDuration)
    {
        foreach (var cooldownData in cooldownList)
        {
            cooldownData.cooldownEndTime += pauseDuration;
        }
    }
}

public class EnemyCooldownData
{
    public Enemy enemy;
    public float cooldownEndTime;

    public EnemyCooldownData(Enemy enemy, float cooldownEndTime)
    {
        this.enemy = enemy;
        this.cooldownEndTime = cooldownEndTime;
    }
}
