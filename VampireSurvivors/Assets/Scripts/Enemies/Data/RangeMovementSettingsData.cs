using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeMovementSettingsData : ScriptableObject
{
    public float _minDistance = 0.1f;
    public float _detectionRange = 2f;
    public float _exclusionRange = 4f;
    public float _minTimeBeforeExclusion = 3f;
}
