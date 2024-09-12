//using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Transform playerTransform;
    private Movement playerMovement;

    public float moveSpeed = 0.3f;
    public float speedMultiplier = 1.5f;
    public float range = 2f;
    public float circleSpeed = 1f;
    public float circleRadius = 2f;
    public float minDistance = 0.1f;
    public Vector2 spiralTightnessRange = new Vector2(0.5f, 1f);
    public float accelerationTime = 2.5f;
    public float radiusIncreaseTime = 2.5f;
    public float waveFrequency = 1f;
    public float waveAmplitude = 1f;
    public float waveDistance = 2f;
    private bool movingForward = true;
    private float movedWaveDistance = 0f;

    private float angle = 0f;
    private bool isInSpiralMode = true;
    private bool isDodging = false;
    private Vector2 playerMovementVector;
    private float currentDirection;
    private float currentSpeed;
    private Vector3 startPosition;

    private void Awake()
    {
        playerTransform = GameManager.Instance.playerTransform;
        isDodging = false;
        isInSpiralMode = true;
        startPosition = transform.position;
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
        bool isEnemyClose = distanceToPlayer < range;

        if (isEnemyClose && !isDodging)
        {
            isInSpiralMode = distanceToPlayer > minDistance;
            if (isInSpiralMode)
            {
                //Debug.Log("isInSpiralMode");
                MoveInSpiralTowardsPlayer(distanceToPlayer);
            }
            else
            {
                MoveInCircleAroundPlayer();
            }
        }
        if(!isEnemyClose && !isDodging)
        {
            float step = moveSpeed * Time.deltaTime;

            if (movingForward)
            {
                transform.position += Vector3.right * step;
                movedWaveDistance += step;

                if (movedWaveDistance >= waveDistance)
                {
                    movingForward = false;
                }
            }
            else
            {
                transform.position -= Vector3.right * step;
                movedWaveDistance -= step;

                if (movedWaveDistance <= 0)
                {
                    movingForward = true;
                }
            }

            float waveOffset = Mathf.Sin(Time.time * waveFrequency) * waveAmplitude;
            transform.position = new Vector3(transform.position.x, startPosition.y + waveOffset, transform.position.z);
        }
    }

    void HandlePlayerMovementChanged(Vector2 movementVector)
    {
        playerMovementVector = movementVector;
    }

    void MoveInSpiralTowardsPlayer(float distanceToPlayer)
    {
        Vector2 directionToPlayer = ((Vector2)playerTransform.position - (Vector2)transform.position).normalized;
        Vector2 playerDirection = playerMovementVector.normalized; // Направление движения игрока

        //if (playerDirection != Vector2.zero)
        {
            // Вычисляем угол между направлением к игроку и направлением игрока
            float angleBetween = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) - Mathf.Atan2(playerDirection.y, playerDirection.x);

            // Нормализуем угол между -π и π
            angleBetween = Mathf.Atan2(Mathf.Sin(angleBetween), Mathf.Cos(angleBetween));

            // Определяем направление спирали в зависимости от угла
            currentDirection = (angleBetween > 0) ? 1f : -1f;
            currentSpeed = (angleBetween > 0) ?
                Mathf.Lerp(currentSpeed, circleSpeed * speedMultiplier, Time.time / accelerationTime) :
                Mathf.Lerp(currentSpeed, circleSpeed, Time.time / accelerationTime);
        }

        // Определить минимальное и максимальное значения для spiralTightness
        float minTightness = spiralTightnessRange.x;
        float maxTightness = spiralTightnessRange.y;

        // Рассчитать spiralTightness в зависимости от расстояния
        float maxDistance = Mathf.Max(minDistance, distanceToPlayer); // Минимальное расстояние не может быть меньше minDistance
        float t = Mathf.InverseLerp(minDistance, maxDistance, distanceToPlayer); // Интерполяция от 0 до 1
        float spiralTightness = Mathf.Lerp(maxTightness, minTightness, t); // Интерполяция для spiralTightness

        float radiusInterpolation = Mathf.Clamp01(Time.time / radiusIncreaseTime);
        float initialRadius = Mathf.Lerp(minDistance, distanceToPlayer, radiusInterpolation);

        //float initialRadius = Mathf.Max(minDistance, distanceToPlayer - spiralTightness * Time.deltaTime);

        Vector2 spiralPosition;

        angle += currentSpeed * Time.deltaTime * currentDirection;
        //Debug.Log("currentDirection " + currentDirection);
        
        // Ограничение угла в диапазоне [0, 2π)
        angle = Mathf.Repeat(angle, Mathf.PI * 2f);
        
        spiralPosition = new Vector2(
            playerTransform.position.x + Mathf.Cos(angle) * initialRadius,
            playerTransform.position.y + Mathf.Sin(angle) * initialRadius
        );

        //if ((Vector2)transform.position != spiralPosition)
        {
            transform.position = Vector2.Lerp(transform.position, spiralPosition, Time.deltaTime * accelerationTime);
        }
    }

    void MoveInCircleAroundPlayer()
    {
        angle += circleSpeed * Time.deltaTime;

        Vector3 circularPosition = new Vector3(
            playerTransform.position.x + Mathf.Cos(angle) * circleRadius,
            playerTransform.position.y + Mathf.Sin(angle) * circleRadius,
            transform.position.z
        );

        Vector3 direction = (circularPosition - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
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