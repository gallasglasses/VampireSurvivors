using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Enemies/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string enemyType;
    public Sprite enemySprite;
    public AnimatorController enemyAnimatorController;
    public float maxHealth;
    public TypeXPGem typeXPGem;
    public float damage;
    public ParticleSystem bloodEffect;
}
