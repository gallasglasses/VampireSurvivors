using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : KillingActor
{
    [SerializeField] private float force = 10f; // +
    [SerializeField] private float damage = 1f; // +
    [SerializeField] private float deactivateTime = 5f; // +
    private ProjectileType projectileType; // +
    public ProjectileType ProjectileType
    {
        get=> ProjectileType;
    }

    private float powerUpDamage = 1;

    private Rigidbody2D body;
    private Vector3 direction;

    private ObjectPool<Projectile> _pool;
    private Coroutine deactivateProjectileAfterTimeCoroutine;

    private bool _hasBeenReleased = false;
    private bool _hasBeenPaused = false;

    private WeaponReward _weaponReward;
    public WeaponReward WeaponReward
    {
        get => _weaponReward;
        set => _weaponReward = value;
    }

    public void Initialize(WeaponReward weaponReward)
    {
        this.force = weaponReward.projectileForce;
        this.damage = weaponReward.damage;
        this.deactivateTime = weaponReward.projectileDeactivationTime;
        this.projectileType = weaponReward.projectileType;
    }

    protected override void Awake()
    {
        base.Awake();
        body = GetComponent<Rigidbody2D>();
    }

    protected override void PausableFixedUpdate()
    {
        base.PausableFixedUpdate(); 
        _hasBeenPaused = true;
        body.velocity = Vector2.zero; 
        body.isKinematic = true;
    }

    protected override void UnPausableFixedUpdate()
    {
        base.PausableUpdate();
        if (GetPauseDuration() > 0)
        {
            ResetDuration();
        }
        if(_hasBeenPaused)
        {
            body.isKinematic = false;
            body.velocity = new Vector2(direction.x, direction.y).normalized * force;
            _hasBeenPaused = false;
        }
    }

    private void OnEnable()
    {
        deactivateProjectileAfterTimeCoroutine = StartCoroutine(DeactivateProjectileAfterTime());

        SetVelocity();
        _hasBeenReleased = false;
    }

    public void SetDirection(Vector3 dir)
    {
        direction = dir;
    }

    public void SetPowerUpDamage(float powerUp)
    { 
        powerUpDamage = powerUp; 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_hasBeenReleased) return;

        if (collision.CompareTag("Enemy"))
        {
            if (collision.gameObject.TryGetComponent<HealthComponent>(out HealthComponent healthComponent))
            {
                DeliverEnemy(collision.gameObject);
                healthComponent.TakeDamage(damage * powerUpDamage, true);
            }

            _hasBeenReleased = true;
            _pool.Release(this);
        }
    }

    public void SetPool(ObjectPool<Projectile> pool)
    { 
        _pool = pool; 
    }

    private IEnumerator DeactivateProjectileAfterTime()
    {
        float elapsedTime = 0f;
        while (elapsedTime < deactivateTime)
        {
            if(!Paused)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            else
            {
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }

        if (!_hasBeenReleased)
        {
            _hasBeenReleased = true;
            _pool.Release(this);
        }
    }

    public void SetVelocity()
    {
        body.velocity = new Vector2(direction.x, direction.y).normalized * force;
    }
}
