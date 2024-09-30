using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SkeweringStalkerMovement : EnemyMovement
{
    //[Header("Child Movement Settings")]
    //[SerializeField] private float waveFrequency = 5f;
    //[SerializeField] private Vector2 waveAmplitudeRange;
    //[SerializeField] private float angleRange;
    //[SerializeField] private float dashInterval = 3f;
    //[SerializeField] private float dashDuration = 0.5f;

    //private Vector2 perpendicularDirection = Vector2.zero;
    //private Vector2 deviatedDirection;
    //private Vector3 dashStartPosition;
    //private Vector3 dashEndPosition;

    //private bool isInMovingMode = false;
    //private bool isInFollowingMode = true;
    //private bool isDashing = false;

    //private float dashTimer;
    //private float dashTime;

    //protected override void Awake()
    //{
    //    base.Awake();
    //    isInFollowingMode = true;
    //    isInMovingMode = false;
    //    perpendicularDirection = Vector2.zero;
    //}

    //protected override void FollowPlayer()
    //{
    //    isInMovingMode = false;
    //    isInFollowingMode = distanceToPlayer > minDistance;
    //    if (isInFollowingMode)
    //    {
    //        isDashing = false;
    //        ChangeCurrentSettings();
    //        MoveInStraight(); 
    //    }
    //    else
    //    {
    //        DashToPlayer();
    //    }

    //    base.FollowPlayer();
    //}

    //private void MoveInStraight()
    //{
    //    transform.position += (Vector3)directionToPlayer * currentSpeed * Time.deltaTime;
    //}

    //private void DashToPlayer()
    //{
    //    dashTimer += Time.deltaTime;

    //    if (isDashing)
    //    {
    //        dashTime += Time.deltaTime;

    //        transform.position = Vector3.Lerp(dashStartPosition, dashEndPosition, dashTime / dashDuration);

    //        if (dashTime >= dashDuration)
    //        {
    //            isDashing = false;
    //            dashTime = 0f;
    //        }
    //    }
    //    else
    //    {
    //        Vector3 offset = new Vector3(Mathf.Cos(Time.time * moveSpeed), Mathf.Sin(Time.time * moveSpeed), 0) * minDistance;

    //        transform.position = Vector3.Lerp(transform.position, playerTransform.position + offset, moveSpeed * Time.deltaTime);

    //        if (dashTimer >= dashInterval)
    //        {
    //            dashStartPosition = transform.position;
    //            Vector3 directionThroughPlayer = (transform.position - playerTransform.position).normalized;
    //            dashEndPosition = playerTransform.position - directionThroughPlayer * minDistance;

    //            isDashing = true;
    //            dashTimer = 0f;
    //        }
    //    }
    //}

    //protected override void Move()
    //{
    //    if (!isInMovingMode)
    //    {
    //        float randomDeviation = Random.Range(-angleRange, angleRange);

    //        deviatedDirection = Quaternion.Euler(0, 0, randomDeviation) * directionToPlayer;
    //        perpendicularDirection = new Vector2(-directionToPlayer.y, directionToPlayer.x);

    //        isInMovingMode = true;

    //    }
    //    Vector2 newDirection = (deviatedDirection + perpendicularDirection * GetWaveOffset()).normalized;

    //    transform.position += (Vector3)newDirection * moveSpeed * Time.deltaTime;
    //    base.Move();
    //}

    //private float GetWaveOffset()
    //{
    //    float maxDistance = Mathf.Max(minDistance, distanceToPlayer);
    //    float t = Mathf.InverseLerp(minDistance, initialDistanceToPlayer, maxDistance);
    //    float waveAmplitudeLerp = Mathf.Lerp(waveAmplitudeRange.y, waveAmplitudeRange.x, t);
    //    return Mathf.Sin(Time.time * waveFrequency) * waveAmplitudeLerp;
    //}
}
