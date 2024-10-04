using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveChasingSettingsData", menuName = "Movement/WaveChasingSettings")]
public class WaveChasingSettingsData : ScriptableObject
{
    public Vector2 WaveAmplitudeRange;
    public float WaveFrequency;
    public float AngleRange;
}
