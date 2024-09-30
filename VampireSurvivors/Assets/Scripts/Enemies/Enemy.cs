using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using System;
using static EnemyDataLoader;

public enum EEnemyType
{
    HefariousScamp,
    SkeweringStalker
}

public class Enemy : GameplayMonoBehaviour
{
    private EnemyDataLoader enemyDataLoader; 
    private HealthComponent healthComponent;
    private EnemyMovement enemyMovement;
    private GameObject visualsObject;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private ParticleSystem bloodEffect;
    private ParticleSystem bloodSpawnEffect;
    

    private ObjectPool<Enemy> _pool;
    private bool _hasBeenReleased = false;
    [SerializeField] private TypeXPGem _typeXPGem;
    [SerializeField] private EEnemyType _type;
    [SerializeField] private float damage;

    public delegate void OnDeathEvent(Vector3 position, TypeXPGem _type);
    public event OnDeathEvent OnDeath;
    public event Action OnRelease;


    protected virtual void OnEnable()
    {
        spriteRenderer.color = Color.white;
        if (healthComponent = GetComponent<HealthComponent>())
        {
            healthComponent.OnDeath += HandleDeath;
            healthComponent.OnTakeDamage += HandleDamage;
        }
        if (enemyMovement = GetComponent<EnemyMovement>())
        {
            enemyMovement.RangeMovementSettings.OnExclusion += HandleRelease;
        }

        _hasBeenReleased = false;
    }

    void Start()
    {
        visualsObject = new GameObject("EnemyVisuals");
        visualsObject.transform.parent = this.transform;
        visualsObject.transform.localPosition = Vector3.zero;

        SpriteRenderer spriteRenderer = visualsObject.AddComponent<SpriteRenderer>();
        Animator animator = visualsObject.AddComponent<Animator>();

        if (enemyDataLoader = GetComponent<EnemyDataLoader>())
        {
            var enemyData = enemyDataLoader.GetEnemyData(_type);
            spriteRenderer.sprite = GetEnemySprite(enemyData.spritePath);
            animator.runtimeAnimatorController = GetEnemyAnimatorController(enemyData.animatorControllerPath);
        }

        spriteRenderer.sortingOrder = 1;
    }

    private Sprite GetEnemySprite(string spritePath)
    {
        return Resources.Load<Sprite>("Sprites/EnemySprite");
    }

    private RuntimeAnimatorController GetEnemyAnimatorController(string animatorControllerPath)
    {
        return Resources.Load<RuntimeAnimatorController>("Animators/EnemyAnimatorController");
    }

    protected override void UnPausableUpdate() 
    {
        if (bloodSpawnEffect != null && bloodSpawnEffect.isPaused)
        {
            bloodSpawnEffect.Play();
        }
    }

    protected override void PausableUpdate() 
    {
        if (bloodSpawnEffect != null && bloodSpawnEffect.isPlaying)
        {
            bloodSpawnEffect.Pause();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("SlowDown ");
            Movement movement = other.GetComponent<Movement>();
            if (movement != null)
            {
                movement.SlowDown();
            }
            else
            {
                Debug.LogWarning("Movement component not found on the Player.");
            }

            if (other.TryGetComponent<HealthComponent>(out HealthComponent playerHealth))
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }

    private IEnumerator Flash()
    {
        spriteRenderer.color = Color.red;
        float elapsedTime = 0f;
        float duration = 0.5f;

        while (elapsedTime < duration)
        {
            if (!Paused)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            else
            {
                yield return new WaitForSeconds(Time.deltaTime);
            }

        }

        spriteRenderer.color = Color.white;
    }

    protected void HandleDamage(float receivedDamage)
    {
        //Debug.Log("HandleDamage");
        StartCoroutine(Flash());
        if (bloodEffect != null)
        {
            bloodSpawnEffect = Instantiate(bloodEffect, transform.position, Quaternion.identity);
        }
        // receivedDamage - UI ?
    }

    protected void HandleDeath()
    {
        //Debug.Log("HandleDeath");
        Unsubcribe();

        if (!_hasBeenReleased)
        {
            _hasBeenReleased = true;
            OnDeath?.Invoke(transform.position, _typeXPGem);
            _pool.Release(this);
        }
    }

    protected void HandleRelease()
    {
        //Debug.Log("HandleRelease");
        Unsubcribe();

        if (!_hasBeenReleased)
        {
            _hasBeenReleased = true;
            OnRelease?.Invoke();
            _pool.Release(this);
        }
    }

    private void OnDisable()
    {
        Unsubcribe();
    }

    public void SetPool(ObjectPool<Enemy> pool)
    {
        _pool = pool;
    }

    public EEnemyType GetTypeEnemy()
    {
        return _type;
    }

    protected virtual void Unsubcribe()
    {
        if (healthComponent != null)
        {
            healthComponent.OnDeath -= HandleDeath;
            healthComponent.OnTakeDamage -= HandleDamage;
        }
        if (enemyMovement != null)
        {
            enemyMovement.RangeMovementSettings.OnExclusion -= HandleRelease;
        }
    }
}
