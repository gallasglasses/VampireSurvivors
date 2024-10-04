using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpiralMovementSettings
{
    [SerializeField] private Vector2 _spiralTightnessRange;
    [SerializeField] private float _spiralRadiusIncreaseTime;
    public void Initialize(
        Vector2 SpiralTightnessRange,
        float SpiralRadiusIncreaseTime)
    {
        this.SpiralTightnessRange = SpiralTightnessRange;
        this.SpiralRadiusIncreaseTime = SpiralRadiusIncreaseTime;
    }

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