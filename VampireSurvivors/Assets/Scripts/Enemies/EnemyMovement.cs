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

public enum EMovementType
{
    SPIRAL,
    WAVE,
    CIRCLE,
    STRAIGHT
}

public enum EActionType
{
    DASH,
    SURROUND
}


[System.Serializable]
public class CommonMovementSettings
{
    [FormerlySerializedAs("MoveSpeed")]
    [SerializeField] private float _moveSpeed = 0.3f;
    [SerializeField] private float _speedMultiplier = 4f;
    [SerializeField] private float _accelerationTime = 2.5f;
    [FormerlySerializedAs("MoveType")]
    [SerializeField] private EMovementType _moveType;
    [SerializeField] private EMovementType _followType;
    [SerializeField] private EActionType _actionType;
    private float _angle = 0f;
    
    public float MoveSpeed
    {
        get => _moveSpeed;
        set => _moveSpeed = value;
    }
    public float SpeedMultiplier
    {
        get => _speedMultiplier;
        set => _speedMultiplier = value;
    }
    public float AccelerationTime
    {
        get => _accelerationTime;
        set => _accelerationTime = value;
    }

    public EMovementType MoveType
    {
        get => _moveType;
        set => _moveType = value;
    }
    public EMovementType FollowType
    {
        get => _followType;
        set => _followType = value;
    }
    public EActionType ActionType
    {
        get => _actionType;
        set => _actionType = value;
    }
    public float Angle
    {
        get => _angle;
        set => _angle = value;
    }
}

[System.Serializable]
public class RangeMovementSettings
{
    [SerializeField] private float _minDistance = 0.1f;
    [SerializeField] private float _detectionRange = 2f;
    [SerializeField] private float _exclusionRange = 4f;
    [SerializeField] private float _minTimeBeforeExclusion = 3f;
    private bool _canCheckDistance = false;
    public event Action OnExclusion;

    public float MinDistance
    {
        get => _minDistance;
        set => _minDistance = value;
    }
    public float DetectionRange
    {
        get => _detectionRange;
        set => _detectionRange = value;
    }
    public float ExclusionRange
    {
        get => _exclusionRange;
        set => _exclusionRange = value;
    }
    public float MinTimeBeforeExclusion
    {
        get => _minTimeBeforeExclusion;
        set => _minTimeBeforeExclusion = value;
    }
    public bool CanCheckDistance
    {
        get => _canCheckDistance;
        set => _canCheckDistance = value;
    }
    public void TriggerExclusion()
    {
        if (OnExclusion != null)
            OnExclusion?.Invoke();
    }
}

[System.Serializable]
public class SpiralMovementSettings
{
    public Vector2 _spiralTightnessRange;
    public float _spiralRadiusIncreaseTime;

    public Vector2 SpiralTightnessRange
    {
        get => _spiralTightnessRange;
        set => _spiralTightnessRange = value;
    }
    public float SpiralRadiusIncreaseTime
    {
        get => _spiralRadiusIncreaseTime;
        set => _spiralRadiusIncreaseTime = value;
    }
}

[System.Serializable]
public class WaveMovementSettings
{
    [SerializeField] private Vector2 _waveAmplitudeRange;
    [SerializeField] private float _waveFrequency;
    [SerializeField] private float _angleRange;
    private Vector2 _perpendicularDirection = Vector2.zero;
    private Vector2 _deviatedDirection;

    public Vector2 WaveAmplitudeRange
    {
        get => _waveAmplitudeRange;
        set => _waveAmplitudeRange = value;
    }
    public float WaveFrequency
    {
        get => _waveFrequency;
        set => _waveFrequency = value;
    }
    public float AngleRange
    {
        get => _angleRange;
        set => _angleRange = value;
    }
    public Vector2 PerpendicularDirection
    {
        get => _perpendicularDirection;
        set => _perpendicularDirection = value;
    }
    public Vector2 DeviatedDirection
    {
        get => _deviatedDirection;
        set => _deviatedDirection = value;
    }
}

[System.Serializable]
public class CircleMovementSettings
{
    [SerializeField] private float _circleRadius;
    public float CircleRadius
    {
        get => _circleRadius;
        set => _circleRadius = value;
    }
}

[System.Serializable]
public class DashActionSettings
{
    [SerializeField] private float _dashInterval;
    [SerializeField] private float _dashDuration;
    private Vector3 _dashStartPosition;
    private Vector3 _dashEndPosition;
    private bool _isDashing = false;
    private float _dashTimer;
    private float _dashTime;

    public float DashInterval
    {
        get => _dashInterval;
        set => _dashInterval = value;
    }
    public float DashDuration
    {
        get => _dashDuration;
        set => _dashDuration = value;
    }
    public Vector3 DashStartPosition
    {
        get => _dashStartPosition;
        set => _dashStartPosition = value;
    }
    public Vector3 DashEndPosition
    {
        get => _dashEndPosition;
        set => _dashEndPosition = value;
    }
    public bool IsDashing
    {
        get => _isDashing;
        set => _isDashing = value;
    }
    public float DashTimer
    {
        get => _dashTimer;
        set => _dashTimer = value;
    }
    public float DashTime
    {
        get => _dashTime;
        set => _dashTime = value;
    }
}

