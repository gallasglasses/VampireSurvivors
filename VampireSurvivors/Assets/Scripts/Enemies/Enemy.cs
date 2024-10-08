using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using System;
using Unity.VisualScripting;
using UnityEditor.Animations;

public enum EEnemyType
{
    HefariousScamp,
    SkeweringStalker
}

public class Enemy : GameplayMonoBehaviour
{
    [SerializeField] private string _type;
    [SerializeField] private TypeXPGem _typeXPGem;
    [SerializeField] private float _damage;
    [SerializeField] private ParticleSystem _bloodEffect;

    private HealthComponent healthComponent;
    private EnemyMovement enemyMovement;
    private SpriteRenderer spriteRenderer;
    private ParticleSystem bloodSpawnEffect;
    private ObjectPool<Enemy> _pool;
    private bool _hasBeenReleased = false;

    public delegate void OnDeathEvent(Vector3 position, TypeXPGem _type);
    public event OnDeathEvent OnDeath;
    public event Action OnRelease;

    public string Type
    {
        get => _type;
        set => _type = value;
    }
    public TypeXPGem TypeXPGem
    {
        get => _typeXPGem;
        set => _typeXPGem = value;
    }
    public float Damage
    {
        get => _damage;
        set => _damage = value;
    }
    public ParticleSystem BloodEffect
    {
        get => _bloodEffect;
        set => _bloodEffect = value;
    }

    public void Initialize(
        string type, 
        TypeXPGem typeXPGem, 
        float damage, 
        ParticleSystem bloodEffect)
    {
        this.Type = type;
        this.TypeXPGem = typeXPGem;
        this.Damage = damage;
        this.BloodEffect = bloodEffect;
    }
    public void Initialize(EnemyData data)
    {
        this.Type = data.enemyType;
        this.TypeXPGem = data.typeXPGem;
        this.Damage = data.damage;
        this.BloodEffect = data.bloodEffect;
    }

    public string GetTypeEnemy()
    {
        return _type;
    }

    private void OnEnable()
    {
        if (spriteRenderer = GetComponent<SpriteRenderer>())
        {
            spriteRenderer.color = Color.white;
        }
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
                playerHealth.TakeDamage(_damage);
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

    private void HandleDamage(float receivedDamage)
    {
        //Debug.Log("HandleDamage");
        if(spriteRenderer != null)
        {
            StartCoroutine(Flash());
            if (_bloodEffect != null)
            {
                bloodSpawnEffect = Instantiate(_bloodEffect, transform.position, Quaternion.identity);
            }
        }
        // receivedDamage - UI ?
    }

    private void HandleDeath()
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

    private void HandleRelease()
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

    private void Unsubcribe()
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
