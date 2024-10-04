using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveMovementSettings
{
    [SerializeField] private Vector2 _waveAmplitudeRange;
    [SerializeField] private float _waveFrequency;
    [SerializeField] private float _angleRange;
    private Vector2 _perpendicularDirection = Vector2.zero;
    private Vector2 _deviatedDirection;
    public void Initialize(
        Vector2 WaveAmplitudeRange,
        float WaveFrequency,
        float AngleRange)
    {
        this.WaveAmplitudeRange = WaveAmplitudeRange;
        this.WaveFrequency = WaveFrequency;
        this.AngleRange = AngleRange;
    }
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
