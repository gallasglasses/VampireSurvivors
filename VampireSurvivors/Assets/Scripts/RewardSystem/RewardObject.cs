using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewReward", menuName = "Rewards/Reward")]
public class RewardObject : ScriptableObject
{
    public string rewardName;
    public string description;
    public Sprite icon; 
    public bool isAddedToManager;
}
