using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class RangeMovementSettings
{
    [SerializeField] private float _minDistance = 0.1f;
    [SerializeField] private float _detectionRange = 2f;
    [SerializeField] private float _exclusionRange = 4f;
    [SerializeField] private float _minTimeBeforeExclusion = 3f;
    private bool _canCheckDistance = false;
    public event Action OnExclusion;
    public void Initialize(
        float MinDistance,
        float DetectionRange,
        float ExclusionRange,
        float MinTimeBeforeExclusion)
    {
        this.MinDistance = MinDistance;
        this.DetectionRange = DetectionRange;
        this.ExclusionRange = ExclusionRange;
        this.MinTimeBeforeExclusion = MinTimeBeforeExclusion;
    }

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