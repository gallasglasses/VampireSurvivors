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
    protected Transform playerTransform;
    private Movement playerMovement;

    [Header("Parent Movement Settings")]
    [SerializeField] protected float moveSpeed = 0.3f;
    [SerializeField] protected float speedMultiplier = 4f;
    [SerializeField] protected float accelerationTime = 2.5f;
    protected float angle = 0f;

    [Header("Range Settings")]
    [SerializeField] protected float minDistance = 0.1f;
    [SerializeField] private float detectionRange = 2f;
    [SerializeField] private float exclusionRange = 4f;
    [SerializeField] private float minTimeBeforeExclusion = 3f;
    private bool canCheckDistance = false;
    public event Action OnExclusion;

    protected Vector2 playerMovementVector;
    protected Vector2 directionToPlayer;
    protected float distanceToPlayer = 0f;
    protected float initialDistanceToPlayer = 0f;

    protected float currentDirection;
    protected float currentSpeed;
    protected float currentSpeedMultiplier;

    private bool isEnemyFar;
    private bool isEnemyClose;
    private bool isDodging = false;

    protected virtual void Awake()
    {
        playerTransform = GameManager.Instance.player.GetComponent<Transform>();
        isDodging = false;
        canCheckDistance = false;
        currentSpeed = moveSpeed; 
        angle = 0f;
        distanceToPlayer = 0f;
    }

    void OnEnable()
    {
        Invoke("EnableDistanceCheck", minTimeBeforeExclusion);
        initialDistanceToPlayer = GetDistanceToPlayer();
    }

    void Start()
    {
        playerMovement = GameManager.Instance.player.GetComponent<Movement>();
        if (playerMovement != null)
        {
            playerMovement.OnMovementChanged -= HandlePlayerMovementChanged;
            playerMovement.OnMovementChanged += HandlePlayerMovementChanged;
        }
    }

    void Update()
    {
        distanceToPlayer = GetDistanceToPlayer();

        isEnemyFar = distanceToPlayer > exclusionRange;

        if (canCheckDistance && isEnemyFar && OnExclusion != null)
        {
            //Debug.Log("OnExclusion");
            OnExclusion.Invoke();
        }

        isEnemyClose = distanceToPlayer < detectionRange;

        directionToPlayer = GetDirectionToPlayer();
        if (isEnemyClose && !isDodging && !isEnemyFar)
        {
            FollowPlayer();
        }
        if(!isEnemyClose && !isDodging)
        {
            Move();
        }
    }

    protected virtual void FollowPlayer()
    {

    }

    protected virtual void Move()
    {
        
    }

    protected void ChangeCurrentSettings()
    {
        // angle between the direction to the player and the direction of the player
        float angleBetween = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) - Mathf.Atan2(playerMovementVector.y, playerMovementVector.x);

        // normalize the angle between -pi and pi
        angleBetween = Mathf.Atan2(Mathf.Sin(angleBetween), Mathf.Cos(angleBetween));

        currentDirection = (angleBetween > 0) ? 1f : -1f;
        currentSpeedMultiplier = (angleBetween > 0) ? speedMultiplier : 1f;
        currentSpeed = Mathf.Lerp(currentSpeed, moveSpeed * currentSpeedMultiplier, Time.time / accelerationTime);

    }

    private float GetDistanceToPlayer()
    {
        return Vector2.Distance((Vector2)transform.position, (Vector2)playerTransform.position);
    }

    private Vector3 GetDirectionToPlayer()
    {
        return (playerTransform.position - transform.position).normalized;
    }

    void EnableDistanceCheck()
    {
        canCheckDistance = true;
    }

    void HandlePlayerMovementChanged(Vector2 movementVector)
    {
        playerMovementVector = movementVector.normalized;
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