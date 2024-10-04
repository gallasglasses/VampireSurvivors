using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovementBehavior
{
    void Initialize(ScriptableObject settings);
    void Move(CommonMovementSettings commonMovementSettings, RangeMovementSettings rangeMovementSettings, Transform transform);
}

public class SpiralMovement : IMovementBehavior
{
    [Header("Spiral Movement Settings")]
    [SerializeField]
    private SpiralMovementSettings spiralMovementSettings = new();
    public SpiralMovementSettings SpiralMovementSettings
    {
        get { return spiralMovementSettings; }
    }

    public void Initialize(ScriptableObject settings)
    {
        spiralMovementSettings.Initialize(((SpiralMovementSettingsData)settings).SpiralTightnessRange, ((SpiralMovementSettingsData)settings).SpiralRadiusIncreaseTime);
    }

    public void Move(CommonMovementSettings commonMovementSettings, RangeMovementSettings rangeMovementSettings, Transform transform)
    {
        //Spiral
        float maxDistance = Mathf.Max(rangeMovementSettings.MinDistance, commonMovementSettings.DistanceToPlayer);
        float t = Mathf.InverseLerp(rangeMovementSettings.MinDistance, commonMovementSettings.InitialDistanceToPlayer, maxDistance);
        float spiralTightness = Mathf.Lerp(spiralMovementSettings.SpiralTightnessRange.y, spiralMovementSettings.SpiralTightnessRange.x, t);

        float spiralRadiusInterpolation = Mathf.Clamp01(Time.time / spiralMovementSettings.SpiralRadiusIncreaseTime);
        float initialRadius = Mathf.Lerp(rangeMovementSettings.MinDistance, commonMovementSettings.DistanceToPlayer - spiralTightness, spiralRadiusInterpolation);

        commonMovementSettings.Angle += commonMovementSettings.CurrentSpeed * Time.deltaTime * commonMovementSettings.CurrentDirection;
        commonMovementSettings.Angle = Mathf.Repeat(commonMovementSettings.Angle, Mathf.PI * 2f);

        Vector2 spiralPosition = new Vector2(
            commonMovementSettings.PlayerTransform.position.x + Mathf.Cos(commonMovementSettings.Angle) * initialRadius,
            commonMovementSettings.PlayerTransform.position.y + Mathf.Sin(commonMovementSettings.Angle) * initialRadius
        );

        transform.position = Vector2.Lerp(transform.position, spiralPosition, Time.deltaTime * commonMovementSettings.AccelerationTime);
    }
}

public class WaveMovement : IMovementBehavior
{
    [Header("Wave Movement Settings")]
    [SerializeField]
    private WaveMovementSettings waveMovementSettings = new();
    public WaveMovementSettings WaveMovementSettings
    {
        get { return waveMovementSettings; }
    }

    public void Initialize(ScriptableObject settings)
    {
        waveMovementSettings.WaveAmplitudeRange = ((WaveMovementSettingsData)settings).WaveAmplitudeRange;
        waveMovementSettings.WaveFrequency = ((WaveMovementSettingsData)settings).WaveFrequency;
        waveMovementSettings.AngleRange = ((WaveMovementSettingsData)settings).AngleRange;
    }

    public void Move(CommonMovementSettings commonMovementSettings, RangeMovementSettings rangeMovementSettings, Transform transform)
    {
        //Wave
        if (!commonMovementSettings.IsInMovingMode)
        {
            float randomDeviation = Random.Range(-waveMovementSettings.AngleRange, waveMovementSettings.AngleRange);

            waveMovementSettings.DeviatedDirection = Quaternion.Euler(0, 0, randomDeviation) * commonMovementSettings.DirectionToPlayer;
            waveMovementSettings.PerpendicularDirection = new Vector2(-commonMovementSettings.DirectionToPlayer.y, commonMovementSettings.DirectionToPlayer.x);

            commonMovementSettings.IsInMovingMode = true;

        }
        Vector2 newDirection = (waveMovementSettings.DeviatedDirection + waveMovementSettings.PerpendicularDirection * GetWaveOffset(commonMovementSettings, rangeMovementSettings)).normalized;

        transform.position += (Vector3)newDirection * commonMovementSettings.MoveSpeed * Time.deltaTime;
    }

    private float GetWaveOffset(CommonMovementSettings commonMovementSettings, RangeMovementSettings rangeMovementSettings)
    {
        float maxDistance = Mathf.Max(rangeMovementSettings.MinDistance, commonMovementSettings.DistanceToPlayer);
        float t = Mathf.InverseLerp(rangeMovementSettings.MinDistance, commonMovementSettings.InitialDistanceToPlayer, maxDistance);
        float waveAmplitudeLerp = Mathf.Lerp(waveMovementSettings.WaveAmplitudeRange.y, waveMovementSettings.WaveAmplitudeRange.x, t);
        return Mathf.Sin(Time.time * waveMovementSettings.WaveFrequency) * waveAmplitudeLerp;
    }
}

public class CircleMovement : IMovementBehavior
{
    [Header("Circle Movement Settings")]
    [SerializeField]
    private CircleMovementSettings circleMovementSettings = new();
    public CircleMovementSettings CircleMovementSettings
    {
        get { return circleMovementSettings; }
    }

    public void Initialize(ScriptableObject settings)
    {
        circleMovementSettings.CircleRadius = ((CircleMovementSettingsData)settings).CircleRadius;
    }

    public void Move(CommonMovementSettings commonMovementSettings, RangeMovementSettings rangeMovementSettings, Transform transform)
    {
        //Circle
        commonMovementSettings.Angle += commonMovementSettings.MoveSpeed * commonMovementSettings.SpeedMultiplier * Time.deltaTime;
        commonMovementSettings.Angle = Mathf.Repeat(commonMovementSettings.Angle, Mathf.PI * 2f);

        Vector3 circularPosition = new Vector3(
            commonMovementSettings.PlayerTransform.position.x + Mathf.Cos(commonMovementSettings.Angle) * circleMovementSettings.CircleRadius,
            commonMovementSettings.PlayerTransform.position.y + Mathf.Sin(commonMovementSettings.Angle) * circleMovementSettings.CircleRadius,
            transform.position.z
        );

        transform.position = Vector3.Lerp(transform.position, circularPosition, commonMovementSettings.MoveSpeed * commonMovementSettings.SpeedMultiplier * Time.deltaTime);
    }
}

public class StraightMovement : IMovementBehavior
{
    public void Initialize(ScriptableObject settings)
    {
        
    }

    public void Move(CommonMovementSettings commonMovementSettings, RangeMovementSettings rangeMovementSettings, Transform transform)
    {
        //Straight
        transform.position += (Vector3)commonMovementSettings.DirectionToPlayer * commonMovementSettings.CurrentSpeed * Time.deltaTime;
    }
}