using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{
    [SerializeField] public float force = 2.5f;
    [SerializeField] public float deactivateTime = 15f;
    [SerializeField] public float damage = 1f;
    private Vector3 mousePosition;
    private Camera mainCamera;
    private Rigidbody2D body;
    private Vector3 direction;
    private ObjectPool<Projectile> _pool;
    private Coroutine deactivateProjectileAfterTimeCoroutine;

    private bool _hasBeenReleased = false;

    private void Awake()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        body = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        //SetVelocity();
    }

    private void OnEnable()
    {
        deactivateProjectileAfterTimeCoroutine = StartCoroutine(DeactivateProjectileAfterTime());

        SetVelocity();
        _hasBeenReleased = false;
    }

    void Update()
    {
    }

    public void SetDirection(Vector3 dir)
    {
        direction = dir;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_hasBeenReleased) return;

        HealthComponent healthComponent = collision.gameObject.GetComponent<HealthComponent>();

        if (healthComponent != null)
        {
            Debug.Log("TakeDamage");
            healthComponent.TakeDamage(damage);
        }
        _hasBeenReleased = true;
        _pool.Release(this);
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
            elapsedTime += Time.deltaTime;
            yield return null;
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
