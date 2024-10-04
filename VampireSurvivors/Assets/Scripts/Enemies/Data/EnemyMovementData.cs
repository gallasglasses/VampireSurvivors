using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyMovementData", menuName = "Enemies/EnemyMovementData")]
public class EnemyMovementData : ScriptableObject
{
    public float moveSpeed = 0.3f;
    public float speedMultiplier = 4f;
    public float accelerationTime = 2.5f;
    public EMovementType moveType;
    public EMovementType followType;
    public EActionType actionType;
    public Animator animator;

    public ScriptableObject movementSettings;
    public ScriptableObject chasingSettings;
    public ScriptableObject actionSettings;
    [SerializeReference] public RangeMovementSettingsData rangeMovementSettingsData;
}
