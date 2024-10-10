//using System;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.U2D;
using static UnityEngine.UI.ScrollRect;
using Random = UnityEngine.Random;

public class EnemyMovement : GameplayMonoBehaviour
{
    [Header("Common Movement Settings")]
    [SerializeField]
    private CommonMovementSettings commonMovementSettings = new CommonMovementSettings();
    public CommonMovementSettings CommonMovementSettings
    {
        get { return commonMovementSettings; }
    }

    [Header("Range Movement Settings")]
    [SerializeField]
    private RangeMovementSettings rangeMovementSettings = new RangeMovementSettings();
    public RangeMovementSettings RangeMovementSettings
    {
        get { return rangeMovementSettings; }
    }

    [SerializeField] private EnemyMovementData _enemyMovementData;
    public EnemyMovementData EnemyMovementData
    {
        set => _enemyMovementData = value;
    }

    private IMovementBehavior movementBehavior;
    private IChasingBehavior chasingBehavior;
    private IActionBehavior actionBehavior;

    private bool isEnemyFar;
    private bool isEnemyClose;
    private bool isDodging = false;
    private bool isInFollowingMode = true;

    protected override void Awake()
    {
        base.Awake();
        isInFollowingMode = true;
        commonMovementSettings.IsInMovingMode = false;
        //+++++++++++
        //waveMovementSettings.PerpendicularDirection = Vector2.zero;
        //+++++++++++
        isDodging = false;
        rangeMovementSettings.CanCheckDistance = false;
        commonMovementSettings.CurrentSpeed = commonMovementSettings.MoveSpeed;
        commonMovementSettings.Angle = 0f;
        commonMovementSettings.DistanceToPlayer = 0f;
    }

    void OnEnable()
    {
        if(commonMovementSettings.PlayerTransform != null)
        {
            commonMovementSettings.InitialDistanceToPlayer = GetDistanceToPlayer();
        }
    }

    void Start()
    {
        commonMovementSettings.PlayerMovement = GameManager.Instance.player.GetComponent<Movement>();
        if (commonMovementSettings.PlayerMovement != null)
        {
            commonMovementSettings.PlayerMovement.OnMovementChanged -= HandlePlayerMovementChanged;
            commonMovementSettings.PlayerMovement.OnMovementChanged += HandlePlayerMovementChanged;
        }
        originalAnimationSpeed = commonMovementSettings.Animator.speed;
        Invoke("EnableDistanceCheck", rangeMovementSettings.MinTimeBeforeExclusion);

        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance is null!");
        }
        else if (GameManager.Instance.player == null)
        {
            Debug.LogError("GameManager.Instance.player is null!");
        }
        if (GameManager.Instance != null)
        {
            commonMovementSettings.PlayerTransform = GameManager.Instance.player.GetComponent<Transform>();
            commonMovementSettings.InitialDistanceToPlayer = GetDistanceToPlayer();
        }

