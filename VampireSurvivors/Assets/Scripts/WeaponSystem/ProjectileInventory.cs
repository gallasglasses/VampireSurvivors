using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RewardsInventory", menuName = "Weapon System/Rewards Inventory")]
public class RewardsInventory : ScriptableObject
{
    public List<RewardObject> rewardsInventory;
}
