using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponUpgradeReward", menuName = "Rewards/WeaponUpgradeReward")]
public class WeaponUpgradeReward : RewardObject
{
    public WeaponUpgradeType weaponUpgradeType;

    public WeaponType weaponType;
    public AuraType auraType;
    public ProjectileType projectileType;
    public float upgradeValue;
    public int numberOfAdditionalProjectiles;
}
