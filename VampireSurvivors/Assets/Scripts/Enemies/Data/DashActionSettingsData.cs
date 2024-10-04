using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DashActionSettingsData", menuName = "Movement/DashActionSettings")]
public class DashActionSettingsData : ScriptableObject
{
    public float DashInterval;
    public float DashDuration;
}
