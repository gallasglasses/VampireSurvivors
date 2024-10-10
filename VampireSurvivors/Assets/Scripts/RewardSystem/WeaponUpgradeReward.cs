using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponUpgradeReward", menuName = "Rewards/WeaponUpgradeReward")]
public class WeaponUpgradeReward : RewardObject
{
    //public WeaponReward.WeaponType weaponType;
    public WeaponUpgradeType weaponUpgradeType;

    public WeaponType weaponType;
    public AuraType auraType;
    public ProjectileType projectileType;
}

public enum WeaponUpgradeType 
{ 
    Cooldown, 
    AdditionalProjectile, 
    Damage, 
    EffectArea 
}