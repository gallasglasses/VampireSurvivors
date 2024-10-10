using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Weapon : GameplayMonoBehaviour
{
    private Camera mainCamera;
    protected float Cooldown { get; set; }
    protected float timer;
    protected bool canFire;

    protected override void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
            return;
        if (mainCamera == null)
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            if (mainCamera == null)
            {
                Debug.LogError("mainCamera not found!");
            }
        }
    }

    public virtual void UpdateCooldown(float deltaTime)
    {
        if (!canFire)
        {
            timer += deltaTime;
            if (timer >= Cooldown)
            {
                canFire = true;
                timer = 0;
            }
        }
    }

    public abstract void Attack();
    
    protected Vector3 GetMouseDirection()
    {
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        return mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
    }

    protected float GetMouseRawRotation()
    {
        Vector3 direction = GetMouseDirection();
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }
}
