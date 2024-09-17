using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeweringStalkerMovement : EnemyMovement
{
    [Header("Child Movement Settings")]
    [SerializeField] private float zigzagFrequency = 5f;
    [SerializeField] private float zigzagAmplitude = 1.5f;

    private Vector2 movementDirection = Vector2.zero;
    private Vector2 perpendicularDirection = Vector2.zero;
    private bool isMoving = false;

    protected override void FollowPlayer()
    {
        isMoving = false;
        ChangeCurrentSettings();
        transform.position += (Vector3)directionToPlayer * currentSpeed * Time.deltaTime;
        base.FollowPlayer();
    }

    protected override void Move()
    {

        if (!isMoving)
        {
            if (Mathf.Abs(directionToPlayer.x) > Mathf.Abs(directionToPlayer.y))
            {
                movementDirection = directionToPlayer.x > 0 ? Vector2.left : Vector2.right;
            }
            else
            {
                movementDirection = directionToPlayer.y > 0 ? Vector2.down : Vector2.up;
            }
            perpendicularDirection = new Vector2(-movementDirection.y, movementDirection.x);

            isMoving = true;

        }
        Vector2 deviatedDirection = (movementDirection + perpendicularDirection * GetZigZagOffset()).normalized;

        transform.position += (Vector3)deviatedDirection * moveSpeed * Time.deltaTime;
        base.Move();
    }

    private float GetZigZagOffset()
    {
        return Mathf.Sin(Time.time * zigzagFrequency) * zigzagAmplitude;
    }
}
