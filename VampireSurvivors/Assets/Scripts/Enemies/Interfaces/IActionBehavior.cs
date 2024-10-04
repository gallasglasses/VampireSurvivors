using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActionBehavior
{
    void Initialize(ScriptableObject settings);
    void Move(CommonMovementSettings commonMovementSettings, RangeMovementSettings rangeMovementSettings, Transform transform);
}

public class CircleAction : IActionBehavior
{
    [Header("Circle Action Settings")]
    [SerializeField]
    private CircleMovementSettings circleMovementSettings = new();
    public CircleMovementSettings CircleMovementSettings
    {
        get { return circleMovementSettings; }
    }

    public void Initialize(ScriptableObject settings)
    {
        circleMovementSettings.CircleRadius = ((CircleActionSettingsData)settings).CircleRadius;
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

public class DashAction : IActionBehavior
{
    [Header("Dash Movement Settings")]
    [SerializeField]
    private DashActionSettings dashActionSettings = new();
    public DashActionSettings DashActionSettings
    {
        get { return dashActionSettings; }
    }

    public void Initialize(ScriptableObject settings)
    {
        dashActionSettings.DashInterval = ((DashActionSettingsData)settings).DashInterval;
        dashActionSettings.DashDuration = ((DashActionSettingsData)settings).DashDuration;
    }

    public void Move(CommonMovementSettings commonMovementSettings, RangeMovementSettings rangeMovementSettings, Transform transform)
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

            transform.position = Vector3.Lerp(transform.position, commonMovementSettings.PlayerTransform.position + offset, commonMovementSettings.MoveSpeed * Time.deltaTime);

            if (dashActionSettings.DashTimer >= dashActionSettings.DashInterval)
            {
                dashActionSettings.DashStartPosition = transform.position;
                Vector3 directionThroughPlayer = (transform.position - commonMovementSettings.PlayerTransform.position).normalized;
                dashActionSettings.DashEndPosition = commonMovementSettings.PlayerTransform.position - directionThroughPlayer * rangeMovementSettings.MinDistance;

                dashActionSettings.IsDashing = true;
                dashActionSettings.DashTimer = 0f;
            }
        }
    }
}