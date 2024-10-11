using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSettings
{
    public ProjectileType projectileType;

    public Sprite weaponSprite;
    public float cooldown;
    public float damage;

    public ProjectileDirectionType projectileDirectionType;
    //public IProjectileDirectionStrategy DirectionStrategy { get; set; }

    public ProjectileAngleStepType AngleStepType { get; set; }
    public float AngleStepValue { get; set; } // if ProjectileAngleStepType.VALUE
    public float TimeBetweenShots { get; set; } // if ProjectileAngleStepType.NONE

    public float projectileForce;
    public int projectileCount;
    public float projectileDeactivationTime;


    public void Initialize(WeaponReward weaponReward)
    {
        projectileType = weaponReward.projectileType;
        weaponSprite = weaponReward.weaponSprite;
        cooldown = weaponReward.cooldown;
        damage = weaponReward.damage;
        projectileDirectionType = weaponReward.projectileDirectionType;
        AngleStepType = weaponReward.projectileAngleStepType;
        AngleStepValue = weaponReward.projectileAngleStepValue;
        TimeBetweenShots = weaponReward.timeBetweenShots;
        projectileForce = weaponReward.projectileForce;
        projectileCount = weaponReward.projectileCount;
        projectileDeactivationTime = weaponReward.projectileDeactivationTime;
    }
    //    public void InitializeStrategy(Camera camera)
    //    {
    //        DirectionStrategy = ProjectileDirectionFactory.GetStrategy(projectileDirectionType);
    //    }
}
