using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpiralChasingSettingsData", menuName = "Movement/SpiralChasingSettings")]
public class SpiralChasingSettingsData : ScriptableObject
{
    public Vector2 SpiralTightnessRange;
    public float SpiralRadiusIncreaseTime;
}