public class EnemyMovement : GameplayMonoBehaviour
{
    protected Transform playerTransform;
    private Movement playerMovement;
    public Animator animator;

    [Header("Common Movement Settings")]
    [SerializeField]
    private CommonMovementSettings commonMovementSettings = new CommonMovementSettings();

    [Header("Range Movement Settings")]
    [SerializeField]
    private RangeMovementSettings rangeMovementSettings = new RangeMovementSettings();
    public RangeMovementSettings RangeMovementSettings
    {
        get { return rangeMovementSettings; }
    }

    [Header("Wave Movement Settings")]
    [SerializeField]
    private WaveMovementSettings waveMovementSettings = new WaveMovementSettings();

    [Header("Dash Movement Settings")]
    [SerializeField]
    private DashActionSettings dashActionSettings = new DashActionSettings();

    [Header("Spiral Movement Settings")]
    [SerializeField]
    private SpiralMovementSettings spiralMovementSettings = new SpiralMovementSettings();

    [Header("Circle Movement Settings")]
    [SerializeField]
    private CircleMovementSettings circleMovementSettings = new CircleMovementSettings();

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
    private bool isInMovingMode = false;
    private bool isInFollowingMode = true;

    protected override void Awake()
    {
        base.Awake();
        playerTransform = GameManager.Instance.player.GetComponent<Transform>();
        isInFollowingMode = true;
        isInMovingMode = false;
        waveMovementSettings.PerpendicularDirection = Vector2.zero;
        isDodging = false;
        rangeMovementSettings.CanCheckDistance = false;
        currentSpeed = commonMovementSettings.MoveSpeed;
        commonMovementSettings.Angle = 0f;
        distanceToPlayer = 0f;
    }

    void OnEnable()
    {
        Invoke("EnableDistanceCheck", rangeMovementSettings.MinTimeBeforeExclusion);
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
        originalAnimationSpeed = animator.speed;
    }

    protected override void PausableUpdate()
    {
        base.UnPausableUpdate();
        if (animator != null)
        {
            animator.speed = 0f;
        }
    }

    protected override void UnPausableUpdate()
    {
        base.UnPausableUpdate();
        if (animator != null)
        {
            animator.speed = originalAnimationSpeed;
        }
        distanceToPlayer = GetDistanceToPlayer();

        isEnemyFar = distanceToPlayer > rangeMovementSettings.ExclusionRange;

        if (rangeMovementSettings.CanCheckDistance && isEnemyFar)
        {
            //Debug.Log("OnExclusion");
            rangeMovementSettings.TriggerExclusion();
        }

        isEnemyClose = distanceToPlayer < rangeMovementSettings.DetectionRange;

        directionToPlayer = GetDirectionToPlayer();
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
        isInMovingMode = false;
        isInFollowingMode = distanceToPlayer > rangeMovementSettings.MinDistance;
        if (isInFollowingMode)
        {
            dashActionSettings.IsDashing = false;
            ChangeCurrentSettings();
            switch (commonMovementSettings.FollowType)
            {
                case EMovementType.SPIRAL:
                    MoveInSpiral(); 
                    break;
                case EMovementType.WAVE: 
                    MoveInWave(); 
                    break;
                case EMovementType.CIRCLE:
                    MoveInCircle();
                    break;
                case EMovementType.STRAIGHT:
                    MoveInStraight();
                    break;
                default:
                    break;

            }
        }
        else
        {
            switch (commonMovementSettings.ActionType)
            {
                case EActionType.DASH:
                    DashToPlayer();
                    break;
                case EActionType.SURROUND:
                    MoveInCircle();
                    break;
                default:
                    break;
            }
        }
    }

    protected virtual void Move()
    {
        switch (commonMovementSettings.MoveType)
        {
            case EMovementType.SPIRAL:
                MoveInSpiral();
                break;
            case EMovementType.WAVE:
                MoveInWave();
                break;
            case EMovementType.CIRCLE:
                MoveInCircle();
                break;
            case EMovementType.STRAIGHT:
                MoveInStraight();
                break;
            default:
                break;

        }
    }

    private void DashToPlayer()
    {
        dashActionSettings.DashTimer += Time.deltaTime;

        if (dashActionSettings.IsDashing)
        {
            dashActionSettings.DashTime += Time.deltaTime;

            transform.position = Vector3.Lerp(dashActionSettings.DashStartPosition, dashActionSettings.DashEndPosition, dashActionSettings.DashTime / dashActionSettings.DashDuration);

            if (dashActionSettings.DashTime >= dashActionSettings.DashDuration)
            {
                dashActionSettings.IsDashing = false;
                dashActionSettings.DashTime = 0f;
            }
        }
        else
        {
            Vector3 offset = new Vector3(Mathf.Cos(Time.time * commonMovementSettings.MoveSpeed), Mathf.Sin(Time.time * commonMovementSettings.MoveSpeed), 0) * rangeMovementSettings.MinDistance;

            transform.position = Vector3.Lerp(transform.position, playerTransform.position + offset, commonMovementSettings.MoveSpeed * Time.deltaTime);

            if (dashActionSettings.DashTimer >= dashActionSettings.DashInterval)
            {
                dashActionSettings.DashStartPosition = transform.position;
                Vector3 directionThroughPlayer = (transform.position - playerTransform.position).normalized;
                dashActionSettings.DashEndPosition = playerTransform.position - directionThroughPlayer * rangeMovementSettings.MinDistance;

                dashActionSettings.IsDashing = true;
                dashActionSettings.DashTimer = 0f;
            }
        }
    }

