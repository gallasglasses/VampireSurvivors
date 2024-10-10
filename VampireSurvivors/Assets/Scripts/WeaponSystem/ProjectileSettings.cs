using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSettings
{
    public ProjectileType projectileType;

    public Sprite weaponSprite;
    public float cooldown;
    public float damage;
    public IProjectileDirectionStrategy DirectionStrategy { get; set; }

    public ProjectileAngleStepType AngleStepType { get; set; }
    public float AngleStepValue { get; set; } // if ProjectileAngleStepType.VALUE
    public float TimeBetweenShots { get; set; } // if ProjectileAngleStepType.NONE

    public float projectileForce;
    public int projectileCount;
    public float projectileDeactivationTime;
}
