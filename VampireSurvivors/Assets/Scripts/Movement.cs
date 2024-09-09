using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] public float speed;
    private SpriteRenderer spriteRenderer;
    private Vector3 direction;
    public Animator animator;

    void Start()
    {
        spriteRenderer = animator.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float xValue = Input.GetAxisRaw("Horizontal");
        float yValue = Input.GetAxisRaw("Vertical");

        direction = new Vector3(xValue, yValue).normalized;

        AnimateMovement();
    }

    private void FixedUpdate()
    {
        this.transform.position += direction * speed * Time.deltaTime; 
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
}
