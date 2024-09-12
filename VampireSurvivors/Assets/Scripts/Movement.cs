using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] public float speed;
    private float originalSpeed;
    private SpriteRenderer spriteRenderer;
    private Vector2 direction;
    public Animator animator;

    public event Action<Vector2> OnMovementChanged;

    void Start()
    {
        spriteRenderer = animator.GetComponent<SpriteRenderer>();
        originalSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        float xValue = Input.GetAxisRaw("Horizontal");
        float yValue = Input.GetAxisRaw("Vertical");

        Vector2 newDirection = new Vector2(xValue, yValue).normalized;

        if (newDirection != direction)
        {
            direction = newDirection;
            OnMovementChanged?.Invoke(direction);
        }

        AnimateMovement();
    }

    private void FixedUpdate()
    {
        this.transform.position += new Vector3(direction.x * originalSpeed * Time.deltaTime, direction.y * originalSpeed * Time.deltaTime, this.transform.position.z); 
    }

    private void AnimateMovement()
    {
        if (animator != null)
        {
            if(direction.magnitude > 0)
            {
                animator.SetBool("isMoving", true);

                if (direction.x != 0)
                {
                    spriteRenderer.flipX = direction.x < 0f;
                }
                //Debug.Log("direction.x : " + direction.x);
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
        //Debug.Log("Player slowed down " + speed);
    }

    public void RestoreSpeed()
    {
        originalSpeed = speed;
    }
}
