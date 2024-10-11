using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponReward", menuName = "Rewards/WeaponReward")]
public class WeaponReward : RewardObject
{
    //for all WeaponType
    public bool isDefault;
    public Sprite weaponSprite;
    public float cooldown;
    public float damage;

    public WeaponType weaponType;

    //++++++++++++++++++++++++++++++++++++++++++++++++++++
    //if WeaponType.AURA

    public AuraType auraType;

    public float radiusOfEffect;

    //++++++++++++++++++++++++++++++++++++++++++++++++++++
    //if WeaponType.PROJECTILE

    public ProjectileType projectileType;
    public Projectile projectilePrefab;
    public ProjectileDirectionType projectileDirectionType;

    public ProjectileAngleStepType projectileAngleStepType; // for additionalProjectiles

    public float projectileAngleStepValue; // if ProjectileAngleStepType.VALUE
    public float timeBetweenShots; // if ProjectileAngleStepType.NONE

    public float projectileForce;
    public int projectileCount;
    public float projectileDeactivationTime;

}