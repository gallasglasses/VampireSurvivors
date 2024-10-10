using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponReward", menuName = "Rewards/WeaponReward")]
public class WeaponReward : RewardObject
{
    //for all WeaponType
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
    public ProjectileDirectionType projectileDirectionType;

    public ProjectileAngleStepType projectileAngleStepType; // for additionalProjectiles

    public float projectileAngleStepValue; // if ProjectileAngleStepType.VALUE
    public float timeBetweenShots; // if ProjectileAngleStepType.NONE

    public float projectileForce;
    public int projectileCount;
    public float projectileDeactivationTime;
}

public enum WeaponType
{
    PROJECTILE,
    AURA
}

public enum AuraType
{
    Garlic,
    Aura
}

public enum ProjectileType
{
    Dagger,
    Knife
}

public enum ProjectileDirectionType 
{ 
    MOUSE, 
    RANDOM, 
    PlayerMovement, 
    DOWN, 
    NearestEnemy 
}

public enum ProjectileAngleStepType 
{ 
    NONE, 
    VALUE, 
    EQUAL 
}