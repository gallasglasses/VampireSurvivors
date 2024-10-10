using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GarlicAura : GameplayMonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float cooldown = 5f;
    //[SerializeField] private float auraRadius = 1f; // for changing radius of collision
    public LayerMask enemyLayer;
    private Animator garlicAnimator;
    private SpriteRenderer spriteRenderer;

    private float nextCooldown;
    private float powerUpDamage = 1;
    private bool isEnabled = false;
    public bool IsEnabled
    {
        get => isEnabled; set => isEnabled = value;
    }

    private List<EnemyCooldownData> enemiesInCooldown = new List<EnemyCooldownData>();
    
    protected override void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        powerUpDamage = 1;
        isEnabled = false;

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = null;
        }

        garlicAnimator = GetComponent<Animator>();
        if (garlicAnimator == null)
        {
            Debug.LogError("Animator component not found on this GameObject.");
        }
        else
        {
            garlicAnimator.speed = 1f;
            garlicAnimator.ResetTrigger("EnableAuraTrigger");
            garlicAnimator.enabled = false;
        }
    }

    protected override void FinishableUpdate()
    {
        base.FinishableUpdate();
        isEnabled = false;
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = null;
        }
        if (garlicAnimator != null)
        {
            garlicAnimator.speed = 0f;
            garlicAnimator.ResetTrigger("EnableAuraTrigger");
            garlicAnimator.enabled = false;
        }
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
            garlicAnimator.speed = 1f;
        }
        if (isEnabled)
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
            garlicAnimator.speed = 1f;
            garlicAnimator.enabled = true; 
            garlicAnimator.SetTrigger("EnableAuraTrigger");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && isEnabled)
        {
            if (collision.gameObject.TryGetComponent<Enemy>(out Enemy enemy) && !IsEnemyOnCooldown(enemy))
            {
                if (collision.gameObject.TryGetComponent<HealthComponent>(out HealthComponent healthComponent))
                {
                    healthComponent.TakeDamage(damage * powerUpDamage, true);
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
        isEnabled = true;
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

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
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
