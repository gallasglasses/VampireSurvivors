using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyMovementData", menuName = "Enemies/EnemyMovementData")]
public class EnemyDodgeData : ScriptableObject
{
    public float dodgeSpeed = 2f;
    public float dodgeDistance = 1f;
    public float dodgeDuration = 0.2f;
    public float detectionRadius = 0.5f;
    public EnemyMovement enemyMovement;
    public LayerMask projectileLayer;
}
