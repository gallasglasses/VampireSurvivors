using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CircleMovementSettings
{
    [SerializeField] private float _circleRadius;
    public void Initialize(
        float CircleRadius)
    {
        this.CircleRadius = CircleRadius;
    }
    public float CircleRadius
    {
        get => _circleRadius;
        set => _circleRadius = value;
    }
}