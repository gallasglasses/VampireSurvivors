using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HefariousScampMovement : EnemyMovement
{
    //[Header("Child Movement Settings")]
    //[SerializeField] private float circleRadius = 2f;
    //[SerializeField] private Vector2 spiralTightnessRange = new Vector2(0.5f, 1f);
    //[SerializeField] private float spiralRadiusIncreaseTime = 2.5f;
    //[SerializeField] private float waveFrequency = 1f;
    //[SerializeField] private float waveAmplitude = 1f;

    //private bool isInSpiralMode = true;

    //protected override void Awake()
    //{
    //    isInSpiralMode = true;
    //    base.Awake();
    //}

    //protected override void FollowPlayer()
    //{
    //    isInSpiralMode = distanceToPlayer > minDistance;
    //    if (isInSpiralMode)
    //    {
    //        //Debug.Log("isInSpiralMode");
    //        ChangeCurrentSettings();
    //        MoveInSpiral();
    //    }
    //    else
    //    {
    //        //Debug.Log("isCircleMode");
    //        MoveInCircle();
    //    }
    //    base.FollowPlayer();
    //}

    //protected override void Move()
    //{
    //    //Debug.Log("isWaveMode");
    //    float angleFactor = 0.5f;
    //    Vector3 movementDirection = Vector3.Slerp(directionToPlayer, playerMovementVector, angleFactor).normalized;

    //    Vector3 perpendicular = new Vector3(-movementDirection.y, movementDirection.x, 0);
    //    float waveOffset = Mathf.Sin(Time.time * waveFrequency) * waveAmplitude;
    //    Vector3 waveMotion = perpendicular * waveOffset;
    //    Vector3 finalMovement = movementDirection + waveMotion;

    //    transform.position += finalMovement.normalized * moveSpeed * Time.deltaTime;
    //    base.Move();
    //}

    //void MoveInSpiral()
    //{
    //    float maxDistance = Mathf.Max(minDistance, distanceToPlayer);
    //    float t = Mathf.InverseLerp(minDistance, initialDistanceToPlayer, maxDistance);
    //    float spiralTightness = Mathf.Lerp(spiralTightnessRange.y, spiralTightnessRange.x, t);

    //    float spiralRadiusInterpolation = Mathf.Clamp01(Time.time / spiralRadiusIncreaseTime);
    //    float initialRadius = Mathf.Lerp(minDistance, distanceToPlayer - spiralTightness, spiralRadiusInterpolation);

    //    angle += currentSpeed * Time.deltaTime * currentDirection;
    //    angle = Mathf.Repeat(angle, Mathf.PI * 2f);

    //    Vector2 spiralPosition = new Vector2(
    //        playerTransform.position.x + Mathf.Cos(angle) * initialRadius,
    //        playerTransform.position.y + Mathf.Sin(angle) * initialRadius
    //    );

    //    transform.position = Vector2.Lerp(transform.position, spiralPosition, Time.deltaTime * accelerationTime /** currentSpeedMultiplier*/);
    //}

    //void MoveInCircle()
    //{
    //    angle += moveSpeed * speedMultiplier * Time.deltaTime;
    //    angle = Mathf.Repeat(angle, Mathf.PI * 2f);

    //    Vector3 circularPosition = new Vector3(
    //        playerTransform.position.x + Mathf.Cos(angle) * circleRadius,
    //        playerTransform.position.y + Mathf.Sin(angle) * circleRadius,
    //        transform.position.z
    //    );

    //    transform.position = Vector3.Lerp(transform.position, circularPosition, moveSpeed * speedMultiplier * Time.deltaTime);
    //}
}
