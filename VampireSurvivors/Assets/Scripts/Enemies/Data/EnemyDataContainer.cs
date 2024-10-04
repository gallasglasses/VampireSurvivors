using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/EnemyDataContainer")]
public class EnemyDataContainer : ScriptableObject
{
    public EnemyData enemyData;
    public EnemyMovementData enemyMovementData;
    public EnemyDodgeData enemyDodgeData;
}
