using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    public void Initialize(
        float DashInterval,
        float DashDuration)
    {
        this.DashInterval = DashInterval;
        this.DashDuration = DashDuration;
    }
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
