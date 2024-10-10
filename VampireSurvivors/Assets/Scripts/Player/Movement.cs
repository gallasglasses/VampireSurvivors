using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : GameplayMonoBehaviour
{
    [SerializeField] public float speed;
    [SerializeField] private float powerUpMultiplier = 1f;
    [SerializeField] private float increasingMultiplier = 0.1f;

    private float originalSpeed;
    private SpriteRenderer spriteRenderer;
    private Vector2 playerMovementDirection = Vector2.zero;
    public Vector2 PlayerMovementDirection
    {
        get => playerMovementDirection;
    }

    public Animator animator;

    public event Action<Vector2> OnMovementChanged;
    
    void Start()
    {
        spriteRenderer = animator.GetComponent<SpriteRenderer>();
        originalSpeed = speed;
        originalAnimationSpeed = animator.speed;
    }

    protected override void PausableFixedUpdate()
    {
        base.PausableFixedUpdate();
        if (animator != null)
        {
            animator.speed = 0f;
        }
    }

    protected override void UnPausableFixedUpdate()
    {
        base.UnPausableFixedUpdate();
        if (animator != null)
        {
            animator.speed = originalAnimationSpeed;
        }
        this.transform.position += new Vector3(playerMovementDirection.x * originalSpeed * Time.deltaTime, playerMovementDirection.y * originalSpeed * Time.deltaTime, this.transform.position.z); 
    }

    private void AnimateMovement()
    {
        if (animator != null)
        {
            if(playerMovementDirection.magnitude > 0)
            {
                animator.SetBool("isMoving", true);

                if (playerMovementDirection.x != 0)
                {
                    spriteRenderer.flipX = playerMovementDirection.x < 0f;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            RestoreSpeed();
        }
    }

    public void SlowDown()
    {
        originalSpeed = speed / 2;
    }

    public void RestoreSpeed()
    {
        originalSpeed = speed;
    }

    public void Move(Vector2 newDirection)
    {
        if(!Paused)
        {
            if (newDirection != playerMovementDirection)
            {
                playerMovementDirection = newDirection;
                OnMovementChanged?.Invoke(playerMovementDirection);
            }

            AnimateMovement();
        }
    }

    public void PowerUpSpeed()
    {
        powerUpMultiplier = powerUpMultiplier * increasingMultiplier + powerUpMultiplier;
        speed = speed * powerUpMultiplier;
        Debug.Log("PowerUpSpeed");
    }
}
