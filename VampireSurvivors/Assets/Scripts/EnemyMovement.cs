//using System;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Transform playerTransform;
    private Movement playerMovement;

    [SerializeField] private float moveSpeed = 0.3f;
    [SerializeField] private float speedMultiplier = 4f;
    [SerializeField] private float detectionRange = 2f;
    [SerializeField] private float exclusionRange = 4f;
    [SerializeField] private float minTimeBeforeExclusion = 3f;
    [SerializeField] private float circleRadius = 2f;
    [SerializeField] private float minDistance = 0.1f;
    [SerializeField] private Vector2 spiralTightnessRange = new Vector2(0.5f, 1f);
    [SerializeField] private float accelerationTime = 2.5f;
    [SerializeField] private float radiusIncreaseTime = 2.5f;
    [SerializeField] private float waveFrequency = 1f;
    [SerializeField] private float waveAmplitude = 1f;

    private float angle = 0f;
    private bool isInSpiralMode = true;
    private bool isDodging = false;
    private bool canCheckDistance = false;
    private Vector2 playerMovementVector;
    private float currentDirection;
    private float currentSpeed;
    private Vector3 startPosition;
    private float currentSpeedMultiplier;

    public event Action OnExclusion;

    private void Awake()
    {
        playerTransform = GameManager.Instance.playerTransform;
        isDodging = false;
        isInSpiralMode = true;
        canCheckDistance = false;
        startPosition = transform.position;
        currentSpeed = moveSpeed;
    }
    void OnEnable()
    {
        Invoke("EnableDistanceCheck", minTimeBeforeExclusion);
    }

    void Start()
    {
        playerMovement = playerTransform.GetComponent<Movement>();
        if (playerMovement != null)
        {
            playerMovement.OnMovementChanged -= HandlePlayerMovementChanged;
            playerMovement.OnMovementChanged += HandlePlayerMovementChanged;
        }
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance((Vector2)transform.position, (Vector2)playerTransform.position);

        bool isEnemyFar = distanceToPlayer > exclusionRange;

        if (canCheckDistance && isEnemyFar && OnExclusion != null)
        {
            //Debug.Log("OnExclusion");
            OnExclusion.Invoke();
        }

        bool isEnemyClose = distanceToPlayer < detectionRange;

        if (isEnemyClose && !isDodging && !isEnemyFar)
        {
            isInSpiralMode = distanceToPlayer > minDistance;
            if (isInSpiralMode)
            {
                //Debug.Log("isInSpiralMode");
                MoveInSpiralTowardsPlayer(distanceToPlayer);
            }
            else
            {
                //Debug.Log("isCircleMode");
                MoveInCircleAroundPlayer();
            }
        }
        if(!isEnemyClose && !isDodging)
        {
            //Debug.Log("isWaveMode");
            Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
            Vector2 playerDirection = playerMovementVector.normalized;
            float angleFactor = 0.5f;
            Vector3 movementDirection = Vector3.Slerp(directionToPlayer, playerDirection, angleFactor).normalized;

            Vector3 perpendicular = new Vector3(-movementDirection.y, movementDirection.x, 0);
            float waveOffset = Mathf.Sin(Time.time * waveFrequency) * waveAmplitude;
            Vector3 waveMotion = perpendicular * waveOffset;
            Vector3 finalMovement = movementDirection + waveMotion;

            transform.position += finalMovement.normalized * moveSpeed * Time.deltaTime;
        }
    }
    void EnableDistanceCheck()
    {
        canCheckDistance = true;
    }

    void HandlePlayerMovementChanged(Vector2 movementVector)
    {
        playerMovementVector = movementVector;
    }

    void MoveInSpiralTowardsPlayer(float distanceToPlayer)
    {
        Vector2 directionToPlayer = ((Vector2)playerTransform.position - (Vector2)transform.position).normalized;
        Vector2 playerDirection = playerMovementVector.normalized;

        // angle between the direction to the player and the direction of the player
        float angleBetween = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) - Mathf.Atan2(playerDirection.y, playerDirection.x);

        // normalize the angle between -pi and pi
        angleBetween = Mathf.Atan2(Mathf.Sin(angleBetween), Mathf.Cos(angleBetween));

        currentDirection = (angleBetween > 0) ? 1f : -1f;
        currentSpeedMultiplier = (angleBetween > 0) ? speedMultiplier : 1f;
        currentSpeed = Mathf.Lerp(currentSpeed, moveSpeed * currentSpeedMultiplier, Time.time / accelerationTime);

        float minTightness = spiralTightnessRange.x;
        float maxTightness = spiralTightnessRange.y;

        // Calculate the tightness of the spiral depending on the distance
        float maxDistance = Mathf.Max(minDistance, distanceToPlayer);
        float t = Mathf.InverseLerp(minDistance, maxDistance, distanceToPlayer);
        float spiralTightness = Mathf.Lerp(maxTightness, minTightness, t);

        float radiusInterpolation = Mathf.Clamp01(Time.time / radiusIncreaseTime);
        float initialRadius = Mathf.Lerp(minDistance, distanceToPlayer, radiusInterpolation);

        angle += currentSpeed * Time.deltaTime * currentDirection;
        angle = Mathf.Repeat(angle, Mathf.PI * 2f);

        Vector2 spiralPosition = new Vector2(
            playerTransform.position.x + Mathf.Cos(angle) * initialRadius,
            playerTransform.position.y + Mathf.Sin(angle) * initialRadius
        );

        transform.position = Vector2.Lerp(transform.position, spiralPosition, Time.deltaTime * accelerationTime * currentSpeedMultiplier);
    }

    void MoveInCircleAroundPlayer()
    {
        angle += moveSpeed * speedMultiplier * Time.deltaTime;
        angle = Mathf.Repeat(angle, Mathf.PI * 2f);

        Vector3 circularPosition = new Vector3(
            playerTransform.position.x + Mathf.Cos(angle) * circleRadius,
            playerTransform.position.y + Mathf.Sin(angle) * circleRadius,
            transform.position.z
        );

        transform.position = Vector3.Lerp(transform.position, circularPosition, moveSpeed * speedMultiplier * Time.deltaTime);
    }

    public void Dodge(bool canDodge)
    {
        isDodging = canDodge;
    }

    private void OnDestroy()
    {
        playerMovement.OnMovementChanged -= HandlePlayerMovementChanged;
    }
}