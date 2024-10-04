using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveMovementSettingsData", menuName = "Movement/WaveMovementSettings")]
public class WaveMovementSettingsData : ScriptableObject
{
    public Vector2 WaveAmplitudeRange;
    public float WaveFrequency;
    public float AngleRange;
}
