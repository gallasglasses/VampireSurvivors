using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpiralMovementSettingsData", menuName = "Movement /SpiralMovementSettings")]
public class SpiralMovementSettingsData : ScriptableObject
{
    public Vector2 SpiralTightnessRange;
    public float SpiralRadiusIncreaseTime;
}
