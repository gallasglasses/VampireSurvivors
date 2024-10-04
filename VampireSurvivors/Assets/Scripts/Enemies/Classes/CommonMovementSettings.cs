using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CommonMovementSettings
{
    [SerializeField] private float _moveSpeed = 0.3f;
    [SerializeField] private float _speedMultiplier = 4f;
    [SerializeField] private float _accelerationTime = 2.5f;
    [SerializeField] private EMovementType _moveType;
    [SerializeField] private EMovementType _followType;
    [SerializeField] private EActionType _actionType;
    [SerializeField] private Animator _animator;
    private float _angle = 0f;

    private Vector2 _playerMovementVector;
    private Vector2 _directionToPlayer;

    private float _distanceToPlayer = 0f;
    private float _initialDistanceToPlayer = 0f;

    private float _currentDirection;
    private float _currentSpeed;
    private float _currentSpeedMultiplier;

    private Transform _playerTransform;
    private Movement _playerMovement;

    private bool _isInMovingMode = false;

    public void Initialize(
        float MoveSpeed,
        float SpeedMultiplier,
        float AccelerationTime,
        EMovementType MoveType,
        EMovementType FollowType,
        EActionType ActionType,
        Animator Animator)
    {
        this.MoveSpeed = MoveSpeed;
        this.SpeedMultiplier = SpeedMultiplier;
        this.AccelerationTime = AccelerationTime;
        this.MoveType = MoveType;
        this.FollowType = FollowType;
        this.ActionType = ActionType;
        this.Animator = Animator;
    }

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
    public Animator Animator
    {
        get => _animator;
        set => _animator = value;
    }
    public float Angle
    {
        get => _angle;
        set => _angle = value;
    }
    public Vector2 PlayerMovementVector
    {
        get => _playerMovementVector;
        set => _playerMovementVector = value;
    }
    public Vector2 DirectionToPlayer
    {
        get => _directionToPlayer;
        set => _directionToPlayer = value;
    }
    public float DistanceToPlayer
    {
        get => _distanceToPlayer;
        set => _distanceToPlayer = value;
    }
    public float InitialDistanceToPlayer
    {
        get => _initialDistanceToPlayer;
        set => _initialDistanceToPlayer = value;
    }
    public float CurrentDirection
    {
        get => _currentDirection;
        set => _currentDirection = value;
    }
    public float CurrentSpeed
    {
        get => _currentSpeed;
        set => _currentSpeed = value;
    }
    public float CurrentSpeedMultiplier
    {
        get => _currentSpeedMultiplier;
        set => _currentSpeedMultiplier = value;
    }
    public Transform PlayerTransform
    {
        get => _playerTransform;
        set => _playerTransform = value;
    }
    public Movement PlayerMovement
    {
        get => _playerMovement;
        set => _playerMovement = value;
    }
    public bool IsInMovingMode
    {
        get => _isInMovingMode;
        set => _isInMovingMode = value;
    }
}