    private void MoveInStraight()
    {
        transform.position += (Vector3)directionToPlayer * currentSpeed * Time.deltaTime;
    }

    protected void MoveInWave()
    {
        if (!isInMovingMode)
        {
            float randomDeviation = Random.Range(-waveMovementSettings.AngleRange, waveMovementSettings.AngleRange);

            waveMovementSettings.DeviatedDirection = Quaternion.Euler(0, 0, randomDeviation) * directionToPlayer;
            waveMovementSettings.PerpendicularDirection = new Vector2(-directionToPlayer.y, directionToPlayer.x);

            isInMovingMode = true;

        }
        Vector2 newDirection = (waveMovementSettings.DeviatedDirection + waveMovementSettings.PerpendicularDirection * GetWaveOffset()).normalized;

        transform.position += (Vector3)newDirection * commonMovementSettings.MoveSpeed * Time.deltaTime;
    }

    protected void MoveInSpiral()
    {
        float maxDistance = Mathf.Max(rangeMovementSettings.MinDistance, distanceToPlayer);
        float t = Mathf.InverseLerp(rangeMovementSettings.MinDistance, initialDistanceToPlayer, maxDistance);
        float spiralTightness = Mathf.Lerp(spiralMovementSettings.SpiralTightnessRange.y, spiralMovementSettings.SpiralTightnessRange.x, t);

        float spiralRadiusInterpolation = Mathf.Clamp01(Time.time / spiralMovementSettings.SpiralRadiusIncreaseTime);
        float initialRadius = Mathf.Lerp(rangeMovementSettings.MinDistance, distanceToPlayer - spiralTightness, spiralRadiusInterpolation);

        commonMovementSettings.Angle += currentSpeed * Time.deltaTime * currentDirection;
        commonMovementSettings.Angle = Mathf.Repeat(commonMovementSettings.Angle, Mathf.PI * 2f);

        Vector2 spiralPosition = new Vector2(
            playerTransform.position.x + Mathf.Cos(commonMovementSettings.Angle) * initialRadius,
            playerTransform.position.y + Mathf.Sin(commonMovementSettings.Angle) * initialRadius
        );

        transform.position = Vector2.Lerp(transform.position, spiralPosition, Time.deltaTime * commonMovementSettings.AccelerationTime);
    }

    void MoveInCircle()
    {
        commonMovementSettings.Angle += commonMovementSettings.MoveSpeed * commonMovementSettings.SpeedMultiplier * Time.deltaTime;
        commonMovementSettings.Angle = Mathf.Repeat(commonMovementSettings.Angle, Mathf.PI * 2f);

        Vector3 circularPosition = new Vector3(
            playerTransform.position.x + Mathf.Cos(commonMovementSettings.Angle) * circleMovementSettings.CircleRadius,
            playerTransform.position.y + Mathf.Sin(commonMovementSettings.Angle) * circleMovementSettings.CircleRadius,
            transform.position.z
        );

        transform.position = Vector3.Lerp(transform.position, circularPosition, commonMovementSettings.MoveSpeed * commonMovementSettings.SpeedMultiplier * Time.deltaTime);
    }

    protected void ChangeCurrentSettings()
    {
        // angle between the direction to the player and the direction of the player
        float angleBetween = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) - Mathf.Atan2(playerMovementVector.y, playerMovementVector.x);

        // normalize the angle between -pi and pi
        angleBetween = Mathf.Atan2(Mathf.Sin(angleBetween), Mathf.Cos(angleBetween));

        currentDirection = (angleBetween > 0) ? 1f : -1f;
        currentSpeedMultiplier = (angleBetween > 0) ? commonMovementSettings.SpeedMultiplier : 1f;
        currentSpeed = Mathf.Lerp(currentSpeed, commonMovementSettings.MoveSpeed * currentSpeedMultiplier, Time.time / commonMovementSettings.AccelerationTime);
    }

    protected float GetWaveOffset()
    {
        float maxDistance = Mathf.Max(rangeMovementSettings.MinDistance, distanceToPlayer);
        float t = Mathf.InverseLerp(rangeMovementSettings.MinDistance, initialDistanceToPlayer, maxDistance);
        float waveAmplitudeLerp = Mathf.Lerp(waveMovementSettings.WaveAmplitudeRange.y, waveMovementSettings.WaveAmplitudeRange.x, t);
        return Mathf.Sin(Time.time * waveMovementSettings.WaveFrequency) * waveAmplitudeLerp;
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
        rangeMovementSettings.CanCheckDistance = true;
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