        movementBehavior = GetMovementBehavior(commonMovementSettings.MoveType);
        chasingBehavior = GetChasingBehavior(commonMovementSettings.FollowType);
        actionBehavior = GetActionBehavior(commonMovementSettings.ActionType);
    }

    protected override void PausableUpdate()
    {
        base.UnPausableUpdate();
        if (commonMovementSettings.Animator != null)
        {
            commonMovementSettings.Animator.speed = 0f;
        }
    }

    protected override void UnPausableUpdate()
    {
        base.UnPausableUpdate();
        if (commonMovementSettings.Animator != null)
        {
            commonMovementSettings.Animator.speed = originalAnimationSpeed;
        }
        commonMovementSettings.DistanceToPlayer = GetDistanceToPlayer();

        isEnemyFar = commonMovementSettings.DistanceToPlayer > rangeMovementSettings.ExclusionRange;

        if (rangeMovementSettings.CanCheckDistance && isEnemyFar)
        {
            //Debug.Log("OnExclusion");
            rangeMovementSettings.TriggerExclusion();
        }

        isEnemyClose = commonMovementSettings.DistanceToPlayer < rangeMovementSettings.DetectionRange;

        commonMovementSettings.DirectionToPlayer = GetDirectionToPlayer();
        if (isEnemyClose && !isDodging && !isEnemyFar)
        {
            FollowPlayer();
        }
        if (!isEnemyClose && !isDodging)
        {
            Move();
        }
    }

    protected virtual void FollowPlayer()
    {
        commonMovementSettings.IsInMovingMode = false;
        isInFollowingMode = commonMovementSettings.DistanceToPlayer > rangeMovementSettings.MinDistance;
        if (isInFollowingMode)
        {
            //+++++++++++++
            //dashActionSettings.IsDashing = false;
            //+++++++++++++

            ChangeCurrentSettings();
            
            chasingBehavior.Move(commonMovementSettings, rangeMovementSettings, transform);
        }
        else
        {
            actionBehavior.Move(commonMovementSettings, rangeMovementSettings, transform);
        }
    }

    protected virtual void Move()
    {
        movementBehavior.Move(commonMovementSettings, rangeMovementSettings, transform);
    }

    private IMovementBehavior GetMovementBehavior(EMovementType moveType)
    {
        switch (moveType)
        {
            case EMovementType.SPIRAL:
                var spiralMovement = new SpiralMovement();
                spiralMovement.Initialize(_enemyMovementData.movementSettings);
                return spiralMovement;
            case EMovementType.WAVE:
                var waveMovement = new WaveMovement();
                waveMovement.Initialize(_enemyMovementData.movementSettings);
                return waveMovement;
            case EMovementType.CIRCLE:
                var circleMovement = new CircleMovement();
                circleMovement.Initialize(_enemyMovementData.movementSettings);
                return circleMovement;
            case EMovementType.STRAIGHT:
                var straightMovement = new StraightMovement();
                straightMovement.Initialize(_enemyMovementData.movementSettings);
                return straightMovement;
            default:
                return null;
        }
    }

    private IChasingBehavior GetChasingBehavior(EMovementType followType)
    {
        switch (followType)
        {
            case EMovementType.SPIRAL:
                var spiralMovement = new SpiralChasing();
                spiralMovement.Initialize(_enemyMovementData.chasingSettings);
                return spiralMovement;
            case EMovementType.WAVE:
                var waveMovement = new WaveChasing();
                waveMovement.Initialize(_enemyMovementData.chasingSettings);
                return waveMovement;
            case EMovementType.CIRCLE:
                var circleMovement = new CircleChasing();
                circleMovement.Initialize(_enemyMovementData.chasingSettings);
                return circleMovement;
            case EMovementType.STRAIGHT:
                var straightMovement = new StraightChasing();
                straightMovement.Initialize(_enemyMovementData.chasingSettings);
                return straightMovement;
            default:
                return null;
        }
    }

    private IActionBehavior GetActionBehavior(EActionType actionType)
    {
        switch (actionType)
        {
            case EActionType.SURROUND:
                var circleAction = new CircleAction();
                circleAction.Initialize(_enemyMovementData.actionSettings);
                return circleAction;
            case EActionType.DASH:
                var dashAction = new DashAction();
                dashAction.Initialize(_enemyMovementData.actionSettings);
                return dashAction;
            default:
                return null;
        }
    }

    protected void ChangeCurrentSettings()
    {
        // angle between the direction to the player and the direction of the player
        float angleBetween = Mathf.Atan2(commonMovementSettings.DirectionToPlayer.y, commonMovementSettings.DirectionToPlayer.x) - Mathf.Atan2(commonMovementSettings.PlayerMovementVector.y, commonMovementSettings.PlayerMovementVector.x);

        // normalize the angle between -pi and pi
        angleBetween = Mathf.Atan2(Mathf.Sin(angleBetween), Mathf.Cos(angleBetween));

        commonMovementSettings.CurrentDirection = (angleBetween > 0) ? 1f : -1f;
        commonMovementSettings.CurrentSpeedMultiplier = (angleBetween > 0) ? commonMovementSettings.SpeedMultiplier : 1f;
        commonMovementSettings.CurrentSpeed = Mathf.Lerp(commonMovementSettings.CurrentSpeed, commonMovementSettings.MoveSpeed * commonMovementSettings.CurrentSpeedMultiplier, Time.time / commonMovementSettings.AccelerationTime);
    }

    private float GetDistanceToPlayer()
    {
        return Vector2.Distance((Vector2)transform.position, (Vector2)commonMovementSettings.PlayerTransform.position);
    }

    private Vector3 GetDirectionToPlayer()
    {
        return (commonMovementSettings.PlayerTransform.position - transform.position).normalized;
    }

    void EnableDistanceCheck()
    {
        rangeMovementSettings.CanCheckDistance = true;
    }

    void HandlePlayerMovementChanged(Vector2 movementVector)
    {
        commonMovementSettings.PlayerMovementVector = movementVector.normalized;
    }

    public void Dodge(bool canDodge)
    {
        isDodging = canDodge;
    }

    private void OnDestroy()
    {
        commonMovementSettings.PlayerMovement.OnMovementChanged -= HandlePlayerMovementChanged;
    }
}