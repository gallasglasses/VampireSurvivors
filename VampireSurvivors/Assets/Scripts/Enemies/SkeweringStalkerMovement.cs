using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeweringStalkerMovement : EnemyMovement
{
    [Header("Child Movement Settings")]
    [SerializeField] private float zigzagFrequency = 5f;
    [SerializeField] private float zigzagAmplitude = 1.5f;
    [SerializeField] private Vector2 zigzagAmpVec;
    [SerializeField] private Vector2 angleRange;

    private Vector2 movementDirection = Vector2.zero;
    private Vector2 perpendicularDirection = Vector2.zero;
    private Vector2 deviatedDirection;
    private bool isMoving = false;

    protected override void Awake()
    {
        base.Awake();
        isMoving = false;
        movementDirection = Vector2.zero;
        perpendicularDirection = Vector2.zero;
    }

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
            //Debug.Log("directionToPlayer " + directionToPlayer);
            //if (Mathf.Abs(directionToPlayer.x) > Mathf.Abs(directionToPlayer.y))
            //{
            //    movementDirection = directionToPlayer.x > 0 ? Vector2.right : Vector2.left;
            //}
            //else
            //{
            //    movementDirection = directionToPlayer.y > 0 ? Vector2.up : Vector2.down;
            //}

            //deviatedDirection = ShiftDirection(GetRandomAngle());
            float randomAngle = GetRandomAngle();
            deviatedDirection = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));

            perpendicularDirection = new Vector2(-deviatedDirection.y, deviatedDirection.x);
            //perpendicularDirection = new Vector2(-movementDirection.y, movementDirection.x);

            isMoving = true;

        }
        //Vector2 newDirection = (movementDirection + perpendicularDirection * GetZigZagOffset()).normalized;
        Vector2 newDirection = (deviatedDirection + perpendicularDirection * GetZigZagOffset()).normalized;

        transform.position += (Vector3)newDirection * moveSpeed * Time.deltaTime;
        base.Move();
    }

    private float GetZigZagOffset()
    {
        //Debug.Log("distanceToPlayer " + distanceToPlayer);
        Debug.Log("initialDistanceToPlayer " + initialDistanceToPlayer);
        float maxDistance = Mathf.Max(minDistance, distanceToPlayer);
        float t = Mathf.InverseLerp(minDistance, initialDistanceToPlayer, maxDistance);
        float zigzagAmpLerp = Mathf.Lerp(zigzagAmpVec.x, zigzagAmpVec.y, t);
        Debug.Log("zigzagAmpLerp " + zigzagAmpLerp);
        return Mathf.Sin(Time.time * zigzagFrequency) * zigzagAmpLerp;
    }

    private Vector2 ShiftDirection(float angle)
    {
        float newX = movementDirection.x * Mathf.Cos(angle) - movementDirection.y * Mathf.Sin(angle);
        float newY = movementDirection.x * Mathf.Sin(angle) + movementDirection.y * Mathf.Cos(angle);
        return new Vector2(newX, newY);
    }

    private float GetRandomAngle()
    {
        float angle = Random.Range(angleRange.x, angleRange.y) * Mathf.Deg2Rad;
        if (movementDirection.x > 0)
        {
            angle = movementDirection.y > 0 ? angle : - angle;
        }
        else
        {
            angle = movementDirection.y > 0 ? Mathf.PI + angle : (3 * Mathf.PI)/2 + angle;
        }
        return angle;
    }
}
